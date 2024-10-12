using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCanvasTimed : MonoBehaviour
{
    public Canvas canvas;

    public void ShowCanvasFor(float time){
        canvas.gameObject.SetActive(true);
        Debug.Log("Clicked Show Canvas");
        StartCoroutine(Showing(time));
    }

    IEnumerator Showing(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvas.gameObject.SetActive(false);
    }   
}
