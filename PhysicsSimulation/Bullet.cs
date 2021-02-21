using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float gravity = 0f;
    private float xVelocity;
    private float yVelocity;
    private MeshCollider meshCollider;
    private bool collided;
    private bool hitWall;
    private bool hitFloor;
    private float restitutionFactor;
    public LineRenderer ground;
    // used when collision since yVelocity will be incremented, we dont want to use that one anymore
    private float initYVelocity;
    // ANGLES ARE INVERTED, 
    // I.E. SIN(ANGLE)*VELOCITY = X VELOCITY   
    // COS(ANGLE)*VELOCITY = Y VELOCITY   
    // Start is called before the first frame update
    void Start () {
        collided = false;
        restitutionFactor = 0.5f; // chosen by me, i.e. collision absorbs 50% of the force
        gravity = 0.00002f; // found the value by trial and error. this value works well for our game
    }

    // Update is called once per frame
    // Since no wind restriction on cannonballs
    // we assume that X velocity won't change, but Y velocity will (by gravity)
    void Update () {
        // checking if out of bounds/landed in water to despawn the bullet
        checkValidState ();
        checkIfCollision ();

        // if our cannonball hasn't collided, then we keep applying the same physics since nothing has changed
        if (!collided) {

            // gravity is already negative
            yVelocity -= gravity;
            gravity += gravity * Time.deltaTime; // instead of squaring we add (it plays better with our game to not actually have gravity at -9.81 squared)
            // our previous calculations were with Update() but we switched to Time.deltaTime
            // instead of refactoring the other code, we'll just multiply by a constant of 400 here so it plays the same way it previously did :)
            transform.position = new Vector3 (transform.position.x + xVelocity * 400 * Time.deltaTime, transform.position.y + yVelocity * 400 * Time.deltaTime, transform.position.z);
            // if our cannonball has collided, then we need to apply specific physics to it and eventually despawn it since it will become a "stopped" bullet
        } else {
            // we want to reduce our xVelocity until we get 0
            // after collision we despawn after 3 seconds

            initYVelocity -= gravity;
            gravity += gravity * Time.deltaTime;

            // yVelocity -= gravity;
            // gravity += gravity * Time.deltaTime / 12;
            transform.position = new Vector3 (transform.position.x + xVelocity * 400 * Time.deltaTime, transform.position.y + initYVelocity * 400 * Time.deltaTime, transform.position.z);

            // i.e. we're at the lowest point (ideally I would keep checking for collisions instead of an if statement but the statement simplifies the physics)
            // if this is the case, we no longer want any Y velocity
            if (transform.position.y <= -1.94f) {
                gravity = 0f;
                initYVelocity = 0f;
                // our ball is constantly rolling against the floor so we reduce it (simulates friction)
                xVelocity -= xVelocity * Time.deltaTime;
                // we despawn after 7 seconds (it will be very slow here, so we can safely assume that the cannonball has fully stopped)
                Invoke ("Despawn", 7);

            }

        }

    }

    public void setVelocities (float x, float y) {
        xVelocity = x;
        yVelocity = y;
        initYVelocity = y;
    }

    // BOILER PLATE CODE, didn't have time to refactor it
    // our collision will play with the STANDARD outline
    // i.e. it will assume perlin noise didn't happen
    // I couldn't find a way to make the ba
    // we use 4 Rays, right, -right, up, -up so we can look at all 4 axis directions
    // WE ONLY BOUNCE THE CANNONBALL ONCE (the # of bounces wasn't specified in assignment, only that it needed to bounce)
    void checkIfCollision () {

        Vector3 direction = new Vector3 (transform.position.x + xVelocity * 400 * Time.deltaTime, transform.position.y + yVelocity * 400 * Time.deltaTime, transform.position.z);
        // Debug.DrawRay (transform.position, transform.right, Color.red);
        // Debug.DrawRay (transform.position, -transform.right, Color.blue);
        // Debug.DrawRay (transform.position, transform.up, Color.green);
        // Debug.DrawRay (transform.position, -transform.up, Color.yellow);

        RaycastHit hitInfo;
        // checking for transform.right direction
        // basically in whichever ray detected a collision, we will use this ray direction to "inverse" the direction of our cannonball
        // if we hit transform.right we invert it
        if (Physics.Raycast (transform.position, transform.right, out hitInfo, 0.12f) 
        || Physics.Raycast (transform.position, -transform.right, out hitInfo, 0.12f)
        || Physics.Raycast (transform.position, transform.up, out hitInfo, 0.12f)
        || (Physics.Raycast (transform.position, -transform.up, out hitInfo, 0.12f))) {
            if (hitInfo.transform.name == "BalloonCollider"){
                Debug.Log("Its BALLOON");
                // need to detroy parent, otherwise we're simply destroyer the mesh collider
                Destroy(hitInfo.transform.parent.gameObject);
            // we only want to modify the velocities if it hasn't collided already, otherwise we ignore
            } else{
            collided = true;
            // Debug.Log (hitInfo.transform.name == "Ground (2)");
            xVelocity *= -1 * restitutionFactor;
            initYVelocity *= restitutionFactor;
            gravity = 0.00002f; // found the value by trial and error. this value works well for our game
            // we divide our yVelocity by 2 (there's no real reason for this, just a nice simple way of making physics a little more realistic without overcomplicating physics)
            yVelocity /= 2;

        }
        // } else if (Physics.Raycast (transform.position, -transform.right, out hitInfo, 0.12f)) {
        //     // checking for -transform.right direction
        //     xVelocity *= -1 * restitutionFactor;
        //     initYVelocity *= restitutionFactor;

        //     gravity = 0.00002f; // found the value by trial and error. this value works well for our game

        //     // we divide our yVelocity by 2 (there's no real reason for this, just a nice simple way of making physics a little more realistic without overcomplicating physics)
        //     yVelocity *= 2;
        //     collided = true;
        //     if (hitInfo.transform.name == "Balloon"){
        //     Destroy(hitInfo.transform.gameObject);
        // }
        // } else if (Physics.Raycast (transform.position, transform.up, out hitInfo, 0.12f)) {
        //     gravity = 0.00002f; // found the value by trial and error. this value works well for our game
        //     xVelocity *= -1 * restitutionFactor;
        //     initYVelocity *= restitutionFactor;

        //     // checking for transform.up direction
        //     collided = true;
        //     if (hitInfo.transform.name == "Balloon"){
        //     Destroy(hitInfo.transform.gameObject);
        // }

        // } else if (Physics.Raycast (transform.position, -transform.up, out hitInfo, 0.12f)) {
        //     gravity = 0.00002f; // found the value by trial and error. this value works well for our game
        //     xVelocity *= -1 * restitutionFactor;
        //     initYVelocity *= restitutionFactor;

        //     // checking for -transform.up direction
        //     collided = true;
        //     if (hitInfo.transform.name == "Balloon"){
        //     Destroy(hitInfo.transform.gameObject);
        // }
        }


    }

    // no instructions were giving about upper Y bound, so it won't despawn if it goes out of screen in position Y, we will just let it come back down
    void checkValidState () {
        // checking if out of bounds by X or Y
        if (transform.position.y < -5.2f || transform.position.x < -9.5f || transform.position.x > 9f) {
            Despawn ();
        }
        // checking if bullet is in water
        if (transform.position.x > -3.75f && transform.position.x < 1.45f && transform.position.y < -1.8) {
            Despawn ();
        }

        // checking if out of bounds by landing in water

    }

    void Despawn () {
        Destroy (gameObject);
    }
}