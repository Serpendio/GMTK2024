using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Resources : MonoBehaviour
{
    public TextMeshProUGUI bigResource, smallResource, currentSize;
    public Viewport viewport;
    public float bigResourceValue = 0, smallResourceValue = 0, currentSizeValue = 1;
    public float scaleSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        bigResource.text = bigResourceValue.ToString();
        smallResource.text = smallResourceValue.ToString();
        currentSize.text = currentSizeValue.ToString();
        transform.localScale = Vector3.one * currentSizeValue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            currentSizeValue += Time.deltaTime * scaleSpeed;
            if (currentSizeValue > bigResourceValue)
            {
                currentSizeValue = bigResourceValue;
            }
            currentSize.text = currentSizeValue.ToString();
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);
            viewport.UpdateScale(currentSizeValue);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentSizeValue -= Time.deltaTime * scaleSpeed;
            if (currentSizeValue < smallResourceValue)
            {
                currentSizeValue = smallResourceValue;
            }
            currentSize.text = currentSizeValue.ToString();
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);
            viewport.UpdateScale(currentSizeValue);
        }
    }

    public void AddBigResource(float value)
    {
        bigResourceValue += value;
        bigResource.text = bigResourceValue.ToString();
    }

    public void AddSmallResource(float value)
    {
        smallResourceValue += value;
        smallResource.text = smallResourceValue.ToString();
    }
}
