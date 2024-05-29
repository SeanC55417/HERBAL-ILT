using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentLogin : MonoBehaviour
{
    public Text name;
    public GameObject studentInfoCard;
    public InputField nameInput;
    public InputField studentIDInput;

    public void ShowStudentLogin()
    {
        studentInfoCard.SetActive(true);
    }

    public void HideStudentLogin()
    {
        studentInfoCard.SetActive(false);
    }

    public void SaveStudentInfo()
    {
        if (nameInput.text != "")
        {
            name.text = nameInput.text;
        }
        else
        {
            name.text = "Guest";
        }
    }
}
