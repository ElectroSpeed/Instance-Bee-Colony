using System.Collections.Generic;
using UnityEngine;

public class Book : MonoBehaviour
{
    [SerializeField] float pageSpeed = 0.5f;
    [SerializeField] List<Transform> pages = new List<Transform>();
    [SerializeField] private SO_FlowerData[] flowerDatas;
    [SerializeField] private GameObject pageBook;
    [SerializeField] private GameObject BookCover;

    int index;


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
            slot.Name.text = flower.FlowerName;
        }
        index = -1;
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
        StartTurn(pages[index], 180f, false);
    }

     public void RotateBack()
    {
        if (isTurning) return;
        if (index < 0) return;

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
        Debug.Log(index);
        if (!isTurning) return;

        animT += Time.deltaTime * pageSpeed;

        currentPage.rotation = Quaternion.Slerp(startRot, targetRot, animT);

        if (animT >= 1f)
        {
            currentPage.rotation = targetRot;

            if (turnBackward)
                index--;

            isTurning = false;
        }
    }
}
