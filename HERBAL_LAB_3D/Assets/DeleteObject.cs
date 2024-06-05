using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObject : MonoBehaviour
{
    public void destroyItem(GameObject item)
    {
        Destroy(item);
    }   
}
