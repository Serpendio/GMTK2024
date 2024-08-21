using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class VFX : MonoBehaviour
{
    ParticleSystem particles;
    private void Awake()
    {
        particles = GetComponent<ParticleSystem>();
    }
    private void Start()
    {
        Destroy(this.gameObject, particles.main.duration);
    }
}
