using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class ControlCanvas : MonoBehaviour
{
    private Transform target;
    private Canvas canvas;

    void Start()
    {
        target = UnityEngine.Camera.main.transform;
        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        canvas.transform.LookAt(target);
        canvas.transform.Rotate(0, 180, 0);
    }
}