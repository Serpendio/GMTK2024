using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerResources : Resources
{
    public TextMeshProUGUI bigResource, smallResource, currentSize;
    public Viewport viewport;

    protected override void Start()
    {
        base.Start();
        if (bigResource != null)
            bigResource.text = bigResourceValue.ToString();
        if (smallResource != null)
            smallResource.text = smallResourceValue.ToString();
        if (currentSize != null)
            currentSize.text = currentSizeValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            TryGrow();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            TryShrink();
        }
    }

    public override void TryGrow()
    {
        base.TryGrow();
        if (currentSize != null)
        {
            currentSize.text = currentSizeValue.ToString();
        }
        if (viewport != null)
            viewport.UpdateScale(currentSizeValue);
    }

    public override void TryShrink()
    {
        base.TryShrink();
        if (currentSize != null)
        {
            currentSize.text = currentSizeValue.ToString();
        }
        if (viewport != null)
            viewport.UpdateScale(currentSizeValue);
    }

    public override void AddBigResource(float value)
    {
        base.AddBigResource(value);

        if (bigResource != null)
            bigResource.text = bigResourceValue.ToString();
    }

    public override void AddSmallResource(float value)
    {
        base.AddSmallResource(value);

        if (smallResource != null)
            smallResource.text = smallResourceValue.ToString();
    }
}
