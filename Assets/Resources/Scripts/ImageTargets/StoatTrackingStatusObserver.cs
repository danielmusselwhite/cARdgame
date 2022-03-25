using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class StoatTrackingStatusObserver : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;
    private Status previousState;
    private static int id;

    // Start is called before the first frame update
    void Start()
    {
        id += 1;

        previousState = Status.NO_POSE; // defaulting to not being observed

        ObserverBehaviour mObserverBehaviour = GetComponent<ObserverBehaviour>();

        if (mObserverBehaviour != null)
            mObserverBehaviour.OnTargetStatusChanged += OnStatusChanged;
    }
    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        //Debug.Log(gameObject.name + " | Status is: "+ status.Status +", statusInfo is: {1} " + status.StatusInfo);

        // if the state was not tracked, but it now is ...
        if (status.Status == Status.TRACKED && previousState != Status.TRACKED) { 
            // ... so lets create another instance of this ImageTarget, in case an identical card is played again
            Debug.Log(gameObject.name + " | Was not tracked before, but now it is");
            // creating a new instance of the ImageTarget for the Stoat, in case an additional Stoat card is played
            var track = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            track.name = "ImageTarget_Stoat_" + id;
        }

        // if the state was tracked, but it isn't anymore ...
        if (status.Status != Status.TRACKED && previousState == Status.TRACKED){
            // ... then we should delete this ImageTarge
            Debug.Log(gameObject.name + " | Was tracked before, but now it is not");
            Destroy(gameObject, 0);
        }


        previousState = status.Status; //updating the previous state
    }
    void OnDestroy()
    {
        if (mObserverBehaviour != null)
            mObserverBehaviour.OnTargetStatusChanged -= OnStatusChanged;
    }

}


