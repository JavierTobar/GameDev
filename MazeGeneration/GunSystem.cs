using UnityEngine;
using TMPro;


// script is inspired from https://www.youtube.com/watch?v=bqNW08Tac0Y
// professor said we're allowed to use prefabs for physics, thats why i used this script 
// it's been modified to fit the assignment though

public class GunSystem : MonoBehaviour
{
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, timeBetweenShots;
    public int magazineSize, bulletsPerTap; // magazineSize tracks the number of bullets left in the magazine
    public bool allowButtonHold;
    public int bulletsLeft = 8; // there are 8 bullets across the map, if all 8 have been shot and maze isn't destroyed then it's game over
    int bulletsShot;

    //bools 
    bool shooting, readyToShoot;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics

    public TextMeshProUGUI text;


    private void Awake()
    {
        readyToShoot = true;
    }

    private void Update()
    {
        MyInput();

        //SetText
        text.SetText(magazineSize + " bullets");
    }
    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        // if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && magazineSize > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            // deletes the object
            Destroy(rayHit.collider.gameObject);
            // Debug.Log(rayHit.collider.name);
        }

        bulletsLeft--;
        bulletsShot--;
        magazineSize--;



        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
}