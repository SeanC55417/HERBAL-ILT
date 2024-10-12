using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 direction;
    public float playerSpeed = 10f;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        direction = new Vector3(x, 0f, z).normalized;
        Vector3 move = transform.right * x + transform.forward * z - transform.up * 9.8f;
        controller.Move(move * playerSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            Application.Quit();
        }
    }
}
