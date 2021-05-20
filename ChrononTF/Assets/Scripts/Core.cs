using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public bool isTimeStopped;
    public float stoppedPeriod;

    GameObject[] stoppableObjects;

    // Start is called before the first frame update
    void Start()
    {
        stoppableObjects = (GameObject[])FindObjectsOfType(typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
