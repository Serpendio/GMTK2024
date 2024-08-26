using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnableTrigger : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }
    private void OnValidate()
    {
        Init();
    }
    void Init()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
