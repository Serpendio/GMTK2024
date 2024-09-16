using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Resources))]
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
    bool playerTakeDamage = false;
    bool selfTakeDamage = false;
    bool shouldApplyForce = true;
    float sizeFactor = 1;

    void Awake()
    {
        self = GetComponent<Resources>();
    }
    private void OnValidate()
    {
        player ??= FindObjectOfType<PlayerResources>();
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
            player.TakeDamage(new DamageInfo(this.gameObject, damage, this.affectsBigResource));
        }

        if (selfTakeDamage)
        {
            var damage = takeDamageSpeed * 1 / sizeFactor * Time.deltaTime;
            var overkill = self.TakeDamage(new DamageInfo(player.gameObject, damage, this.affectsBigResource));
            damage -= overkill;
            if (affectsBigResource)
                player.AddBigResource(damage * resourceMultiplier);
            else
                player.AddSmallResource(damage * resourceMultiplier);
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