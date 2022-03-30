using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        /*if (!gameObject.GetComponent<PlayerHealth>())
        {
            return;
        }
        else
        {
            gameObject.GetComponent<PlayerHealth>().Crash();
        }*/

        // This will return "other" collider we are passing in which we are getting by the GetComponent that has "PlayerHealth" script and setting that
        // to a variable to be used

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // The if statement checks to see if it crashes into something else w/o a "PlayerHealth" script which will come back as "null"
        // i.e. another Astroid we will just ignore it
        if (playerHealth == null)
        {
            return;
        }

        // if we do hit something with the Component "PlayerHealth()" we will use the Crash() method that is in PlayerHealth.cs which disables the player
        playerHealth.Crash();

    }

    /*
     * This is a built in method that once the gameObject is no longer able to be seen such as goes off the screen. We are going to destroy that gameObj so that we
     * dont have an ever ending amount of asteroids. That would become super memory intensive and slow down the game.  This is not a perfect fix but will do the 
     * job until we have a better way of doing this.
     */
    private void OnBecameInvisible()
    {
        // The gameObject in this script is the Asteroid because the script is attached to that gameObj.
        Destroy(gameObject);
    }


}
