using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallonSpawner : MonoBehaviour {

    private float ropeSegLen = 0.25f;
    private int segmentLength = 35;
    private float lineWidth = 0.1f;
    public GameObject WaterLine;
    private float[] windForce;
    public Text text;
    private float currentWindForce;
    public Balloon balloon;

    // Use this for initialization
    void Start () {
        windForce = new float[] {-0.025f, -0.02f, -0.015f, -0.01f, 0f, 0.01f, 0.015f, 0.02f, 0.025f, };
        // change wind direction every 2 seconds
        InvokeRepeating ("WindDirection", 0f, 2f);
        // create ballon every 1.25s
        InvokeRepeating ("CreateBalloonHead", 0f, 1.25f);

    }

    void WindDirection () {

        int index = Random.Range (0, 8);

        currentWindForce = windForce[index];

        float proportionalForce = currentWindForce / 0.025f * 100;

        text.text = "Wind Force: " + proportionalForce.ToString ("0") + " %";

    }

    public float getCurrentWindForce () {
        return currentWindForce;
    }

    // we spawn in the water
    void CreateBalloonHead () {

        float xPosition = Random.Range (-1.5f, 0f);
        // hard coded for height spawn to be a little bit on the water
        float yPosition = -2.5f;

        Balloon t_balloonhead = Instantiate (balloon, new Vector3 (xPosition, yPosition, 0.0f), WaterLine.transform.rotation)as Balloon;
        t_balloonhead.isClone = true;

    }

    // Update is called once per frame
    void Update () { }

}