using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CannonControl : MonoBehaviour
{
    public GameObject Gun;
    public Bullet Bullet;
    public Text text;
    // Idea : Each cannon has their own distinct velocties
    // they both start at the same velocity however
    // but you can change one cannon to 100% and the other one could still be at the default value
    // when switching cannons, it will display its corresponding power for its velocity
    private float velocity = 0.01f;
    private float ratioPower;

    // Start is called before the first frame update
    void Start()
    {
        ratioPower = velocity / 0.035f * 100;
        text.text = "Power : " + ratioPower.ToString("0") + " %";
    }

    // Update is called once per frame
    void Update()
    {
        ratioPower = velocity / 0.035f * 100;

        text.text = "Power : " + ratioPower.ToString("0") + " %";
        // our cannons have different angles (since they're aiming from different direction)
        // they will each have their own tag so we know which angle restriction to enforce
        if (Gun.CompareTag("Cannon1"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && Gun.transform.rotation.z < -0.087f)
            {
                Gun.transform.Rotate(0, 0, 10);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && Gun.transform.rotation.z > -0.7f)
            {
                Gun.transform.Rotate(0, 0, -10);
            }
        }

        if (Gun.CompareTag("Cannon2"))
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && Gun.transform.rotation.z > 0.087f)
            {
                Gun.transform.Rotate(0, 0, -10);
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) && Gun.transform.rotation.z < 0.7f)
            {
                Gun.transform.Rotate(0, 0, 10);
            }
        }

         if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
            velocity -= 0.003f;
            // lower bound cap
            if (velocity <= 0.007f){
                velocity = 0.007f;
            }
            ratioPower = velocity / 0.035f * 100;
            text.text = "Power : " + ratioPower.ToString("0") + " %";
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
            {
            velocity += 0.001f;
            // higher bound cap
            if (velocity >= 0.035f){
                velocity = 0.035f;
            }
            ratioPower = velocity / 0.035f * 100;

            text.text = "Power : " + ratioPower.ToString("0") + " %";
        }

        // we fire on space
        if (Input.GetKeyDown(KeyCode.Space))
            {
           Fire();
            }
    }

    void Fire()
    {
        Bullet t_bullet = Instantiate(Bullet, Gun.transform.position, Gun.transform.rotation);
        t_bullet.setVelocities(velocity*Mathf.Sin(Gun.transform.localEulerAngles.z * Mathf.Deg2Rad) * -1.0f, // multiply by -1 because angles are inverted in X
        Mathf.Abs(velocity*Mathf.Cos(Gun.transform.localEulerAngles.z * Mathf.Deg2Rad))); // absolute value for Y axis since we can't shoot downwards
    }







}
