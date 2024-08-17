using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeakpoint : MonoBehaviour
{
    public bool shouldBeBigger = false;
    public bool givesBigResource = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (shouldBeBigger)
            {
                if (transform.parent.localScale.x < collision.transform.localScale.x)
                {
                    if (givesBigResource)
                    {
                        collision.gameObject.GetComponent<Resources>().AddBigResource(.4f);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Resources>().AddSmallResource(.4f);
                    }
                    transform.parent.localScale -= Vector3.one * .4f;
                    if (transform.parent.localScale.x < .4f)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
                else
                {
                    collision.transform.localScale -= Vector3.one * .4f;
                    if (collision.transform.localScale.x < .4f)
                    {
                        Destroy(collision.transform.gameObject);
                    }
                }
            }
            else
            {
                if (transform.parent.localScale.x > collision.transform.localScale.x)
                {
                    if (givesBigResource)
                    {
                        collision.gameObject.GetComponent<Resources>().AddBigResource(.4f);
                    }
                    else
                    {
                        collision.gameObject.GetComponent<Resources>().AddSmallResource(.4f);
                    }
                    transform.parent.localScale -= Vector3.one * .4f;
                    if (transform.parent.localScale.x < .4f)
                    {
                        Destroy(transform.parent.gameObject);
                    }
                }
                else
                {
                    collision.transform.localScale -= Vector3.one * .4f;
                    if (collision.transform.localScale.x < .4f)
                    {
                        Destroy(collision.transform.gameObject);
                    }
                }
            }
        }
    }
}
