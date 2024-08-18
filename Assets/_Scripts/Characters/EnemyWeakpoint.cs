using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWeakpoint : MonoBehaviour
{
    public bool playerShouldBeBigger = false;
    public bool affectsBigResource = false;
    public float giveDamageSpeed = 1f;
    public float takeDamageSpeed = 1f;
    public float resourceMultiplier = 0.5f;
    public int numIncorrect = 0;
    public int numCorrect = 0;
    public Resources player;
    private Resources self;

    void Awake()
    {
        self = GetComponent<Resources>();
    }

    // Update is called once per frame
    void Update()
    {
        bool playerTakeDamage = false;
        bool selfTakeDamage = false;
        if (numCorrect > 0)
        {
            selfTakeDamage = (playerShouldBeBigger && player.transform.localScale.x > transform.localScale.x)
                || (!playerShouldBeBigger && player.transform.localScale.x < transform.localScale.x);
            playerTakeDamage = !selfTakeDamage;
        }
        else if (numIncorrect > 0)
        {
            playerTakeDamage = true;
        }

        if (playerTakeDamage)
        {
            player.transform.localScale -=  giveDamageSpeed * Time.deltaTime * Vector3.one;
            if (affectsBigResource)
            {
                player.AddBigResource(-giveDamageSpeed * Time.deltaTime * resourceMultiplier);
            }
            else
            {
                player.AddSmallResource(-giveDamageSpeed * Time.deltaTime * resourceMultiplier);
            }

            if (player.transform.localScale.x < .4f)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (selfTakeDamage)
        {
            transform.localScale -= takeDamageSpeed * Time.deltaTime * Vector3.one;
            if (affectsBigResource)
            {
                self.AddBigResource(-takeDamageSpeed * Time.deltaTime * resourceMultiplier);
                player.AddBigResource(takeDamageSpeed * Time.deltaTime * resourceMultiplier);
            }
            else
            {
                self.AddSmallResource(-takeDamageSpeed * Time.deltaTime * resourceMultiplier);
                player.AddSmallResource(takeDamageSpeed * Time.deltaTime * resourceMultiplier);
            }

            if (transform.localScale.x < .4f)
            {
                Destroy(gameObject);
            }
        }
    }
}
