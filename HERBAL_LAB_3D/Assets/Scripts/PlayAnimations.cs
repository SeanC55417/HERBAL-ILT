using UnityEngine;

public class MultiAnimationTrigger : MonoBehaviour
{
    public Animator animator; // Reference to the Animator component
    public string[] animationNames; // Names of the animations to play
    public int[] layerIndexes; // Corresponding layer indexes

    public void PlayAnimations()
    {
        if (animator != null && animationNames.Length == layerIndexes.Length)
        {
            for (int i = 0; i < animationNames.Length; i++)
            {
                animator.Play(animationNames[i], layerIndexes[i], 0f);
            }
        }
    }
}
