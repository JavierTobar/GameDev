using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    private Vector2 normal;

    private LineRenderer line;

    private Vector2 start;
    private Vector2 end;

    private Vector3[] oldPositions;




    // Use this for initialization
    void Start()
    {
        line = GetComponent<LineRenderer>();
        // Stores all the vertices of our line

        this.oldPositions = new Vector3[line.positionCount];

        // constaints : origin and end must be the same from the old line renderer
        // we store our old positions in this array
        line.GetPositions(oldPositions);

        // storing these values because we want to keep them at the end
        start = oldPositions[0];
        end = oldPositions[line.positionCount-1];


        // Debug.Log(positions[0]);
        // Debug.Log(positions[1]);

        // initially we pass the left most and right most argument to initialize our recursion
        PerlinNoiseRecursion(0, line.positionCount - 1);

        // I don't want to change the start and end so I'm going to overwrite them again with their original values

        oldPositions[0] = start;
        oldPositions[line.positionCount - 1] = end;

        // once PerlinRecursion is done, we can set the new positions of the line renderer
        line.SetPositions(oldPositions);
    }

    // We want to create noise between each adjacent vertex
    void AddPerlinNoise(Vector3[] positions)
    {

        // targetPoint - initialPoint to get vector
        Vector2 temp = positions[1] - positions[0];
        Vector2 mid = (positions[1] + positions[0]) / 2;
        Vector2 normal = Vector2.Perpendicular(temp).normalized;

        mid = new Vector2(mid.x + normal.x * 0.1f, mid.y + normal.y * 0.1f); // 
        // Debug.Log("mid is " + mid);
        // line.SetPosition(2, positions[1]);
        // line.SetPosition(1, mid);

        Vector3[] pos = new Vector3[3];
        pos[0] = positions[0];
        pos[1] = mid;
        pos[2] =  positions[1];

        line.SetPositions(pos);



    }

    void PerlinNoiseRecursion(int start, int end){
        // base case / exit condition
        if (start > end)
        {
            return;
        }
        // index of the mid point
        int midIndex = (start + end) / 2;


        // if mid == start then start == end
        // then the normal will be 0 and we won't get a shift
        // so we have to take care of this edge case manually by picking nearby point to create a non zero normal
        if (midIndex == start)
        {
            // if start = end = > 0, we decrement our start so our end is bigger index
            if (start > 0)
            {
                start--;
                Vector2 temp = oldPositions[end] - oldPositions[start];
                Vector2 mid = (oldPositions[end] + oldPositions[start]) / 2;
                Vector2 normal = Vector2.Perpendicular(temp).normalized;
                // random shift for both x and y direction
                float xShift = Random.Range(-0.1f, 0.1f);
                float yShift = Random.Range(-0.1f, 0.1f);
                // we shift the point by a random value
                mid = new Vector2(mid.x + normal.x * xShift, mid.y + normal.y * yShift);
                oldPositions[midIndex] = mid;
                start++; // we re-increment to avoid infinite loop

                // if start = end = 0, we increment end by 1
            } else {
                end++;
                Vector2 temp = oldPositions[end] - oldPositions[start];
                Vector2 mid = (oldPositions[end] + oldPositions[start]) / 2;
                Vector2 normal = Vector2.Perpendicular(temp).normalized;
                // random shift for both x and y direction
                float xShift = Random.Range(-0.1f, 0.1f);
                float yShift = Random.Range(-0.1f, 0.1f);
                // we shift the point by a random value
                mid = new Vector2(mid.x + normal.x * xShift, mid.y + normal.y * yShift);
                oldPositions[midIndex] = mid;
                end--; // we re=decrement to avoid infinite loop
            }
            }
         
        // when midIndex != start, i.e. start != end
        else 
        {
            Vector2 temp = oldPositions[end] - oldPositions[start];
            Vector2 mid = (oldPositions[end] + oldPositions[start]) / 2;
            Vector2 normal = Vector2.Perpendicular(temp).normalized;
            // random shift for both x and y direction
            float xShift = Random.Range(-0.1f, 0.1f);
            float yShift = Random.Range(-0.1f, 0.1f);
            // we shift the point by a random value
            mid = new Vector2(mid.x + normal.x * xShift, mid.y + normal.y * yShift);

            oldPositions[midIndex] = mid;

        }
        PerlinNoiseRecursion(start, midIndex - 1);
        PerlinNoiseRecursion(midIndex + 1, end);
    }

// Update is called once per frame
void Update () {

    }
}