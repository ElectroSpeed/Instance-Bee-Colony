using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class PagesScript : MonoBehaviour
{
    [Header("Images")]
    public Image Image;
    public Image TemperatureIcon;
    public Image HumidityIcon;
    public Image SunlightIcon;

    [Header("Texts")]
    public TMP_Text Description;
    public TMP_Text Name;

    [Header("Conditions")]
    public TMP_Text HumidityText;
    public TMP_Text SunlightText;
    public TMP_Text TemperatureText;
}
