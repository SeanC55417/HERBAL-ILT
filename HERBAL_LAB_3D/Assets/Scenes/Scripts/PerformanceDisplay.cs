using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    int gcCollectionCount = 0;
    long allocatedMemory = 0;
    float marginX = 10.0f; // Horizontal margin
    float marginY = 10.0f; // Vertical margin
    int fontSize = 24; // Increased font size

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        gcCollectionCount = System.GC.CollectionCount(0);
        allocatedMemory = System.GC.GetTotalMemory(false) / (1024 * 1024); // Convert to MB
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle
        {
            alignment = TextAnchor.UpperLeft,
            fontSize = fontSize,
            normal = { textColor = Color.white }
        };

        // Adjust the position of each label by adding marginX and marginY
        float labelHeight = h * 2 / 100;

        // FPS
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string fpsText = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(new Rect(marginX, marginY, w, labelHeight), fpsText, style);

        // // Memory Usage
        // string memoryText = $"Memory Usage: {allocatedMemory} MB";
        // GUI.Label(new Rect(marginX, marginY + labelHeight, w, labelHeight), memoryText, style);

        // // CPU Time
        // float cpuTime = Time.deltaTime * 1000.0f; // ms
        // string cpuText = $"CPU Time: {cpuTime:0.0} ms";
        // GUI.Label(new Rect(marginX, marginY + 2 * labelHeight, w, labelHeight), cpuText, style);

        // // GPU Time (Placeholder)
        // float gpuTime = 0.0f; // Placeholder, real value needs more complex setup
        // string gpuText = $"GPU Time: {gpuTime:0.0} ms (Placeholder)";
        // GUI.Label(new Rect(marginX, marginY + 3 * labelHeight, w, labelHeight), gpuText, style);

        // // Garbage Collection
        // string gcText = $"GC Collections: {gcCollectionCount}";
        // GUI.Label(new Rect(marginX, marginY + 4 * labelHeight, w, labelHeight), gcText, style);
    }
}
