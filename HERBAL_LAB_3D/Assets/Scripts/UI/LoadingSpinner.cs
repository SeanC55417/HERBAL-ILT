using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LoadingSpinner : MonoBehaviour
{
    public float rotationSpeed = 200f;    // Speed of rotation

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
// public class LoadingSpinner : MonoBehaviour
// {
//     public float rotationSpeed = 200f;    // Speed of rotation
//     public float stopAfterSeconds = 2f;   // Time after which rotation stops
//     public Canvas canvas;

//     private bool isRotating = true;

//     void Start()
//     {
//         StartCoroutine(StopRotationAfterTime(stopAfterSeconds));
//     }

//     void Update()
//     {
//         if (isRotating)
//         {
//             transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
//         }
//     }

//     IEnumerator StopRotationAfterTime(float seconds)
//     {
//         yield return new WaitForSeconds(seconds);
//         isRotating = false;
//         canvas.gameObject.SetActive(false);
//     }
// }
