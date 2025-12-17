using System;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BeehiveUI : MonoBehaviour
{
    [Header("UI Elements")] [SerializeField]
    private GameObject _tooltipText;

    [SerializeField] private TextMeshProUGUI _pollenText;
    [SerializeField] private TextMeshProUGUI _honeyText;
    [SerializeField] private TextMeshProUGUI _beeNumberText;

    [Header("References")] [SerializeField]
    private Beehive _beehive;
    private Camera _cam;
    
    Ray _ray;
    RaycastHit _hit;

    private void Start()
    {
        _cam = Camera.main;
        if (_beehive == null) return;

        _pollenText.text = _beehive.GetPollenStock().ToString();
        _honeyText.text = _beehive.GetHoneyStock().ToString();

        _beehive.OnPollenChanged += value => _pollenText.text = value.ToString();
        _beehive.OnHoneyChanged += value => _honeyText.text = value.ToString();
        _beehive.OnBeeNumberChanged += value => _beeNumberText.text = value.ToString();
    }

    private void FixedUpdate()
    {
        
        _ray = _cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(_ray, out _hit))
        {
            if (_hit.collider.transform.parent.gameObject == _beehive.gameObject)
            {
                _tooltipText.SetActive(true);
            }
            else
            {
                _tooltipText.SetActive(false);
            }
        }
    }
}