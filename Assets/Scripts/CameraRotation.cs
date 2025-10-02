using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for camera movement with mouse
public class CameraRotation : MonoBehaviour
{
    //Sensitivity
    public float sensitivity = 9.0f;
    //The maximal and minimal angle our camera can turn in vertical axis
    public float minimumVert = -80.0f;
    public float maximumVert = 80.0f;
    private float _rotationX = 0;

    private bool IsPaused = false;

    [SerializeField] Transform cam;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!IsPaused)
        {
            transform.Rotate(0, sensitivity * Input.GetAxis("Mouse X"), 0, Space.World);
            _rotationX -= Input.GetAxis("Mouse Y") * sensitivity;
            _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);
            float rotationY = cam.localEulerAngles.y;
            cam.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
        }
    }
}
