using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Resources))]
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
    HitEffect hitEffect;

    void Awake()
    {
        self = GetComponent<Resources>();
        onDeath = GetComponent<IOnDeath>();
        hitEffect = GetComponent<HitEffect>();
    }
    private void Start()
    {
        audioSingle = AudioSingle.Instance;
        if (audioSingle == null)
        {
            Debug.LogWarning("missing AudioSingle instance!", this);
        }
    }
    private void OnValidate()
    {
        player = FindObjectOfType<PlayerResources>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
            return;

        if (numCorrect > 0)
        {
            selfTakeDamage = (playerShouldBeBigger && player.CurrentSize > self.CurrentSize)
                || (!playerShouldBeBigger && player.CurrentSize < self.CurrentSize);
            playerTakeDamage = !selfTakeDamage;
        }
        else if (numIncorrect > 0)
        {
            playerTakeDamage = true;
        }

        if (playerTakeDamage || selfTakeDamage)
        {
            var sizeRatio = self.CurrentSize / player.CurrentSize;
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

            if (player.CurrentSize < .4f * player.BaseSize)
            {
                player.GetComponent<IOnDeath>()
                    .Die(player.transform);
            }
            else if (shouldPlayPrimeHitSFX)
            {
                StartCoroutine(Cr_SFXPrimeHitCheck());
                audioSingle?.PlaySFX(audioSingle?.primeSlimeHit);
                player.GetComponent<HitEffect>().PlayEffect();
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
                shouldDie = self.CurrentSize < .4f * self.BaseSize;
            }
            else
            {
                transform.localScale += damage * Vector3.one;

                self.AddSmallResource(-damage * resourceMultiplier);
                player.AddSmallResource(damage * resourceMultiplier);
                shouldDie = self.CurrentSize > 2.1f * self.BaseSize;
            }

            if (shouldDie)
            {
                onDeath.Die(self.rb);
            }
            else if (shouldPlayHitSFX)
            {
                StartCoroutine(Cr_SFXHitCheck());
                audioSingle?.PlaySFX(audioSingle?.slimeHit, self.rb.position);
                hitEffect?.PlayEffect();
            }
        }
    }
    private void FixedUpdate()
    {
        if (!(playerTakeDamage || selfTakeDamage))
            return;
        if (!shouldApplyForce)
        {
            playerTakeDamage = false;
            selfTakeDamage = false;
            return;
        }

        StartCoroutine(Cr_ApplyForceCheck());
        //add min/max velocity?
        if (playerTakeDamage || selfTakeDamage)
        {
            player.rb.AddForce(self.rb.position.DirectionTo(player.rb.position) * player.CurrentSize * separationForce * Time.deltaTime, ForceMode2D.Impulse);
            playerTakeDamage = false;
        }
        if (selfTakeDamage)
        {
            self.rb.AddForce(player.rb.position.DirectionTo(self.rb.position) * self.CurrentSize * separationForce * Time.deltaTime, ForceMode2D.Impulse);
            selfTakeDamage = false;
        }
    }
    IEnumerator Cr_SFXPrimeHitCheck()
    {
        shouldPlayPrimeHitSFX = false;

        yield return new WaitForSecondsRealtime(0.3f);
        shouldPlayPrimeHitSFX = true;
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
        if (playerTakeDamage || selfTakeDamage)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(self.rb.position, player.rb.position);
        }
    }
}