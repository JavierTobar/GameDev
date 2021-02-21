using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLShapes : MonoBehaviour
{
    public GameObject standardLShape;
    private int numberOfShapes;
    private int counter; // to know how many L shapes we've created thus far
    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        numberOfShapes = Random.Range(3,5); // to get either 3 or 4 L shapes
        VisibilityGraphGenerator.setNumberOfLShapes(numberOfShapes);
        SpawnLShapes();
    }

    // pseudo random helper method that returns true or false to determine design decisions for the creation of L shapes
    private bool getRandomBool(){
        return Random.Range(0, 2) == 1;
    }

    // Spawns L shape at random locations in terrain
    // The randomized shape is defined as the following : the wideness of the L shape and the orientation of the L shape
    private void SpawnLShapes(){
        // each loop will spawn an L shape
        // the design decisions allow us to create randomized L shapes while still being an L shape
        while (counter < numberOfShapes){
            // the size of the shape is proportional to its wideness, this is design decision
            // i.e. thicc shapes will be bigger, thin shapes will be smaller
            // rotation.x = 0 if non inverted, 180 if inverted
            // rotation.y = 0 if straight, 90 if on side
            GameObject LShape = Instantiate(standardLShape, 
            new Vector3(
            Random.Range(30, 64), // X coordinates
            0f, // Y coordinates
            18 + 26 * counter++ // Z coordinates shifted by a constant 
            ), 
            Quaternion.Euler(
            getRandomBool() ? 180 : 0, // pseudo randomly pick X rotation
            getRandomBool() ? 0 : 90, // pseudo randomly pick Y rotation
            0 // Z rotation
            ));
            LShape.transform.localScale = new Vector3(Random.Range(2, 4), 1, Random.Range(2, 4)); // pseudo randomly scale of instantiate
            LShape.tag = "LShape"; // used for visibility graph
            foreach (Transform child in LShape.transform)
            {
                child.tag = "LShape";
            }
            Physics.SyncTransforms();
            // VisibilityGraphGenerator.addLShape(LShape);
        }
        // update after we've instatiantes our prefabs
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
