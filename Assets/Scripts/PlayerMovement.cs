using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;          // Is the amount we will multiply and/or add to the force to make it move
    [SerializeField] private float maxVelo;                 // Top velo/speed that will be used in this equation


    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    
    void Update()
    {

        ProcessInput();
        KeepPlayerOnScreen();

    }




    /* We need to use FixedUpdate bc we are using Physics which is updated every physics update (which is not every frame like Update but every couple of frames);
       This is done bc physics calculations are an expensive process and takes up a lot of the "computers" resources especially if it was done every frame like in Update
       This way it lessens the amount of times this is done and makes the game move more smoothly*/
    private void FixedUpdate()
    {
        // if the movementDirection is Zero, this will happen bc there is no force being applied in the above else statement in Update, then just return out of statement
        if (movementDirection == Vector3.zero)
        {
            return;
        }

        // If it isn't Zero then we want to AddForce to the movement

        /* We dont add "Time.deltaTime" after 'forceMagnitude' bc since it is in FixedUpdate(). FixedUpdate() is called a certain amount of time and a time interval which is
         * what Time.deltaTime is trying to do, In this instance we may need to cut "forceMagnitude" by half of original amount set up in this lecture or "divide by 50"
         * The original amount in the inspector was 850 and now we have changed it to 425 but can still be adjusted in the Inspector to feel optimal.*/

        /* Also we want to make sure the velocity doesn't go over our defined maximum which can happed if Vector3.ClampMagnitude is not used because it will continue
         * to add Magnitude to it overtime as you continue to press down on screen*/
        rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelo);
    }



    // Process Touch Input from the player on the screen to move player around the screen
    private void ProcessInput()
    {
        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Debug.Log(touchPosition);

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            //Debug.Log(worldPosition);

            // This is player position (transform.position since this script is attached to player) - where we touched (worldPosition)
            /* This moves around like INVERTED CONTROLS so may need to tweak this to "movementDirection = worldPosition - transform.position" or something similiar*/

            // movementDirection = transform.position - worldPosition;
            movementDirection = worldPosition - transform.position;
            movementDirection.z = 0f;
            /* We will use Normalize() here bc that means we dont care about how far away we touched or hard we touched, just the direction and "Normalize()" does that
             by returning a 'Maginatude' of 1*/
            movementDirection.Normalize();
        }
        else
        {
            /* This says if we aren't touching then dont add any force to it to get it to slowdown and stop; And if we release our finger how fast it slows down
             * depends on the DRAG, that is attached onto the RIGIDBODY. So this can be adjusted via code or inspector*/
            movementDirection = Vector3.zero;
        }
    }


    // This function is to keep player ship on screen so, if it goes past horizontal or vertical screen limit it gets teloported to other side of screen.
    // i.e. y = 10 becomes y = -10, x = 15 becomes x = -15...
    private void KeepPlayerOnScreen()
    {
        /* var for the position we are at if it is not of screen. so at this point "newPosition" becomes current position on screens bc that is what 'transform.position' is*/
        Vector3 newPosition = transform.position;

        /* Convert from "WorldSpace" to "Viewport" bc "Viewport" on every device is the same ((0, 1) is top left, (1, 1) is top right, (0, 0) is bottom left and
           (1,0) is bottom right, with (0.5, 0.5) being in the middle) this makes it easy because every screen has these dimensions not effected by screen pixel size */

        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);     // storing converted WorldPoint to ViewPort point in a variable to use

        if (viewportPosition.x > 1)
        {
            newPosition.x = -newPosition.x + 0.1f;
        }
        else if (viewportPosition.x < 0)
        {
            newPosition.x = -newPosition.x - 0.1f;
        }

        if (viewportPosition.y > 1)
        {
            newPosition.y = -newPosition.y + 0.1f;
        }
        else if (viewportPosition.y < 0)
        {
            newPosition.y = -newPosition.y - 0.1f;
        }

        transform.position = newPosition;
        
        
    }



}
