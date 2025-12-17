using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages = new List<Transform>();
    [SerializeField] private SO_FlowerData[] flowerDatas;
    [SerializeField] private GameObject pageBook;
    [SerializeField] private GameObject BookCover;

    [Header("Ressources Loader")]
    [SerializeField] private Sprite TemperatureImage;
    [SerializeField] private Sprite HumidityImage;
    [SerializeField] private Sprite SunlightImage;

    int index = -1;


    bool isTurning = false;
    Transform currentPage;
    Quaternion startRot;
    Quaternion targetRot;
    float animT;

    
    private void Start()
    {
        foreach (SO_FlowerData flower in flowerDatas) 
        {
            GameObject slotGO = Instantiate(pageBook, BookCover.transform);
            pages.Add(slotGO.transform);
            PagesScript slot = slotGO.GetComponent<PagesScript>();
            if (slot == null)
            {
                Debug.LogError("Le prefab n'a PAS de script 'GrimoirSlotScript' !");
                return;

            }
            // Images
            slot.TemperatureIcon.sprite = TemperatureImage;
            slot.HumidityIcon.sprite = HumidityImage;
            slot.SunlightIcon.sprite = SunlightImage;

            // Textes
            slot.Description.text = flower.Description;
            slot.Name.text = flower.FlowerName;

            // Conditions
            slot.HumidityText.text = flower.MinHumidity + "% / " + flower.MaxHumidity + "%";
            slot.SunlightText.text = flower.MinSunlight + "% / " + flower.MaxSunlight + "%";
            slot.TemperatureText.text = flower.MinTemperature + "°C / " + flower.MaxTemperature + "°C";
            slotGO.transform.SetAsFirstSibling();

            //Flower Button
            GameObject FlowerButtonInstance = Instantiate(
                flower.FlowerButtonPrefab,
                slot.FlowerButtonContainer.transform
            );
     

        }

        if (pages.Count == 0)
        {
            foreach (Transform t in transform)
                pages.Add(t);
        }
    }
    public void RotateForward()
    {
        if (isTurning) return;
        if (index + 1 >= pages.Count) return;


        index++;
        pages[index].transform.SetAsLastSibling();
        StartTurn(pages[index], 180f, false);


    }

     public void RotateBack()
    {
        if (isTurning) return;
        if (index < 0) return;

        pages[index].transform.SetAsLastSibling();
        StartTurn(pages[index], 0f, true);
    }

     private void StartTurn(Transform page, float angle, bool backward)
    {
        if (page == null) return;

        isTurning = true;
        currentPage = page;
        startRot = page.rotation;
        targetRot = Quaternion.Euler(0, angle, 0);
        animT = 0f;


        turnBackward = backward;
    }

    bool turnBackward = false;

     private void Update()
    {
        if (!isTurning) return;

        animT += Time.deltaTime * pageSpeed;

        currentPage.rotation = Quaternion.Slerp(startRot, targetRot, animT);

        if (animT >= 0.5f && pages[index].GetComponent<PagesScript>().Container != null)
        {
            pages[index].GetComponent<PagesScript>().Container.SetActive(false);
            if (turnBackward)
                pages[index].GetComponent<PagesScript>().Container.SetActive(true);
        }

        if (animT >= 1f)
        {
            currentPage.rotation = targetRot;
  
            if (turnBackward)
                index--;


 

            isTurning = false;
        }
    }
}
