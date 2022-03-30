using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    

    public void Crash()
    {
        /* This just turns off the player gameObj and doesn't delete it. We are doing it this way bc we are going to include the ability for the user
        // to watch an add and turn back on the player 
        */
        gameObject.SetActive(false);
    }

}
