using UnityEngine;

public class Resources : MonoBehaviour, IDamageable
{
    BonesFormation bonesFormation;
    IOnDeath onDeath;
    HitEffect hitEffect;
    public Rigidbody2D rb;
    public float bigResourceValue = 0, smallResourceValue = 0, currentSizeValue = 1;
    public float scaleSpeed = 1f;
    public float scaleOffset;
    public float BaseSize => 1 + scaleOffset;
    public float CurrentSize => transform.localScale.x * BaseSize;

    private void Awake()
    {
        bonesFormation = GetComponent<BonesFormation>();
        onDeath = GetComponent<IOnDeath>();
        hitEffect = GetComponent<HitEffect>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.localScale = Vector3.one * (currentSizeValue - scaleOffset);
    }

    public virtual void TryGrow()
    {
        currentSizeValue += Time.deltaTime * scaleSpeed;
        if (currentSizeValue > bigResourceValue)
        {
            currentSizeValue = bigResourceValue;
        }
        transform.localScale = Vector3.one * (currentSizeValue - scaleOffset);
        bonesFormation.ResetPositions();
    }

    public virtual void TryShrink()
    {
        currentSizeValue -= Time.deltaTime * scaleSpeed;
        if (currentSizeValue < smallResourceValue)
        {
            currentSizeValue = smallResourceValue;
        }
        transform.localScale = new Vector3(currentSizeValue - scaleOffset, currentSizeValue - scaleOffset, currentSizeValue - scaleOffset);
        bonesFormation.ResetPositions();
    }

    public virtual void AddBigResource(float value)
    {
        bigResourceValue += value;
        if (bigResourceValue < smallResourceValue)
        {
            bigResourceValue = smallResourceValue;
        }
    }

    public virtual void AddSmallResource(float value)
    {
        smallResourceValue += value;
        if (smallResourceValue > bigResourceValue)
        {
            smallResourceValue = bigResourceValue;
        }
        if (smallResourceValue < 0.4)
        {
            smallResourceValue = 0.4f;
        }
    }

    public float TakeDamage(DamageInfo damage)
    {
        float overkill = 0f;
        if (damage.AffectsBigResource)
        {
            if (damage.Amount > bigResourceValue)
                overkill = damage.Amount - bigResourceValue;

            transform.localScale -= (damage.Amount - overkill) * Vector3.one;

            AddBigResource(-damage.Amount + overkill);

            if (CurrentSize < .4f * BaseSize)
                onDeath.Die(transform);
            else
                hitEffect.PlayBigEffect();
        }
        else
        {
            if (damage.Amount > smallResourceValue)
                overkill = damage.Amount - smallResourceValue;

            transform.localScale += (damage.Amount - overkill) * Vector3.one;
            AddSmallResource(-damage.Amount + overkill);

            if (CurrentSize > 2.1f * BaseSize)
                onDeath.Die(transform);
            else
                hitEffect.PlaySmallEffect();
        }

        return overkill;
    }
}
