using UnityEngine;

public class FloatingNotebookScript : MonoBehaviour
{
    public GameObject player;    
    public Transform menuTransform; // The transform of the menu object
    public Vector3 offset = new Vector3(0, 0, 2); // Offset from the center of the screen
    public bool isVisible = true; // Whether the menu is initially visible or not
    public float maxDistance = 3f;

    void Start()
    {
        if (menuTransform != null)
        {
            menuTransform.gameObject.SetActive(isVisible);
        }
        else
        {
            Debug.LogError("Menu Transform is not set.");
        }

        // setNotebook();
    }

    public void setNotebook()
    {
        if (player == null)
        {
            Debug.LogError("Player is not set.");
            return;
        }

        // Calculate the desired position with the offset applied in world space
        Vector3 targetPosition = Camera.main.transform.position + Camera.main.transform.forward * offset.z + Camera.main.transform.right * offset.x + Camera.main.transform.up * offset.y;


        // Set the position and make the notebook face the player
        transform.position = targetPosition;
        transform.LookAt(player.transform.position);
        transform.Rotate(0, 180, 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (menuTransform != null)
            {
                menuTransform.gameObject.SetActive(isVisible);
            }
            setNotebook();
        }
    }
}
