﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Camera))]
public class CameraController : MonoBehaviour {

    [SerializeField]
    private float border;

    private Camera cam;

    private void Awake () {
        cam = GetComponent<Camera> ();
    }

    private void OnEnable () {
        TreeGenerator.SetCameraEvent += SetCamera;
    }
    private void OnDisable () {
        TreeGenerator.SetCameraEvent -= SetCamera;
    }

    private void SetCamera (int depth, int branching, float stepY) {
        float totalLeafs = Mathf.Pow (branching, depth);
        float horizontalUnits = totalLeafs;

        float sizeY = ((stepY * depth) + 1 ) / 2;

        float sizeX = ((horizontalUnits + 1) / 16 * 12) / 2;

        cam.orthographicSize = Mathf.Max (sizeX, sizeY) + border;

        float posY = stepY * depth / 2;
        cam.transform.position = new Vector3 (0f, - posY, cam.transform.position.z);
    }
}
