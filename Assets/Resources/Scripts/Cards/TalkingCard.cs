using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingCard : MonoBehaviour
{

    // the children we are changing the images for the animations
    GameObject eye_child;
    GameObject mouth_child;
    GameObject name_child;


    //blinkTimers
    private float eyeOpenTimerMaxMax = 6.0f;
    private float eyeOpenTimerMaxMin = 3.0f; // pick random value to wait between this and the max
    private float eyeOpenTimer = 2.0f;
    private float eyeCloseTimerMax = 0.2f;
    private float eyeCloseTimer = -1f;

    //speechTimers
    private float speakShowTimerMax = 6.0f;
    private float speakShowTimer = -1f;
    private float speakHideTimerMaxMax = 30.0f;
    private float speakHideTimerMaxMin = 10.0f; // pick random value to wait between this and the max
    private float speakHideTimer = 8.0f; // TODO - want it talking relatively quickly after playing for sake of testing; set this to larger value later

    //mouthAnimationTimers (only used when speaking)
    private float mouthMoveTimerMax = 2.5f;
    private float mouthMoveTimer = 2.5f;
    private float mouthOpenTimerMax = 0.2f;
    private float mouthOpenTimer = 0.2f;
    private float mouthCloseTimerMax = 0.2f;
    private float mouthCloseTimer = -1f;

    //add the materials for the animation in the inspector
    public Material eyesOpen;
    public Material eyesClosed;
    public Material mouthOpen;
    public Material mouthClosed;

    private string original_name;

    //add the phrases for saying in the ide
    public string[] idlePhrases; // source from the inscryption wikipedia quotes section for each card

    // Start is called before the first frame update
    void Start()
    {

        #region "Getting and storing references to our animation component children"
        // getting reference to the eyes/mouth and name of the talking card so they can blink and talk
        var eye_children = GameObject.FindGameObjectsWithTag("Card_Anim_Eyes");
        var mouth_children = GameObject.FindGameObjectsWithTag("Card_Anim_Mouth");
        var name_children = GameObject.FindGameObjectsWithTag("Card_Name");

        GameObject[][] all_children = { eye_children, mouth_children, name_children };
;
        // find out of all object with the eye tag, which is our child
        for (int j = 0; j < eye_children.Length; j++)
        {
            // if this child is our child...
            if (eye_children[j].transform.parent.name == gameObject.name)
            {
                //store reference to this eye child
                eye_child = eye_children[j];
                break;
            }
                
        }

        // find out of all object with the mouth tag, which is our child
        for (int j = 0; j < mouth_children.Length; j++)
        {
            // if this child is our child...
            if (mouth_children[j].transform.parent.name == gameObject.name)
            {
                //store reference to this mouth child
                mouth_child = mouth_children[j];
                break;
            }
                
        }

        // find out of all object with the name tag, which is our child
        for (int j = 0; j < name_children.Length; j++)
        {
            // if this child is our child...
            if (name_children[j].transform.parent.name == gameObject.name)
            {
                //store reference to this name child
                name_child = name_children[j];
                original_name = name_child.GetComponent<TextMesh>().text;
                break;
            }
                
        }

        #endregion

        
    }

    // Update is called once per frame
    void Update()
    {
        #region "blinking animation"

        //if our eyes are open
        if (eyeOpenTimer > 0)
        {
            eyeOpenTimer -= Time.deltaTime;


            //if the timer has now run out, close our eyes and start the close eyes timer
            if (eyeOpenTimer <= 0)
            {
                eye_child.GetComponent<MeshRenderer>().material = eyesClosed;
                eyeCloseTimer = eyeCloseTimerMax;
            }
        }
        //else our eyes are closed
        else
        {
            eyeCloseTimer -= Time.deltaTime;

            //if the timer has now run out, open our eyes and start the open eyes timer
            if (eyeCloseTimer <= 0)
            {
                eye_child.GetComponent<MeshRenderer>().material = eyesOpen;
                eyeOpenTimer = Random.Range(eyeOpenTimerMaxMin, eyeOpenTimerMaxMax);
            }
        }

        #endregion



        #region "Speaking after idle time"

        //if we are talking
        if (speakShowTimer > 0)
        {
            speakShowTimer -= Time.deltaTime;

            #region "Animating mouth whilst speaking
            //if we still want to move the mouth
            if (mouthMoveTimer > 0)
            {
                mouthMoveTimer -= Time.deltaTime;

                //if our mouth is open
                if (mouthOpenTimer > 0)
                {
                    mouthOpenTimer -= Time.deltaTime;


                    //if the timer has now run out, close our mouth and start the close mouth timer
                    if (mouthOpenTimer <= 0)
                    {
                        mouth_child.GetComponent<MeshRenderer>().material = mouthClosed;
                        mouthCloseTimer = mouthCloseTimerMax;
                    }
                }
                //else our mouth is closed
                else
                {
                    mouthCloseTimer -= Time.deltaTime;

                    //if the timer has now run out, open our mouth and start the open mouth timer
                    if (mouthCloseTimer <= 0)
                    {
                        mouth_child.GetComponent<MeshRenderer>().material = mouthOpen;
                        mouthOpenTimer = mouthOpenTimerMax;
                    }
                }
           
            }
            //else we have stopped moving our mouth, set it back to closed
            else
            {
                mouth_child.GetComponent<MeshRenderer>().material = mouthClosed;
            }


            #endregion


            //if the timer has now run out, not talking so close our mouth, reset our name, start the speakHideTimer, and reset the mouth move timer ready for the next time
            if (speakShowTimer <= 0)
            {
                name_child.GetComponent<TextMesh>().text = original_name;
                mouth_child.GetComponent<MeshRenderer>().material = mouthClosed;
                speakHideTimer = Random.Range(speakHideTimerMaxMin, speakHideTimerMaxMax);
                mouthMoveTimer = mouthMoveTimerMax;

            }
        }

        //else we are not talking at the moment
        else
        {
            speakHideTimer -= Time.deltaTime;

            //if the timer has now run out, pick a random phrase to speak and reset the timer for showing speech
            if (speakHideTimer <= 0)
            {
                name_child.GetComponent<TextMesh>().text = idlePhrases[Random.Range(0,idlePhrases.Length-1)]; // pick a random idle phrase
                mouth_child.GetComponent<MeshRenderer>().material = mouthOpen;
                speakShowTimer = speakShowTimerMax;
            }
        }

        #endregion

    }
}
