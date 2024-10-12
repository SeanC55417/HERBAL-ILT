using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Restart : MonoBehaviour
{
    public TextMeshProUGUI Text1;
    public TextMeshProUGUI Text2;
    public TextMeshProUGUI Text3;

    public TextMeshProUGUI Text4;

    public TextMeshProUGUI Column;
    public TextMeshProUGUI CompatOutput1;
    public TextMeshProUGUI CompatOutput2;
    

    public GameObject ClosedEmptyDrawer;
    public GameObject ClosedFilledDrawer;
    public GameObject OpenEmptyDrawer;
    public GameObject OpenFilledDrawer;

    public GameObject EmptySolventBase1;
    public GameObject EmptySolventBase2;


    public GameObject FullSolventBase1S1;
    public GameObject FullSolventBase1S2;
    public GameObject FullSolventBase1S3;
    public GameObject FullSolventBase1S4;

    public GameObject FullSolventBase2S1;
    public GameObject FullSolventBase2S2;
    public GameObject FullSolventBase2S3;
    public GameObject FullSolventBase2S4;

    public GameObject EmptyColumn;

    public GameObject FullColumnC1;
    public GameObject FullColumnC2;
    public GameObject FullColumnC3;

    public AudioSource MiscibilityAudioSource;
    public AudioSource Compatibility1AudioSource;
    public AudioSource Compatibility2AudioSource;
    public AudioClip nodataSFX;

    public bool quizStarted = false;

    public void ResetText()
    {
        Text1.text = "[None]";
        Text2.text = "[None]";
        Text3.text = "[None]";
        MiscibilityAudioSource.clip = nodataSFX;

        if (quizStarted == false)
        {
            Text4.text = "[None]";
            EmptySolventBase1.SetActive(true);
        }
    }

    public void ResetCompatText()
    {
        Column.text = "[None]";
        CompatOutput1.text = "[None]";
        CompatOutput2.text = "[None]";
        Compatibility1AudioSource.clip = nodataSFX;
        Compatibility2AudioSource.clip = nodataSFX;
    }

    public void ResetDrawer()
    {
        if(ClosedEmptyDrawer != null)
        {
            ClosedFilledDrawer.SetActive(false);
            ClosedEmptyDrawer.SetActive(true);
        }
        else if (OpenEmptyDrawer != null)
        {
            OpenEmptyDrawer.SetActive(false);
            OpenFilledDrawer.SetActive(true);  
        }
    }

    public void ResetSolventBase1()
    {
        if(FullSolventBase1S1 != null || FullSolventBase1S2 != null || FullSolventBase1S3 != null || FullSolventBase1S4 != null)
        {
            FullSolventBase1S1.SetActive(false);
            FullSolventBase1S2.SetActive(false);
            FullSolventBase1S3.SetActive(false);
            FullSolventBase1S4.SetActive(false);
            EmptySolventBase1.SetActive(true);
        }
    }

    public void ResetSolventBase2()
    {
        if(FullSolventBase2S1 != null || FullSolventBase2S2 != null || FullSolventBase2S3 != null || FullSolventBase2S4 != null)
        {
            FullSolventBase2S1.SetActive(false);
            FullSolventBase2S2.SetActive(false);
            FullSolventBase2S3.SetActive(false);
            FullSolventBase2S4.SetActive(false);
            EmptySolventBase2.SetActive(true);
        }
    }

    public void ResetColumn()
    {
        if (FullColumnC1 != null || FullColumnC2 != null || FullColumnC3 != null)
        {
            FullColumnC1.SetActive(false);
            FullColumnC2.SetActive(false);
            FullColumnC3.SetActive(false);
            EmptyColumn.SetActive(true);
        }
    }
}