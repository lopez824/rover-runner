using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlanetGravity : MonoBehaviour
{
    public float gravityAccel = 9.81f;

    [HideInInspector]
    public bool onPlanet;

    private Animator cameraAnim;
    private GameObject player;
    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cameraAnim = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Animator>();
        onPlanet = false;
    }

    // Checks if player is within a planet's orbit.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onPlanet = true;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<PlayerController>().currentPlanet = this;

            cameraAnim.Play("vCamZoomIn");
        }
    }

    // Checks if player is outside a planet's orbit.
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onPlanet = false;
            player.GetComponent<Rigidbody>().useGravity = false;

            cameraAnim.Play("vCamZoomOut");
            //Invoke("GameOver", 3f);
        }
    }

    // Reloads level.
    public void GameOver()
    {
        SceneManager.LoadScene(0);
    }

    private void FixedUpdate()
    {
        if (onPlanet == true)
        {
            // Calculate the direction from the player to the planet; used to update gravity.
            Vector3 directionFromPlayer = transform.position - player.transform.position;
            Physics.gravity = directionFromPlayer.normalized * gravityAccel;

            // Calculate the direction from the planet to the player.
            Vector2 directionToPlayer = player.transform.position - transform.position;

            // Calculate the angle between the planet and the player.
            float angleOfPlayer = Mathf.Atan2(directionToPlayer.normalized.y, directionToPlayer.normalized.x);

            /* Calculate the tangent vector of the player's position relative to the planet's surface.
             Formula is the derivative of the planet's cubic curve, translated to a 2d curve for design constraints, which all sphere/circular objects naturally have.
             f(u) = (rcos(u), rsin(u)) where u is the angleOfPlayer and r is the radius (irrelevent for the use of this vector) 
            */
            Vector2 tangentVector = new Vector2( -1f * Mathf.Sin(angleOfPlayer), Mathf.Cos(angleOfPlayer));

            Debug.DrawRay(directionToPlayer, tangentVector * -20f, Color.red);
            player.GetComponent<PlayerController>().tangentVector = tangentVector;
        }
    }
}
