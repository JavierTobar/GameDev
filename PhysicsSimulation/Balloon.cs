using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour {

    public BallonSpawner balloonSpawner;
    private LineRenderer lineRenderer;
    private List<RopeSegment> ropeSegments = new List<RopeSegment> ();
    private float ropeSegLen = 0.2f;
    private int segmentLength = 10;
    private float lineWidth = 0.05f;
    public bool isClone; // So we don't delete our original balloon that is being used to clone/spawn the other balloons
    public MeshCollider balloonCollider;

    // Start is called before the first frame update
    void Start () {

        lineRenderer = this.GetComponent<LineRenderer> ();
        lineRenderer.GetPosition (5);
        // Debug.Log("pos is " + lineRenderer.GetPosition(5));

        // Debug.Log("count is " + lineRenderer.positionCount);

        Vector3 ropeStartPoint = lineRenderer.GetPosition (5);

        for (int i = 0; i < lineRenderer.positionCount; i++) {
            // we add each vertex to ropeSegment array
            ropeSegments.Add (new RopeSegment (lineRenderer.GetPosition (i)));
            ropeStartPoint.y -= ropeSegLen;
        }
    }

    // Position updates for the balloon
    // Wind factor only when above the mountains, i.e. above 0 on Y axis
    // also check if we're out of bounds, in which case, we despawn the ballonhead
    void Update () {
        this.DrawRope ();

        if (isClone) {

            // Despawn if out of bounds (out of visual screen)
            if (transform.position.x < -9f || transform.position.x > 10f || transform.position.y > 5f) {
                Despawn ();
            }
            // when below mountains we only have vertical velocity
            if (transform.position.y <= 0.3f) {
                transform.position = new Vector3 (transform.position.x, transform.position.y + 0.0028f, transform.position.z);
            }
            // Wind Factor
            else {

                transform.position = new Vector3 (transform.position.x + balloonSpawner.getCurrentWindForce (), transform.position.y + 0.0028f, transform.position.z);

            }
        }

    }

    void Despawn () {
        if (isClone) // if a clone, delete it, otherwise it's the original and we don't want to delete it
            Destroy (gameObject);
    }

    private void FixedUpdate () {
        this.Simulate ();
    }

    private void Simulate () {
        // SIMULATION
        Vector2 forceGravity = new Vector2 (0f, 0f);

        for (int i = 1; i < lineRenderer.positionCount; i++) {
            RopeSegment firstSegment = ropeSegments[i];
            Vector2 velocity = firstSegment.posNow - firstSegment.posOld;
            firstSegment.posOld = firstSegment.posNow;
            firstSegment.posNow += velocity * Time.fixedDeltaTime;

            // firstSegment.posNow += forceGravity * Time.fixedDeltaTime;
            this.ropeSegments[i] = firstSegment;
        }

        //CONSTRAINTS
            this.ApplyConstraint();
    }

    private void ApplyConstraint () {

        RopeSegment firstSegment = ropeSegments[5];
        // RopeSegment ropeStartSegment = ropeSegments[0];
        this.ropeSegments[0] = firstSegment;
        // firstSegment.posNow = transform.position;
        this.ropeSegments[5] = firstSegment;
        // this.ropeSegments[10] = this.ropeSegments[5];

        // We adjust the position of the mesh collider to be at ropesegment[0]
        Debug.Log(transform.position);
        balloonCollider.transform.position = transform.position;
        // Two points need to have fixed distance apart (0.25)
        for (int i = 0; i < lineRenderer.positionCount - 1; i++) {

            // we retrieve i-th segment and i-th + 1 segment (notice -1 in for loop)
            RopeSegment firstSeg = ropeSegments[i];
            RopeSegment secondSeg = ropeSegments[i + 1];

            // we retrieve their distance
            float distance = (firstSeg.posNow - secondSeg.posNow).magnitude;
            // we check if there's an error, i.e. if distance is greater than 0.25 or less than 0.25
            float difference = Mathf.Abs (distance - ropeSegLen);
            Vector2 changeDir = Vector2.zero;

            if (i < 5 && i != 0) {

                distance = (firstSeg.posNow - this.ropeSegments[5].posNow).magnitude;
                if (distance > ropeSegLen) {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

                    // Case 2 : Our points are too close together
                } else if (distance < ropeSegLen) {
                    changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
                }

                Vector2 changeAmount = changeDir * difference;
                secondSeg.posNow += changeAmount;
                ropeSegments[i + 1] = secondSeg;

            } else {
                // Case 1 : Our points are too far apart
                if (distance > ropeSegLen) {
                    changeDir = (firstSeg.posNow - secondSeg.posNow).normalized;

                    // Case 2 : Our points are too close together
                } else if (distance < ropeSegLen) {
                    changeDir = (secondSeg.posNow - firstSeg.posNow).normalized;
                }

                Vector2 changeAmount = changeDir * difference;
                // firstSeg.posNow -= changeAmount * 0.5f;
                // secondSeg.posNow += changeAmount * 0.5f;

                if (i != 5) {
                    // we add half the amount to individual points
                    firstSeg.posNow -= changeAmount * 0.5f;
                    ropeSegments[i] = firstSeg;
                    secondSeg.posNow += changeAmount * 0.5f;
                    ropeSegments[i + 1] = secondSeg;
                } else {
                    secondSeg.posNow += changeAmount;
                    ropeSegments[i + 1] = secondSeg;
                }

            }

        }
    }
// inspired from https://www.youtube.com/watch?v=FcnvwtyxLds
// still somewhat buggy, need to fix it
    private void DrawRope () {
        float lineWidth = this.lineWidth;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        Vector3[] ropePositions = new Vector3[lineRenderer.positionCount];
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            ropePositions[i] = this.ropeSegments[i].posNow;
        }

        lineRenderer.positionCount = ropePositions.Length;
        lineRenderer.SetPositions (ropePositions);
    }

    public struct RopeSegment {
        public Vector2 posNow;
        public Vector2 posOld;

        public RopeSegment (Vector2 pos) {
            this.posNow = pos;
            this.posOld = pos;
        }
    }
}
