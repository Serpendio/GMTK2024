using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ScaleParticles : MonoBehaviour
{
    ParticleSystem.MainModule mainModule;
    private void Awake()
    {
        mainModule = GetComponent<ParticleSystem>().main;
    }
    void Update()
    {
        mainModule.startSize = transform.lossyScale.x;
    }
}
