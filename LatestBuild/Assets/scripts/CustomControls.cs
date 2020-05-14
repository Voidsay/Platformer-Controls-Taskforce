/*
 * This scipt was created for the Platformer Controls Taskforce Project.
 * It shows an advanced and customisable movement script.
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
 * 05/14/2020
 * 
 * Features:
 * - determine ground by groundlayer
 * - customisable extra gravity, max movement speedm, acceleration, deceleration, jumpforece, crouchforce, coyotetime
 * - walls in groundlayer arn't seen as ground (angleLimit is a cone below character, that accepts ground collisions)
 * - uses custom input manager
 *  
 * Flaws:
 * - jump can get stuck (problems with on collision event)
 * - jump sometimes delayed (basicaly the same reason as above, this one just fixes itself)
 * - two funktions of questionable quality (see TODO)
 */

using UnityEngine;

public class CustomControls : MonoBehaviour
{
    // rigitbody and transform component of character
    public Rigidbody rb;
    public Transform tf;

    public InputManager inputManager;
    public LineRenderer lr;// only for debugging remove in real script

    // adjustable parameters
    public int groundLayer = 8;// put all ground coliders in a layer
    public float extraGrav = 1000f;// normal gravity appers too slow
    //public float terminalVelocity = 100f; // no need! Adjusted by cahning drag parameter in rigidbody
    public float maxMoveSpeed = 10f;// max sidway movement speed
    public float dampening = 0.7f;// dampening constant
    public float threshhold = 0.5f;// detects no input and applies dampening, would only be useful for controlers, haven't testet it yet, controlers arn't supported with custom input manager!
    public float jumpForce = 1000f;// force for jumping
    public float jumpCooldownTime = 0.2f;// minimal time between jumps
    public float move = 100f;// movement force
    public float crouchForce = 50f;// additional force when crouching
    public float angelLimit = 30f;// only here to dissable walljump bug/feature ;)
    public float coyoteTime = 0.1f;// jump only disabled after some time off the ground

    // private variables
    private Vector3 direction = Vector3.zero;
    private bool jumpEnabeled = true;
    private bool coyoteOn = false;
    private bool jumpCooldown = true;
    private bool jumpPressed, crouch;


    //debug line direction, remove for real script
    private Vector3 force = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UserInput();// if you don't use the custom input manager replace the input detection in this function!

        if (Input.GetKey(KeyCode.Escape))// just for testing move to game manager
        {
            Application.Quit();
        }
    }

    // fixed update for physics ticks
    void FixedUpdate()
    {
        Movement();
        debugLine();
    }

    private void UserInput()
    {
        //direction.x = Input.GetAxisRaw("Horizontal");// use for joysticks, not customisable by input manager!
        //TODO
        if (inputManager.GetButton("P1Right") ^ inputManager.GetButton("P1Left"))// sort of messy
        {
            if (inputManager.GetButton("P1Right"))
            {
                direction.x = 1;
            }
            else
            {
                direction.x = -1;
            }
        }
        else
        {
            direction.x = 0;
        }

        jumpPressed = inputManager.GetButtonDown("P1Jump");
        crouch = inputManager.GetButton("P1Crouch");
    }

    private void Movement()
    {
        force = Vector3.zero;

        // slow down
        if (direction.magnitude <= threshhold)
        {
            CounterMovement();
        }

        // extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * extraGrav, ForceMode.Acceleration);

        // jump
        if ((jumpEnabeled || coyoteOn) && jumpPressed && jumpCooldown)
        {
            Invoke("JumpCooldown", jumpCooldownTime);
            jumpEnabeled = false;
            jumpCooldown = false;
            Jump();
        }
        if (jumpPressed)
        {
            Debug.Log("Jumprequest:" + jumpEnabeled + " " + jumpCooldown);
        }

        // limit speed and add it 
        //TODO// is there a better way?
        if (rb.velocity.x + direction.x * move * Time.deltaTime > maxMoveSpeed)
        {
            rb.velocity = new Vector3(maxMoveSpeed, rb.velocity.y, rb.velocity.z);
        }
        else if (rb.velocity.x + direction.x * move * Time.deltaTime < -maxMoveSpeed)
        {
            rb.velocity = new Vector3(-maxMoveSpeed, rb.velocity.y, rb.velocity.z);
        }
        else
        {
            rb.AddForce(direction.x * Time.deltaTime * move, 0, 0, ForceMode.VelocityChange);
        }

        force += new Vector3(direction.x * Time.deltaTime * move, 0, 0);

        if (crouch)
        {
            Crouch();
        }

    }

    private void CounterMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x * dampening, rb.velocity.y, 0);// for some reason vector set methode dosn't work whats up with that?
        force += new Vector3(rb.velocity.x * dampening, rb.velocity.y, 0);
    }

    private void Jump()
    {
        Debug.Log("jump");
        rb.AddForce(Vector3.up * Time.deltaTime * jumpForce, ForceMode.VelocityChange);
        force += Vector3.up * Time.deltaTime * jumpForce;
    }

    private void Crouch()
    {
        rb.AddForce(Vector3.down * crouchForce * Time.deltaTime, ForceMode.VelocityChange);
        force += Vector3.down * crouchForce * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collisionInfo)//FIXME jump disable bug when landing in corner // OnCollisionStay would work better but causes doublejump bug
    {
        if (collisionInfo.gameObject.layer == groundLayer)
        {
            for (int i = 0; i < collisionInfo.contacts.Length; i++)
            {
                if (Vector3.Angle(Vector3.up, rb.position - collisionInfo.contacts[i].point) <= angelLimit)
                {
                    Debug.Log("unlock");
                    jumpEnabeled = true;
                    coyoteOn = true;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.layer == groundLayer)
        {
            Invoke("coyoteDisable", coyoteTime);
        }
    }

    private void debugLine()
    {
        lr.SetPosition(0, tf.position + Vector3.back);
        lr.SetPosition(1, tf.position + force + Vector3.back);
    }

    private void JumpCooldown()
    {
        Debug.Log("cooldown");
        jumpCooldown = true;
    }

    private void coyoteDisable()
    {
        Debug.Log("coyote");
        coyoteOn = false;
    }
}

