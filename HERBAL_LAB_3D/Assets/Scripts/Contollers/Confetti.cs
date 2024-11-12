using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Confetti : MonoBehaviour
{
    public ParticleSystem confetti;
    public TextMeshProUGUI playerHudText;

    public void StartConfetti(string hud_text, float duration_sec)
    {
        if (confetti != null)
        {   
            // playerHudText.text = hud_text;
            confetti.Play();
            StartCoroutine(StopConfettiAfterDelay(duration_sec));
        }
    }

    IEnumerator StopConfettiAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // playerHudText.text = "";
        confetti.Stop();
    }
}
