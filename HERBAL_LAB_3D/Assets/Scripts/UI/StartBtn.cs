using Unity.VisualScripting;
using UnityEngine;

public class StartBtn : MonoBehaviour
{
    public bool start = false;

    public bool Start
    {
        get { return start; }
        set { start = value; }
    }
}
