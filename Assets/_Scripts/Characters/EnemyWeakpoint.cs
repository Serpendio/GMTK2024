using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWeakpoint : MonoBehaviour
{
    public bool playerShouldBeBigger = false;
    public bool affectsBigResource = false;
    public float giveDamageSpeed = 1f;
    public float takeDamageSpeed = 1f;
    public float resourceMultiplier = 0.5f;
    public float separationForce = 10f;
    public int numIncorrect = 0;
    public int numCorrect = 0;
    public Resources player;
    private Resources self;
    AudioSingle audioSingle;
    bool playerTakeDamage = false;
    bool selfTakeDamage = false;
    [SerializeField] float forceDelay = 1;
    bool shouldPlayPrimeHitSFX = true;
    bool shouldPlayHitSFX = true;
    bool shouldApplyForce = true;


    void Awake()
    {
        self = GetComponent<Resources>();
    }
    private void Start()
    {
        audioSingle = AudioSingle.Instance;
        if (audioSingle == null)
        {
            Debug.LogWarning("missing AudioSingle instance!", this);
        }
    }

    // Update is called once per frame
    void Update()
    {
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
            player.transform.localScale -= giveDamageSpeed * Time.deltaTime * Vector3.one;
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

                audioSingle?.PlaySFX(audioSingle?.primeSlimeHit);
                audioSingle?.PlaySFX(audioSingle?.slimeSquash, Vector3.zero);
            }
            else if (shouldPlayPrimeHitSFX)
            {
                StartCoroutine(Cr_SFXPrimeHitCheck());
                audioSingle?.PlaySFX(audioSingle?.primeSlimeHit);
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
                audioSingle?.PlaySFX(audioSingle?.slimeSquash, self.rb.position);
                Destroy(gameObject);
            }
            else if (shouldPlayHitSFX)
            {
                StartCoroutine(Cr_SFXHitCheck());
                audioSingle?.PlaySFX(audioSingle?.slimeHit, self.rb.position);
            }
        }
    }
    private void FixedUpdate()
    {

        if(!(playerTakeDamage || selfTakeDamage))
            return;
        if (!shouldApplyForce)
        {
            playerTakeDamage = false;
            selfTakeDamage = false;
            return;
        }

        StartCoroutine(Cr_ApplyForceCheck());

        if (playerTakeDamage || selfTakeDamage)
        {
            player.rb.AddForce(self.rb.position.DirectionTo(player.rb.position) * separationForce * Time.deltaTime, ForceMode2D.Impulse);
            playerTakeDamage = false;
        }
        if (selfTakeDamage)
        {
            self.rb.AddForce(player.rb.position.DirectionTo(self.rb.position) * separationForce * Time.deltaTime, ForceMode2D.Impulse);
            selfTakeDamage = false;
        }
    }
    IEnumerator Cr_SFXPrimeHitCheck()
    {
        shouldPlayPrimeHitSFX= false;
    
        yield return new WaitForSecondsRealtime(0.3f);
        shouldPlayPrimeHitSFX= true;
    }
    IEnumerator Cr_SFXHitCheck()
    {
        shouldPlayHitSFX = false;
    
        yield return new WaitForSecondsRealtime(0.3f);
        shouldPlayHitSFX = true;
    }
    IEnumerator Cr_ApplyForceCheck()
    {
        shouldApplyForce = false;

        yield return new WaitForSeconds(1f);
        shouldApplyForce = true;
    }
    private void OnDrawGizmos()
    {
        if(player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(self.rb.position, player.rb.position);
        }
    }
}
