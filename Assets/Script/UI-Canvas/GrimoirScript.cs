using UnityEngine;

public class GrimoirScript : MonoBehaviour
{
    //tableaux de fleurs
    [SerializeField] private SO_FlowerData[] flowerDatas;
    [SerializeField] private GameObject GrimoirSlotPrefab;
    [SerializeField] private GameObject GrimoirContent;

    [Header("Ressources Loader")]
    [SerializeField] private Sprite FlowerImage;
    [SerializeField] private Sprite TemperatureImage;
    [SerializeField] private Sprite HumidityImage;
    [SerializeField] private Sprite SunlightImage;

    void Start()
    {
        foreach (SO_FlowerData flowerData in flowerDatas)
        {
            // Instantiate slot
            GameObject slotGO = Instantiate(GrimoirSlotPrefab, GrimoirContent.transform);

            // Get script attached to prefab
            GrimoirSlotScript slot = slotGO.GetComponent<GrimoirSlotScript>();
            if (slot == null)
            {
                Debug.LogError("Le prefab n'a PAS de script 'GrimoirSlotScript' !");
                return;
            }

            // Images
            slot.Image.sprite = FlowerImage;
            slot.TemperatureIcon.sprite = TemperatureImage;
            slot.HumidityIcon.sprite = HumidityImage;
            slot.SunlightIcon.sprite = SunlightImage;

            // Textes
            slot.Description.text = flowerData._description;
            slot.Name.text = flowerData._flowerName;

            // Conditions
            slot.HumidityText.text = flowerData._minHumidity + "% / " + flowerData._maxHumidity + "%";
            slot.SunlightText.text = flowerData._minSunlight + "% / " + flowerData._maxSunlight + "%";
            slot.TemperatureText.text = flowerData._minTemperature + "°C / " + flowerData._maxTemperature + "°C";
        }
    }
}
