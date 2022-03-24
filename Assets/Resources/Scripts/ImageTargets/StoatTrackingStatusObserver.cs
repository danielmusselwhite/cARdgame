using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;


public class StoatTrackingStatusObserver : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;
    private Status previousState;

    // Start is called before the first frame update
    void Start()
    {
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
            Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
        }

        // if the state was tracked, but it isn't anymore ...
        if (status.Status != Status.TRACKED && previousState == Status.TRACKED){
            // ... then we should delete this ImageTarget, unless this is the only ImageTarget
            Debug.Log(gameObject.name + " | Was tracked before, but now it is not");

            // if there is more than 1 Stoat prefab currently...
            if (GameObject.FindGameObjectsWithTag("Card_Stoat").Length > 1)
                Destroy(gameObject, 0); //... then destroy the additional ImageTarget for this stoat card

        }


        previousState = status.Status; //updating the previous state
    }
    void OnDestroy()
    {
        if (mObserverBehaviour != null)
            mObserverBehaviour.OnTargetStatusChanged -= OnStatusChanged;
    }


















    //bool lastTrackingStatus;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    lastTrackingStatus = isTrackingImageTarget(gameObject.name);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    bool isTracking = isTrackingImageTarget(gameObject.name);
    //    // if we have just started tracking this imageTarget...
    //    if (isTracking == true && lastTrackingStatus == false)
    //    {
    //        //... create a new imageTarget of this type in case an identical card is played
    //        Debug.Log("Just started tracking " + gameObject.name);
    //    }

    //    // if we have just stopped tracking this imageTarget...
    //    if (isTracking == false && lastTrackingStatus == true)
    //    {
    //        //... destroy this imageTarget (unless there is only 1 of them, as we want a minimum of 1)
    //        Debug.Log("Just stopped tracking " + gameObject.name);
    //    }
    //    lastTrackingStatus = isTrackingImageTarget(gameObject.name);
    //}


    //private ObserverBehaviour mObserverBehaviour;

    //private bool isTrackingImageTarget(string imageTargetName)
    //{
    //    IVuModelTargetState
    //    Vuforia.DataSetTrackableBehaviour
    //    //Vuforia.DefaultObserverBehaviourPlaceholder
    //}
}


