using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forceMagnitude;
    [SerializeField] private float maxVelo;


    private Rigidbody rb;
    private Camera mainCamera;

    private Vector3 movementDirection;
    
    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    
    void Update()
    {

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

            // Debug.Log(touchPosition);

            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

            //Debug.Log(worldPosition);

            // This is player position (transform.position since this script is attached to player) - where we touched (worldPosition)
            /* This moves around like INVERTED CONTROLS so may need to tweak this to "movementDirection = worldPosition - transform.position" or something similiar*/
            movementDirection = transform.position - worldPosition;
            movementDirection.z = 0f;
            /* We will use Normalize() here bc that means we dont care about how far away we touched or hard just the direction and "Normalize()" does that
             by returning a 'Maginatude' of 1*/
            movementDirection.Normalize();
        }
        else
        {
            /* This says if we aren't touching then dont add anything to it; And if we release our finger how fast it slows down depends on the DRAG on the
             * RIGIDBODY that is attached. So this can be adjusted via code or inspector*/
            movementDirection = Vector3.zero;
        }

    }

    // We need to use FixedUpdate bc we are using Physics which is updated every physics update (which is not every frame like Update)
    private void FixedUpdate()
    {
        // if the movementDirection is Zero just return out of statement
        if (movementDirection == Vector3.zero)
        {
            return;
        }

        // If it isn't Zero then we want to AddForce to the movement
        /* We dont add "Time.deltaTime" after 'forceMagnitude' bc since it is in FIxedUpdate() its already called a specific amount of time,
         * do to this we may need to cut the forceMagnitude by half(divide by 50)*/
        /* Also we want to make sure the velocity doesn't go over our defined maximum which can happed if Vector3.ClampMagnitude is not used because it will continue
         * to add Magnitude to it overtime as you continue to press down on screen*/
        rb.AddForce(movementDirection * forceMagnitude, ForceMode.Force);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelo);
    }
}
