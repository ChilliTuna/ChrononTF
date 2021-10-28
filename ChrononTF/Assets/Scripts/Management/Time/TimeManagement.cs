using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManagement : MonoBehaviour
{
    public bool isTimeStopped;
    public float stoppedPeriod;

    public GameObject stoppables;

    List<GameObject> stoppableObjects = new List<GameObject>();
    List<Vector3> velocities = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform stoppable in stoppables.transform)
        {
            stoppableObjects.Add(stoppable.gameObject);
            velocities.Add(new Vector3(0, 0, 0));
        }

        if (isTimeStopped)
        {
            SetTimeFlux(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary> 
    ///activeness = true ? play time : stop time;
    /// </summary>
    /// <param name="activeness"></param>
    public void SetTimeFlux(bool activeness)
    {
        for (int i = 0; i < stoppableObjects.Count; i++)
        {
            Rigidbody rb = stoppableObjects[i].GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (activeness == false)
                {
                    velocities[i] = rb.velocity;
                }
                else
                {
                    rb.velocity = velocities[i];
                }
                rb.isKinematic = !activeness;
            }
        }
        isTimeStopped = activeness;
    }

    public void SwapTimeFlux()
    {
        SetTimeFlux(!isTimeStopped);
    }
}
