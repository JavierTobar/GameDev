using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class GameOver : MonoBehaviour
{
    // Start is called before the first frame update

    public GunSystem Gun;
    public Maze Maze;
    public UnityStandardAssets.Characters.FirstPerson.RigidbodyFirstPersonController FPS;
    public TextMeshProUGUI gameOverText;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // check for win situation first i.e. 0 bullets and win

        if (Maze.destroyedSolution && FPS.m_RigidBody.position.z >= 235)
        {
            Time.timeScale = 0;
            gameOverText.SetText("You won");
        }

        if (Gun.bulletsLeft == 0)
        {
            // check if we destroyed maze solution => yes => win
            // check if we're in the other side too => z >= VALUE for FPS controller
            // else lose
            Time.timeScale = 0;
            gameOverText.SetText("Game over. No more bullets and didn't destroy maze solution");
        }
        else
        {
            // COMMENT IT OUT if you want to test if you can escape the canyon when falling (you can't :) )
            if (FPS.m_RigidBody.position.y <= 106)
            {
                Time.timeScale = 0;
                gameOverText.SetText("Game over. You fell into the canyon");
            }
        }

    }
}
