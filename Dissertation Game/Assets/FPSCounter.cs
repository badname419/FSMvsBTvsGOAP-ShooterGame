using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsDisplay;
    public Text averageFPSDisplay;
    int framesPassed = 0;
    float fpsTotal = 0f;
    public Text minFPSDisplay, maxFPSDisplay;
    float minFPS = Mathf.Infinity;
    float maxFPS = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        fpsDisplay.text = "Current FPS: " + fps;
        fpsTotal += fps;
        framesPassed++;
        averageFPSDisplay.text = "Average: " + (fpsTotal / framesPassed);

        if (fps > maxFPS && framesPassed > 10)
        {
            maxFPS = fps;
            maxFPSDisplay.text = "Max: " + maxFPS;
        }

        if (fps < minFPS && framesPassed > 10)
        {
            minFPS = fps;
            minFPSDisplay.text = "Min: " + minFPS;
        }
    }
}
