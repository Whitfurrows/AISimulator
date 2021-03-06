﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// goes straight to the goal by expanding only 1 path. is not optimal, but gives a path and is pretty fast. gets stuck often
/// </summary>
public class HillClimbing : GraphSearchAlgorithm {

	public override IEnumerator Search (List<Node> nodes, int size, Node start, Node goal, int framesPerSecond) {
        IsSearching = true;
        Debug.Log ("Searching path using Hill Climbing algorythm.");
        bool hasPath = false;

        bool[,] visited = new bool[size , size];
        cameFrom = new Dictionary<Node , Node> ();
        Stack<Node> frontier = new Stack<Node> ();
        Node current = new Node();

        frontier.Push (start);
        cameFrom[start] = null;

        while (frontier.Count > 0) {
            current = frontier.Pop ();

            UIUpdate (frontier.Count, CalculatePathLength (start, current));

            //visualize path to current
            GraphSearcher.VisualizePath (cameFrom, current , start);
            yield return new WaitForSeconds (1f/framesPerSecond); 

            if (current == goal) {
                hasPath = true;
                break;
            }

            float closestDistance = Vector2.Distance (current.pos , goal.pos);
            Node closestNeighbour = null;
            for (int i = 0 ; i < current.links.Count ; i++) {
                Node neighbour = current.links[i];

                // test shortest path
                float neighbourDist = Vector2.Distance (neighbour.pos , goal.pos);
                if (neighbourDist < closestDistance || closestDistance == float.MaxValue) {
                    closestNeighbour = neighbour;
                    closestDistance = neighbourDist;
                }
            }

            // extend path
            if (closestNeighbour != null) {
                if (!visited[(int) closestNeighbour.pos.x , (int) closestNeighbour.pos.y]) {
                    visited[(int) closestNeighbour.pos.x , (int) closestNeighbour.pos.y] = true;

                    cameFrom[closestNeighbour] = current;
                    frontier.Push (closestNeighbour);
                    UIIncrementEnqueuings ();
                }
            }
            else {
                Debug.LogFormat ("Stuck in local maxima. node {0}" , current.ID);
            }
        }

        SearchComplete(hasPath);
        yield break;
    }

}
