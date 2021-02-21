using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    public GunSystem Gun;
    public GameObject ammoPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (Gun != null)
        {
            Gun.magazineSize++;
        }
        Destroy(gameObject);
    }
}
