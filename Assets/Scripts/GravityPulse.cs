using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPulse : MonoBehaviour
{
    public float pulseStrength = 6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            SoundLibrary.sound.PlayOneShot(SoundLibrary.library["Pulse"]);

            // Sends impulse force to the player in the opposite direction of the current planet's gravity.
            rb.AddForce(Physics.gravity * -1f * pulseStrength, ForceMode.Impulse);
        }
    }
}
