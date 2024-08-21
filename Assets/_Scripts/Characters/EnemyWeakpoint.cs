using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(IOnDeath))]
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
    bool shouldPlayPrimeHitSFX = true;
    bool shouldPlayHitSFX = true;
    bool shouldApplyForce = true;
    float sizeFactor = 1;
    IOnDeath onDeath;

    void Awake()
    {
        self = GetComponent<Resources>();
        onDeath= GetComponent<IOnDeath>();
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

        if(playerTakeDamage || selfTakeDamage)
        {
            var sizeRatio = transform.localScale.x / player.transform.localScale.x;
            if (playerShouldBeBigger)
            {
                sizeFactor = sizeRatio;
            }
            else
            {
                sizeFactor /= sizeRatio;
            }
        }
        if (playerTakeDamage)
        {
            var damage = giveDamageSpeed * sizeFactor * Time.deltaTime;
            player.transform.localScale -= damage * Vector3.one;
            if (affectsBigResource)
            {
                player.AddBigResource(-damage * resourceMultiplier);
            }
            else
            {
                player.AddSmallResource(-damage * resourceMultiplier);
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
            var damage = takeDamageSpeed * 1 / sizeFactor * Time.deltaTime;
            bool shouldDie;
            if (affectsBigResource)
            {
                transform.localScale -= damage * Vector3.one;
                self.AddBigResource(-damage * resourceMultiplier);
                player.AddBigResource(damage * resourceMultiplier);
                shouldDie = transform.localScale.x < .4f;
            }
            else
            {
                transform.localScale += damage * Vector3.one;
                self.AddSmallResource(-damage * resourceMultiplier);
                player.AddSmallResource(damage * resourceMultiplier);
                shouldDie = transform.localScale.x > 2.1f;
            }

            if (shouldDie)
            {
                onDeath.Die(self.rb);
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
            player.rb.AddForce(self.rb.position.DirectionTo(player.rb.position) * transform.localScale.x * separationForce * Time.deltaTime, ForceMode2D.Impulse);
            playerTakeDamage = false;
        }
        if (selfTakeDamage)
        {
            self.rb.AddForce(player.rb.position.DirectionTo(self.rb.position) * player.transform.localScale.x * separationForce * Time.deltaTime, ForceMode2D.Impulse);
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
