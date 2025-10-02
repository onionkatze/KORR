using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is responsible for HeadBobbing, whenever our playable chrachter is moving (HeadBobbing is camera shaking imitating IRL head shaking during movement)
public class HeadBobController : MonoBehaviour
{
    //Amplitude of Head Bobbing
    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    //How fast our camera is shaking
    [SerializeField, Range(0, 30)] private float frequency = 5.0f;
    //sprintBobbingMultiplier multiplies frequency and amplitude whenever playable charachter is sprinting 
    [SerializeField, Range(1, 3)] private float sprintBobbingMultiplier = 1.5f;

    [SerializeField] private Transform cam = null;
    [SerializeField] private Transform cameraHolder = null;

    private float toggleSpeed = 1.5f;
    private Vector3 startPos;
    private CharacterController controller;
    private PlayerMovement player;
    private float bobbingTime = 0;
    private bool wasRunning = false;

    Vector3 pos = Vector3.zero;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        controller = GetComponent<CharacterController>();
        startPos = cam.localPosition;
    }
    void FixedUpdate()
    {
        if (!enabled) return;

        CheckMotion();
        cam.LookAt(FocusTarget());
    }
    
    //Checks, wheather HeadBobbing should occur. If not, method ResetPosition is called
    private void CheckMotion()
    {
        float speed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
        if (speed < toggleSpeed || !controller.isGrounded || Mathf.Abs(Input.GetAxisRaw("Vertical"))+ Mathf.Abs(Input.GetAxisRaw("Horizontal"))==0)
        {
            bobbingTime = 0;
            ResetPosition(0.05f);
        }
        else PlayMotion(FootStepMotion());
    }

    //Responsible for the trajectory the camera follows during HeadBobbing
    private Vector3 FootStepMotion()
    {
        float speedMultiplier = 1;
        if (player.isRunning)
        {
            speedMultiplier = sprintBobbingMultiplier;
            if (!wasRunning)
            {
                wasRunning=true;
                bobbingTime /= sprintBobbingMultiplier;
            }
        }
        else
        {
            speedMultiplier = 1;
            if (wasRunning)
            {
                wasRunning = false;
                bobbingTime *= sprintBobbingMultiplier;
            }
        }

        pos.y = amplitude * 10 - Mathf.Abs(Mathf.Cos(bobbingTime * frequency * speedMultiplier)) * amplitude * 10 * speedMultiplier * (1f + Input.GetAxisRaw("Vertical")) / 2f;
        pos.x = Mathf.Sin(bobbingTime * frequency * speedMultiplier) * amplitude * 12 * speedMultiplier * (0.2f + Input.GetAxisRaw("Vertical")) / 1.2f;
        bobbingTime += Time.deltaTime;
        return pos;
    }

    //Smoothly returns camera to it's origin point
    private void ResetPosition(float intensity)
    {
        if (cam.localPosition == startPos) return;
        cam.localPosition = Vector3.Lerp(cam.localPosition, startPos, intensity);
        pos = Vector3.zero;
    }

    //Rotates the camera slightly to face approximately the same direction it would face, if HeadBobbing was disabled
    private Vector3 FocusTarget()
    {
        Vector3 pos = new Vector3(transform.position.x, cameraHolder.position.y, transform.position.z)+ cameraHolder.forward * 15.0f;
        return pos;
    }

    //Sets a new camera localPosition
    private void PlayMotion(Vector3 motion)
    {
        cam.localPosition = Vector3.Lerp(cam.localPosition,motion,0.75f);
    }
}
