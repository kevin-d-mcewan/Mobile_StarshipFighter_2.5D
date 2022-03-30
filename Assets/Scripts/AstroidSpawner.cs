using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstroidSpawner : MonoBehaviour
{

    [SerializeField] private GameObject[] asteroidPrefabs;
    [SerializeField] private float secondsBetweenAsteroids = 1.5f;
    /* "forceRange" is a range in which the amount of force each asteroid has. Instead of just having a single var force amount. You can also do this with 2
     * vars a "forceMin" and "forceMax" but this is easier with it just being one variable. The Vector2 way the "x-value" will be the min and the "y-value" will be max      
     */
    [SerializeField] private Vector2 forceRange;

    // This is the timer for how long to wait between astroid spawns
    private float timer;
    private Camera mainCamera;


    private void Start()
    {
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        // For the timer: Every single frame we want to reduce the timer by how many seconds have passed since the last frame. We do this by taking 
        // timer -= Time.deltaTime. And once it reaches 0 or below 0 we want to spawn in a new Asteroid. We do this with an if statement and another fx
        // So that Update doesn't have to much stuff in it. Better to refactor the code and break it into smaller parts

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            // This spawns in Asteroid from this f(x) and then we take "timer" and add it to "secondsBetweenAsteroid" variable
            SpawnAsteroid();

            timer += secondsBetweenAsteroids;
        }
    }


    /* 
     * In this function we are going to pick a random side of the screen and depending on which side of the screen gets randomly choosen we will do 
     * the coorisponding "case #" from the "switch" statement.
     * 
     * Then we want to take the values and convert it into the world space for the device being used by the size and shape of the screen. Then pick a random asteroid out
     * of our array and spawn that asteroid in at the position we calculated with a random rotation to make each one look differently.
     * 
     * Then grab the rigidbody component of that asteroid and set the velocity by the Random.Range of numbers from the "forceRange.x & y" using the min value from
     * x and the maximum value from y as its range.
     */
    private void SpawnAsteroid()
    {
        // Pick a random number between 1 & 3 in this case. With INTS minNum is Inclusive and maxNum is Exclusive so we dont chose 3 for Max it is 4
        int side = Random.Range(0, 4);

        Vector2 spawnPoint = Vector2.zero;
        Vector2 direction = Vector2.zero;

        switch (side)
        {
            case 0:
                // LeftSide Screen
                spawnPoint.x = 0;                   // Means its on the left side of the screen
                spawnPoint.y = Random.value;        //Random.value returns a vaule between (0 and 1)
                direction = new Vector2(1.0f, Random.Range(-1.0f, 1.0f));
                break;
    
            case 1:
                // RightSide Screen
                spawnPoint.x = 1;
                spawnPoint.y = Random.value;
                direction = new Vector2(-1.0f, Random.Range(-1.0f, 1.0f));
                break;

            case 2:
                // BottomSide Screen
                spawnPoint.x = Random.value;
                spawnPoint.y = 0;
                direction = new Vector2(Random.Range(-1.0f, 1.0f), 1.0f);
                break;
            
            case 3:
                // TopSIde Screen
                spawnPoint.x = Random.value;
                spawnPoint.y = 1;
                direction = new Vector2(Random.Range(-1.0f, 1.0f), -1.0f);
                break;

        }

        // This is where we will spawn in the Asteroid
        Vector3 worldToSpawnPoint = mainCamera.ViewportToWorldPoint(spawnPoint);
        worldToSpawnPoint.z = 0;        // We do this because we dont care about the z-axis in our game and "ViewportToWorldPoint" requires a z-axis coord. so we make it 0

        // A variable that Picks a random asteroid to spawn
        GameObject selectedAsteroid = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];


        // Spawning in the random asteroid var; This "Instantiate" has a (GameObj, Position and Rotation)
        // Then we want to CACHE (make a variable) the Instantiate so we can make multiple by setting it equal to "GameObject asteroidInstance"
        GameObject asteroidInstance = Instantiate(
            selectedAsteroid, 
            worldToSpawnPoint, 
            Quaternion.Euler(0f, 0f, Random.Range(0, 360.0f)));

        // This gets the RigidBody Component of the Instantiate Var so we can manipulate the velocity and magnitude
        Rigidbody rb = asteroidInstance.GetComponent<Rigidbody>();

        // We already have the direction figured out above but we want to normalize the magnitude. Normalizing something sets it to 1. We do this because
        // Vectors have both "DIRECTIONS" and "MAGNITUDES". We then mult by the "forceRange" set up above. We want it to chose a random "forceRange.x" (minimum) and
        // "forceRange.y" (maximum) between those two numbers
        rb.velocity = direction.normalized * Random.Range(forceRange.x, forceRange.y);
    }
}
