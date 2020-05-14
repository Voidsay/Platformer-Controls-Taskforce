/*
 * This scipt was created for the Platformer Controls Taskforce Project.
 * It's just for debugging! Displays current veloci vector and magnetude in text. 
 * 
 * Project home: https://github.com/Voidsay/Platformer-Controls-Taskforce
 * 
 * Copyright: 
 * GNU GENERAL PUBLIC LICENSE
 * 
 * Contributors:
 * - Voidsay
 * 
 * Last update: 
 * 05/14/2020
 * 
 */

using UnityEngine;
using UnityEngine.UI;

public class debug : MonoBehaviour
{
    public Text debugText;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        debugText.text = rb.velocity.ToString() + " " + rb.velocity.magnitude.ToString() + " m/s";
    }
}
