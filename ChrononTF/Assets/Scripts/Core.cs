using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public bool isTimeStopped;
    public float stoppedPeriod;

    public GameObject stoppables;

    List<GameObject> stoppableObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform stoppable in stoppables.transform)
        {
            stoppableObjects.Add(stoppable.gameObject);
        }

        if (isTimeStopped)
        {
            ToggleTime(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            ToggleTime(!isTimeStopped);
        }
    }

    /// <summary> 
    ///activeness = true ? play time : stop time
    /// </summary>
    /// <param name="activeness"></param>
    void ToggleTime(bool activeness)
    {
        foreach (GameObject gameObject in stoppableObjects)
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = !activeness;
            }
        }
        isTimeStopped = activeness;
    }
}
