using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCharacter : MonoBehaviour
{

    public GameObject cannon1;
    public GameObject cannon2;
    int whichCannonIsOn = 1;
    // Start is called before the first frame update
    void Start()
    {
        cannon1.gameObject.SetActive(true);
        cannon2.gameObject.SetActive(false);

    }
    
    // method to switch cannons by pressing TAB
    public void SwitchCannon()
    {
        switch (whichCannonIsOn)
        {
            case 1:
                // second cannon is on now
                whichCannonIsOn = 2;
                cannon1.gameObject.SetActive(false);
                cannon2.gameObject.SetActive(true);
                break;

            case 2:
                // first cannon is on now
                whichCannonIsOn = 1;
                cannon1.gameObject.SetActive(true);
                cannon2.gameObject.SetActive(false);
                break;
        }
    }

    // Update is called once per frame
     void Update () {
         // we change cannon when we press TAB
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchCannon();
        }
     }

    
}
