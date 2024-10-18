using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.UIElements;

public class VrTextPanel : MonoBehaviour
{

    public GameObject Card;
    public GameObject HudText;
    // Duration for scaling effect
    public float scaleDuration = 0.5f;
    public Transform[] textLocations;
    private Vector3 cardScale;
    private bool vrActive = true;

    void Start()
    {
        // if (FindAnyObjectByType<XROrigin>())
        // {
        //     vrActive = true;
        // }
        // else
        // {
        //     Card.SetActive(false);
        // }
        cardScale = Card.transform.localScale;
    }

    public void CardMoveTo(int positionIdx)
    {
        if (vrActive)
        {
            CardLeave();
            CardEnter(positionIdx);
        }
    }

    // Scale to zero (CardLeave)
    public void CardLeave()
    {
        HudText.transform.localScale = Vector3.zero;
        StartCoroutine(ScaleOverTime(Card, Vector3.zero, scaleDuration));

    }

    // Scale back to original size (CardEnter)
    public void CardEnter(int positionIdx)
    {

        if (positionIdx != -1 && positionIdx < textLocations.Length)
        {
            Card.transform.SetLocalPositionAndRotation(textLocations[positionIdx].localPosition, textLocations[positionIdx].rotation);
        }

        StartCoroutine(ScaleOverTime(Card, cardScale, scaleDuration));  // Assuming original scale is (1,1,1)
        HudText.transform.localScale = Vector3.one;
    }


    // Coroutine to scale objects over time
    private IEnumerator ScaleOverTime(GameObject obj, Vector3 targetScale, float duration)
    {
        Vector3 initialScale = obj.transform.localScale;
        float time = 0;

        while (time < duration)
        {
            obj.transform.localScale = Vector3.Lerp(initialScale, targetScale, time / duration);
            time += Time.deltaTime;
            yield return null; // Wait until next frame
        }

        obj.transform.localScale = targetScale;  // Ensure exact target scale at the end
    }
}
