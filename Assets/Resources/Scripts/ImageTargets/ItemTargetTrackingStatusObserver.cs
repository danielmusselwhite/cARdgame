using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ItemTargetTrackingStatusObserver : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;
    private Status currentState;
    private Status previousState;
    public string baseName;
    private static Dictionary<string, int> imageTarget_counts = new Dictionary<string, int>();

    private float destroyTimerMax = 2.0f;
    private float destroyTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {

        previousState = Status.NO_POSE; // defaulting to not being observed

        //  observe when the tracking status has changed
        ObserverBehaviour mObserverBehaviour = GetComponent<ObserverBehaviour>();
        if (mObserverBehaviour != null)
            mObserverBehaviour.OnTargetStatusChanged += OnStatusChanged;
    }

    // Called every game frame
    private void Update()
    {
        // if currentState is Extended_Tracked, then decrement the counter
        if(currentState == Status.EXTENDED_TRACKED)
        {
            destroyTimer -= Time.deltaTime;
            // if the counter is now <= 0, destroy the imageTarget, as we did not find it again within time.
            if(destroyTimer <= 0)
            {
                Debug.Log(gameObject.name + " | Was not found within "+destroyTimerMax+" seconds of becoming Extended_Tracked");
                Destroy(gameObject, 0);

            }

        }
        
    }

    void OnStatusChanged(ObserverBehaviour behaviour, TargetStatus status)
    {
        currentState = status.Status;
        // if the state was not tracked, but it now is ...
        if (currentState == Status.TRACKED && previousState != Status.TRACKED)
        {
            // ... so lets create another instance of this ImageTarget, in case an identical card is played again
            Debug.Log(gameObject.name + " | Was not tracked before, but now it is");
            // creating a new instance of the ImageTarget for the Stoat, in case an additional Stoat card is played
            var track = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            incrementCounter(); //increment the counter
            track.name = baseName + imageTarget_counts[baseName];
        }

        // if the state was tracked, but it is now extended tracked...
        if (currentState == Status.EXTENDED_TRACKED)
        {
            // ... start the destroy timer for 2 seconds
            Debug.Log(gameObject.name + " | Was tracked before, but now it is not");
            destroyTimer = destroyTimerMax;
        }

        // if the state was isn't Tracked or Extended_Tracked, destroy it as we have lost it
        if (currentState != Status.TRACKED && currentState != Status.EXTENDED_TRACKED)
        {
            Debug.Log(gameObject.name + " | Was lost track of");
            Destroy(gameObject, 0);
        }

        previousState = currentState; //updating the previous state
    }
    void OnDestroy()
    {
        if (mObserverBehaviour != null)
            mObserverBehaviour.OnTargetStatusChanged -= OnStatusChanged;
    }

    private void incrementCounter()
    {
        // if this is not present in our dictionary of imageTargets we are counting; add it (initialising count to 0)
        if (!imageTarget_counts.ContainsKey(baseName))
            imageTarget_counts.Add(baseName, 0);
        // else, increment the counter for this imageTarget
        else
            imageTarget_counts[baseName]++;
    }



}
