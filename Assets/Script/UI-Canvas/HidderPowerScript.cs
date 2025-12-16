using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class HidderPowerScript : MonoBehaviour
{

    [SerializeField] private GameObject powerContainer;
    private RectMask2D maskObject;

    private bool isMasked;

    private void Start()
    {
        maskObject = powerContainer.GetComponent<RectMask2D>();
        maskObject.enabled = true;
        isMasked = false;
        
    }

    // Update is called once per frame
    public void SetMasked()
    {
        if (!isMasked)
        {
            maskObject.enabled = false;
            isMasked = true;
        }
        else
        {
            maskObject.enabled = true;
            isMasked = false;
        }
    }
}
