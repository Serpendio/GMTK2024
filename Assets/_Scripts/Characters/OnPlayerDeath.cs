using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnPlayerDeath : MonoBehaviour, IOnDeath
{
    AudioSingle audioSingle;
    void Start()
    {
        audioSingle = AudioSingle.Instance;
        if (audioSingle == null)
        {
            Debug.LogWarning("missing AudioSingle instance!", this);
        }
    }
    public void Die(Component component)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        audioSingle?.PlaySFX(audioSingle?.primeSlimeHit);
        audioSingle?.PlaySFX(audioSingle?.slimeSquash, Vector3.zero);
    }
}
