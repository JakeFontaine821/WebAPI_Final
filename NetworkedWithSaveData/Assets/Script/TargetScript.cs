using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public Transform min;
    public Transform max;
    // Start is called before the first frame update
    void Start()
    {
        NewPostion();
    }

    public void NewPostion()
    {
        float xPos = Random.Range(min.position.x, max.position.x);
        float yPos = Random.Range(min.position.y, max.position.y);
        float zPos = Random.Range(min.position.z, max.position.z);
        transform.position = new Vector3(xPos, yPos, zPos);
    }
}
