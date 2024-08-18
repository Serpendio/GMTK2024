using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Resources : MonoBehaviour
{
    BonesFormation bonesFormation;
    public float bigResourceValue = 0, smallResourceValue = 0, currentSizeValue = 1;
    public float scaleSpeed = 1f;
    public float scaleOffset;

    private void Awake()
    {
        bonesFormation = GetComponent<BonesFormation>();
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        transform.localScale = Vector3.one * currentSizeValue;
    }

    public virtual void TryGrow()
    {
        currentSizeValue += Time.deltaTime * scaleSpeed;
        if (currentSizeValue > bigResourceValue)
        {
            currentSizeValue = bigResourceValue;
        }
        transform.localScale = new Vector3(currentSizeValue - scaleOffset, currentSizeValue - scaleOffset, currentSizeValue - scaleOffset);
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
}
