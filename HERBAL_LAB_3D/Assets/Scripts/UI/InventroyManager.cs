using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<Item> Items = new List<Item>();
    public GameObject ItemSlot;
    // public GameObject RightHand;
    Image ItemIcon;
    TextMeshProUGUI ItemName;
    public ScrollRect ScrollView; // Reference to the ScrollView
    public RectTransform Content; // Reference to the Content inside the 
    public Image ScrollBarImage;

    private int selectedIndex = -1; // Index of the selected item
    private Color selectedColor = Color.yellow; // Color for the selected item
    private Color normalColor = Color.white; // Normal color for items

    public void Start()
    {
        // Create a new color with the same RGB values but a different alpha
        ScrollBarImage.color = new Color(ScrollBarImage.color.r, ScrollBarImage.color.g, ScrollBarImage.color.b, 0f);
    }

    private void Update()
    {
        HandleScrollInput();
    }

    private void ShowInventory()
    {
        ScrollBarImage.color = new Color(ScrollBarImage.color.r, ScrollBarImage.color.g, ScrollBarImage.color.b, .6f);
    }

    public void PickupItem(Item item)
    {
        ShowInventory();
        GameObject InventorySlot = Instantiate(ItemSlot, Content.transform);
        ItemIcon = InventorySlot.transform.GetChild(0).GetComponent<Image>();
        ItemName = InventorySlot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();


        ItemIcon.sprite = item.Icon;
        ItemName.text = item.ItemName;
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        // Find the slot corresponding to the item
        for (int i = 0; i < Content.childCount; i++)
        {
            GameObject slot = Content.GetChild(i).gameObject;
            Image icon = slot.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI name = slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

            if (icon.sprite == item.Icon && name.text == item.ItemName)
            {
                Destroy(slot);
                break;
            }
        }

        Items.Remove(item);
    }

    private void HandleScrollInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0 && Items.Count > 0)
            {
                selectedIndex = (selectedIndex - 1 + Items.Count) % Items.Count;
            }
            else if (Input.mouseScrollDelta.y < 0 && Items.Count > 0)
            {
                selectedIndex = (selectedIndex + 1) % Items.Count;
            }
            UpdateSelectedItem();
        }
    }

    private void UpdateSelectedItem()
    {
        for (int i = 0; i < Content.childCount; i++)
        {
            GameObject slot = Content.GetChild(i).gameObject;
            Image background = slot.GetComponent<Image>();

            if (i == selectedIndex)
            {
                background.color = selectedColor;
                Debug.Log("alpha" + background.color.a);

                // Clear existing objects in RightHand
                // foreach (Transform child in RightHand.transform)
                // {
                //     Destroy(child.gameObject);
                // }

                // // Instantiate the selected item's object into RightHand
                // GameObject HeldObject = Instantiate(Items[selectedIndex].Object, RightHand.transform);
                // Rigidbody rb = HeldObject.GetComponent<Rigidbody>(); // Get reference to Rigidbody
                // if (rb != null)
                // {
                //     Destroy(rb); // Remove the Rigidbody component
                // }
                // HeldObject.layer = LayerMask.NameToLayer("Player UI");
                // HeldObject.transform.localPosition = new Vector3(-0.35f, 0.03f, 0.589f);
                // HeldObject.transform.localScale = new Vector3(1f, 1/0.3f, 2f); // Example scaling to double the size
                // Vector3 currentScale = HeldObject.transform.localScale;
                // HeldObject.transform.localScale = new Vector3(1f, 1f, .2f);

            }
            else
            {
                background.color = normalColor;
            }
        }
    }
}
