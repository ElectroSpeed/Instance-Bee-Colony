using UnityEngine;
using UnityEngine;

public class HiveLocatorScript : MonoBehaviour
{
    private Camera playerCamera;
    public bool _yLock = true;

    private void Start()
    {
        playerCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 direction = transform.position - playerCamera.transform.position;
        if (_yLock)
        {
            direction.y = 0f; // optionnel : garde l'objet droit
        }

        transform.rotation = Quaternion.LookRotation(direction);
    }
}
