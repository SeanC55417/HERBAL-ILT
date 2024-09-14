using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StudentLogin : MonoBehaviour
{
    public Text studentName;
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
            studentName.text = nameInput.text;
        }
        else
        {
            studentName.text = "Guest";
        }
    }
}
