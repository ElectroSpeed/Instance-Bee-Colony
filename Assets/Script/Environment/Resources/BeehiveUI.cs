using UnityEngine;
using TMPro;

public class BeehiveUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _pollenText;
    [SerializeField] private TextMeshProUGUI _honeyText;

    [Header("References")]
    [SerializeField] private Beehive _beehive;

    private void Start()
    {
        if (_beehive == null) return;
        
        _pollenText.text = _beehive.GetPollenStock().ToString();
        _honeyText.text = _beehive.GetHoneyStock().ToString();
        
        _beehive.OnPollenChanged += value => _pollenText.text = value.ToString();
        _beehive.OnHoneyChanged += value => _honeyText.text = value.ToString();
    }
}