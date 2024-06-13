using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MenuMode : MonoBehaviour
{
    public Button PumpButton;
    public Button BatchButton;
    public Button addBatchTable;
    public Button Option1;
    public Button Option2;
    public Button Option3;
    public Button Option4;
    public Button Option5;
    public Button Option6;
    public Button Option7;

    public GameObject Screen;
    public Material image1;
    public Material image2;

    public GameObject PumpMenu;

    private bool box1 = false;
    private bool box2 = false;
    private bool box3 = false;
    private bool box4 = false;
    private bool box5 = false;
    private bool box6 = false;
    private bool box7 = false;

    private bool allBoxesDeleted = false;

    public void menuSpawn()
    {
        PumpMenu.SetActive(true);
        Screen.GetComponent<MeshRenderer>().material = image1;
    }

    private void checkAllBoxes()
    {
        if (box1 && box2 && box3 && box4 && box5 && box6 && box7)
        {
            allBoxesDeleted = true;
        }
    }

    public void onClickDeleteSelf1()
    {
        Destroy(Option1.gameObject);
        box1 = true;
    }

    public void onClickDeleteSelf2()
    {
        Destroy(Option2.gameObject);
        box2 = true;
    }

    public void onClickDeleteSelf3()
    {
        Destroy(Option3.gameObject);
        box3 = true;
    }

    public void onClickDeleteSelf4()
    {
        Destroy(Option4.gameObject);
        box4 = true;
    }

    public void onClickDeleteSelf5()
    {
        Destroy(Option5.gameObject);
        box5 = true;
    }

    public void onClickDeleteSelf6()
    {
        Destroy(Option6.gameObject);
        box6 = true;
    }

    public void onClickDeleteSelf7()
    {
        Destroy(Option7.gameObject);
        box7 = true;
    }

    public void onClickDeleteSelfPump()
    {
        Destroy(PumpButton.gameObject);

    }

    public void batchSpawn()
    {
        checkAllBoxes();
        if (allBoxesDeleted) {
            BatchButton.transform.localPosition = new Vector3(-76, 365, -5000);
            addBatchTable.transform.localPosition = new Vector3(-3, -96, 0);
            Screen.GetComponent<MeshRenderer>().material = image2;
        }
    }
}
