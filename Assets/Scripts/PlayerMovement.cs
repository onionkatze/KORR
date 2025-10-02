using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Responsible for playable charachter movement, such as walking, sprinting and jumping
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float movementAcceleration = 1;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float jumpStrength = 0.15f;
    private CharacterController _char;

    private float speedMultiplier=1.0f;
    public bool isRunning = false;

    private Vector3 lastMovement;
    private float VerticalSpeed = 0;
    private bool jumpBuffered = false;
    private bool isTechnicallyGrounded = true;
    private bool wasJustGrounded = true;
    private Coroutine coyote;

    //[SerializeField] private AudioSource soundSource;
    //[SerializeField] private AudioClip walkingSound;
    //private bool soundPlaying = false;

    public bool IsPaused = false;
    void Start()
    {
        _char = GetComponent<CharacterController>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedMultiplier = 2.2f;
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        
        if (_char.isGrounded)
        {
            isTechnicallyGrounded = true;
            wasJustGrounded = true;
        }
        else if (wasJustGrounded)
        {
            if(coyote != null) StopCoroutine(coyote);
            coyote = StartCoroutine(CoyoteJump());
        }
        if (Input.GetButtonDown("Jump") && isTechnicallyGrounded) jumpBuffered = true;
    }
    void FixedUpdate()
    {
        if (!IsPaused)
        {
            CalculateVerticalSpeed();
            MovePlayer();
            speedMultiplier = 1.0f;

//All of the commented-out code is reponsible for playing footstep sounds. This code is a leftover from my previous project, so it might not work as intended
//However I still decided to leave in case it might be useful in the future

            /*float nowSpeed = new Vector3(_char.velocity.x, 0, _char.velocity.z).magnitude;
            if (nowSpeed >= 0.6f)
            {
                if (!soundPlaying)
                {
                    StartCoroutine(PlayWalkSound(nowSpeed));
                }
            }*/
        }
    }
    /*private IEnumerator PlayWalkSound(float spe)
    {
        //soundSource.volume = Random.Range(0.8f*spe, 1f*spe);
        //soundSource.pitch = Random.Range(0.8f, 1.1f);
        //soundSource.PlayOneShot(walkingSound);
        soundPlaying = true;
        yield return new WaitForSeconds(0.6f/2.2f);
        if (!isRunning)
        {
            yield return new WaitForSeconds(0.55f - 0.6f / 2.2f);
            soundPlaying = false;
        }
        else
        {
            soundPlaying = false;
        }
    }*/

    //Makes it so that if the player has just fell from a platform, they can still jump for a short timeframe after, despite not actually being grounded
    private IEnumerator CoyoteJump()
    {
        wasJustGrounded = false;
        yield return new WaitForSeconds(0.2f);
        isTechnicallyGrounded = false;
    }

    //Calculates player's vertical velocity
    private void CalculateVerticalSpeed()
    {
        VerticalSpeed -= gravity * 0.001f;
        VerticalSpeed = Mathf.Max(VerticalSpeed, -0.45f);
        if (jumpBuffered)
        {
            jumpBuffered = false;
            VerticalSpeed = jumpStrength;
        }
        else if (_char.isGrounded)
        {
            VerticalSpeed = -gravity * 0.001f;
        }
    }

    //Calculates player's horizontal velocity and moves playable charachter accordingly
    private void MovePlayer()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal") * speed * 0.1f * speedMultiplier, 0, Input.GetAxisRaw("Vertical") * speed * 0.1f * speedMultiplier);
        movement = Vector3.ClampMagnitude(movement, speed * 0.1f * speedMultiplier);
        lastMovement = Vector3.Lerp(lastMovement, movement, 0.1f * movementAcceleration);
        movement = transform.TransformDirection(lastMovement + new Vector3(0, VerticalSpeed, 0));
        _char.Move(movement);
    }
}
