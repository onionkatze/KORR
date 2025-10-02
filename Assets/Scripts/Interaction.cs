using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for player interacting with objects by aiming at them
public class Interaction : MonoBehaviour
{
    private Camera cam;
    [SerializeField] private GameObject middleUI;
    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    //Each frame a ray is casted towards the middle of the screen. If the total length of the ray is less than 2.5f and an object, hit by this ray is IInteractable, than a crosshair as well
    //as a tooltip become active. Also, if the previous condition is met, you can interact with an object by the press of the "E" button, causing said object's "Interact" function to be called
    void Update()
    {
        Vector3 point = new Vector3(cam.pixelWidth / 2, cam.pixelHeight / 2, 0);
        Ray ray = cam.ScreenPointToRay(point);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            IInteractable hitobject = hit.transform.GetComponent<IInteractable>();
            if (hit.distance <= 2.5f && hitobject!=null)
            {
                middleUI.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                    hitobject.Interact();
            }
            else
            {
                middleUI.SetActive(false);
            }
        }
    }
}
