using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlanetGravity currentPlanet;
    public TextMeshProUGUI speedUI;
    public float maxSpeed = 5.0f;
    public float movementSpeed = 1000.0f;
    public float jumpForce = 100.0f;

    [HideInInspector]
    public Vector2 tangentVector = Vector2.zero;

    private Rigidbody rb;
    private AudioSource sound;
    private bool isJumping = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        sound.Play();
    }

    // Reloads level.
    public void ResetGame(InputAction.CallbackContext context)
    {
        if (context.performed)
            SceneManager.LoadScene(0);
    }

    // Applies force in the opposite direction of the planet's gravity.
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && isJumping == false)
        {
            isJumping = true;
            SoundLibrary.sound.PlayOneShot(SoundLibrary.library["Jump"]);
            rb.AddForce(Physics.gravity * -1f * jumpForce);
        }
    }

    // Checks if the player's velocity is too fast.
    private bool IsAtMaxSpeed()
    {
        float currentXSpeed = rb.velocity.x;
        float currentYSpeed = rb.velocity.y;

        if (Mathf.Abs(currentYSpeed) > maxSpeed)
            rb.velocity = new Vector2(currentXSpeed, maxSpeed * Mathf.Sign(currentYSpeed));

        if (Mathf.Abs(currentXSpeed) > maxSpeed)
        {
            rb.velocity = new Vector2(maxSpeed * Mathf.Sign(currentXSpeed), currentYSpeed);
            return true;
        }

        return false;
    }

    private void FixedUpdate()
    {
        // Adds force to the player in the direction of the tanget vector relative to the planet's surface.
        if (IsAtMaxSpeed() == false && currentPlanet.onPlanet == true)
            rb.AddForce(tangentVector * movementSpeed * Time.deltaTime * -1f);
        speedUI.text = Mathf.Round(rb.velocity.magnitude).ToString();
    }

    // Checks if the player is on the ground.
    private void OnCollisionEnter(Collision collision)
    {
        if (isJumping == true)
            if (collision.gameObject.tag == "Platform")
                isJumping = false;
    }
}
