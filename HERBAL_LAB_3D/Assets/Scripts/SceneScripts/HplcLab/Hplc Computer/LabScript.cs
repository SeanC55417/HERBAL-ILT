using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabScript : MonoBehaviour
{

    public InstructionScript instructionScript;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            instructionScript.postBulletPoints("bitch");
        }
    }
}
