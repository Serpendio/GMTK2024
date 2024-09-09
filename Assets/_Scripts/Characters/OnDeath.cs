using UnityEngine;


public class OnDeath : MonoBehaviour, IOnDeath
{
    AudioSingle audioSingle;
    [SerializeField] GameObject DeathVFX;
    void Start()
    {
        audioSingle = AudioSingle.Instance;
        if (audioSingle == null)
        {
            Debug.LogWarning("missing AudioSingle instance!", this);
        }
    }
    void IOnDeath.Die(Component component)
    {
        audioSingle?.PlaySFX(audioSingle.slimeSquash, component.transform.position);
        Instantiate(this.DeathVFX, component.transform.position, component.transform.rotation);
        Destroy(component.transform.root.gameObject);
    }
}
