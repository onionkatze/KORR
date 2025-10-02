using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interactable door with functionality to be opened and closed (For now can only be opened one way)
public class Door : MonoBehaviour, IInteractable
{
    private bool isOpening;
    private float angle=0;
    private void Update()
    {
        if (isOpening)
        {
            if (angle > -90) angle -= 1;
            else angle = -90;
        }
        else
        {
            if (angle < 0) angle += 1;
            else angle = 0;
        }
        transform.localEulerAngles = new Vector3(0, angle, 0);
    }
    public void Interact()
    {
        isOpening = !isOpening;
    }
}
