/*
 * This scipt was created for the Platformer Controls Taskforce Project.
 * It shows a basci example for a Unity movement script.
 * 
 *  * Project home: https://github.com/Voidsay/Platformer-Controls-Taskforce
 * 
 * Copyright: 
 * GNU GENERAL PUBLIC LICENSE
 * 
 * Contributors:
 * - Voidsay
 * 
 * Last update: 
 * 05/06/2020
 * 
 * Features:
 * - rigitbody based controls
 * - WASD controls and arrow key controls
 * - one jump only, refresh after landing
 * - reset porition with return key
 * 
 * Flaws:
 * - controls appear verry floaty, unusable for platforming
 */

using UnityEngine;

public class RigitControls : MonoBehaviour
{
    // rigitbody and transform component of character
    public Rigidbody rb;
    public Transform tf;

    // adjustable parameters
    public float jump = 500;
    public float move = 100;

    // private variables
    private bool jumpenabled = true;

    // Start is called before the first frame update
    void Start()
    {
        // nothing to do
    }

    // Update is called once per frame
    // I was told fixed updates are better for rigit bodys
    void FixedUpdate()
    {
        // input handle
        if (Input.GetKey("w") || Input.GetKey(KeyCode.UpArrow))
        {
            Debug.Log("up");
            if (jumpenabled)
            {
                jumpenabled = false;
                rb.AddForce(0, jump * Time.deltaTime, 0, ForceMode.Impulse);
            }
        }
        if (Input.GetKey("a") || Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("left");
            rb.AddForce(-move * Time.deltaTime, 0, 0, ForceMode.Impulse);
        }
        if (Input.GetKey("s") || Input.GetKey(KeyCode.DownArrow))
        {
            Debug.Log("down");
            rb.AddForce(0, -move * Time.deltaTime, 0, ForceMode.Impulse);
        }
        if (Input.GetKey("d") || Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("right");
            rb.AddForce(move * Time.deltaTime, 0, 0, ForceMode.Impulse);
        }


        // character reset
        if (Input.GetKey(KeyCode.Return))
        {
            Debug.Log("reset");
            tf.position = new Vector3(0, 1.5f, 0);
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    // collision detection
    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "ground")
        {
            Debug.Log("ground");
            jumpenabled = true;
        }
    }
}
