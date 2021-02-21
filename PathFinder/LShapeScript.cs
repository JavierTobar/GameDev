using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// the purpose of this script is to retrieve the reflex vertices of our obstacle
public class LShapeScript : MonoBehaviour
{
    public GameObject vertex;
    // Start is called before the first frame update
    void Start()
    {
        getVertices();
    }

    // returns the reflex vertices of the instance
    // because of our L shape, the reflex vertices are all the vertices that are not shared
    // between our 2 blocks forming our L 
    public Vector3[] getVertices(){
        Matrix4x4 localToWorld = transform.localToWorldMatrix;

        GameObject lShape = transform.GetChild(1).gameObject;
        GameObject _Shape = transform.GetChild(0).gameObject;
        Mesh mesh = lShape.GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < vertices.Length; i++){
            // Debug.Log(localToWorld.MultiplyPoint3x4(vertices[i]));
            // Instantiate(vertex, localToWorld.MultiplyPoint3x4(vertices[i]), Quaternion.identity);
        }

        mesh = _Shape.GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;
        for (int i = 0; i < vertices.Length; i++){
            // Debug.Log(localToWorld.MultiplyPoint3x4(vertices[i]));
            // Instantiate(vertex, localToWorld.MultiplyPoint3x4(vertices[i]), Quaternion.identity);
        }

        return vertices;



    }
}
