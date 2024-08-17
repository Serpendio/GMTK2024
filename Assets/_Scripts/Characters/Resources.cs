using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class Resources : MonoBehaviour
{
    public TextMeshProUGUI bigResource, smallResource, currentSize;
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
        if (Input.GetKey(KeyCode.UpArrow) && bigResourceValue > 0)
        {
            currentSizeValue += Time.deltaTime * scaleSpeed;
            bigResourceValue -= Time.deltaTime * scaleSpeed;
            bigResource.text = bigResourceValue.ToString();
            currentSize.text = currentSizeValue.ToString();
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);
        }
        if (Input.GetKey(KeyCode.DownArrow) && smallResourceValue > 0 && currentSizeValue > 0.6f)
        {
            currentSizeValue -= Time.deltaTime * scaleSpeed;
            smallResourceValue -= Time.deltaTime * scaleSpeed;
            smallResource.text = smallResourceValue.ToString();
            currentSize.text = currentSizeValue.ToString();
            transform.localScale = new Vector3(currentSizeValue, currentSizeValue, currentSizeValue);
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
