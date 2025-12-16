using UnityEngine;

public class BookManager : MonoBehaviour
{

    [SerializeField] private GameObject book;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        book.SetActive(false);
        
    }

    // Update is called once per frame
    public void OpenBook()
    {
        book.SetActive(true);
    }

    public void CloseBook()
    {
        book.SetActive(false);
    }
}
