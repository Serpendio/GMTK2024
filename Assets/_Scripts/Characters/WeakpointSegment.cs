using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakpointSegment : MonoBehaviour
{
    EnemyWeakpoint parentWeakpoint;
    [SerializeField] bool correctSide = true;

    private void Awake()
    {
        parentWeakpoint = transform.parent.GetComponentInParent<EnemyWeakpoint>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            parentWeakpoint.player = collision.transform.GetComponentInParent<Resources>();
            if (correctSide)
                parentWeakpoint.numCorrect++;
            else
                parentWeakpoint.numIncorrect++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (correctSide)
                parentWeakpoint.numCorrect--;
            else
                parentWeakpoint.numIncorrect--;
        }
    }
}
