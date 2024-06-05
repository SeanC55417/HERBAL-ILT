using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Singleton: A class that can only have one instance
public class gameManager : MonoBehaviour
{
private static gameManager instance;

public static gameManager Instance{
    get{ 
        if (instance == null){
            instance = FindObjectOfType<gameManager>();
        }
        return instance;
    }
  }
 
 
 
 
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
