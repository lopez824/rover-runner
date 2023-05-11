using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerPickupHandler : MonoBehaviour
{
    public TextMeshProUGUI scoreUI;
    private int score = 0;

    // Updates Score UI.
    private void UpdateScore()
    {
        score++;
        scoreUI.text = score.ToString();
    }

    // Checks what object was picked up.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ScorePickup")
        {
            SoundLibrary.sound.PlayOneShot(SoundLibrary.library["Pickup"]);
            UpdateScore();
            Destroy(other.gameObject);
        }
    }
}
