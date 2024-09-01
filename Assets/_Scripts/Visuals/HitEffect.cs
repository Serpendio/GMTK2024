using System.Collections;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] float duration = 0.2f;
    int flashValue = Shader.PropertyToID("_FlashValue");
    SpriteRenderer[] renderers;
    Material[] materials;

    private void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }
    public void PlayEffect()
    {
        StartCoroutine(Cr_AppyEffect());
    }

    IEnumerator Cr_AppyEffect()
    {
        yield return StartCoroutine(Cr_Lerp(0f, 1f));
        yield return StartCoroutine(Cr_Lerp(1f, 0f));
    }

    IEnumerator Cr_Lerp(float start, float end)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;

            float lerp = Mathf.Lerp(start, end, time / duration);
            foreach (var material in materials)
            {
                material.SetFloat(flashValue, lerp);
            }
            yield return null;
        }
    }
}
