using TMPro;
using UnityEngine;

public class WaxUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _waxText;
    
    [Header("References")]
    [SerializeField] private WaxManager _waxManager;
    
    private void Start()
    {
        if (_waxText == null) return;
        
        _waxText.text = _waxManager.GetWaxStock().ToString();
        
        _waxManager.OnWaxChanged += value => _waxText.text = value.ToString();

    }
}

