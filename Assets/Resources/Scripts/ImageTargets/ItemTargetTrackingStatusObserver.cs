using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class ItemTargetTrackingStatusObserver : MonoBehaviour
{
    private ObserverBehaviour mObserverBehaviour;
    private Status previousState;
    public string baseName;
    private static Dictionary<string, int> imageTarget_counts = new Dictionary<string, int>();

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
        // if the state was not tracked, but it now is ...
        if (status.Status == Status.TRACKED && previousState != Status.TRACKED)
        {
            // ... so lets create another instance of this ImageTarget, in case an identical card is played again
            Debug.Log(gameObject.name + " | Was not tracked before, but now it is");
            // creating a new instance of the ImageTarget for the Stoat, in case an additional Stoat card is played
            var track = Instantiate(gameObject, new Vector3(0, 0, 0), Quaternion.identity);
            incrementCounter(); //increment the counter
            track.name = baseName + imageTarget_counts[baseName];
        }

        // if the state was tracked/extended tracked, but it isn't anymore ...
        if (status.Status != Status.TRACKED && status.Status != Status.EXTENDED_TRACKED && previousState == Status.TRACKED)
        {
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
