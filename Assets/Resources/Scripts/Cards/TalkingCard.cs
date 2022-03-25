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
    private float eyeOpenTimerMax = 3.0f;
    private float eyeOpenTimer = 2.0f;
    private float eyeCloseTimerMax = 0.2f;
    private float eyeCloseTimer = -1f;

    //speechTimers
    private float speakShowTimerMax = 3.0f;
    private float speakShowTimer = -1f;
    private float speakHideTimerMax = 8.0f;
    private float speakHideTimer = 8.0f;

    //mouthAnimationTimers (only used when speaking)
    private float mouthOpenTimerMax = 0.3f;
    private float mouthOpenTimer = 0.3f;
    private float mouthCloseTimerMax = 0.3f;
    private float mouthCloseTimer = -1f;

    //random phrases to speak when talking
    private string idlePhrase1 = "Hello";
    private string idlePhrase2 = "World";
    private string idlePhrase3 = "Random";
    private string idlePhrase4 = "Test";
    private string idlePhrase5 = "Something";


    //add the materials for the animation in the inspector
    public Material eyesOpen;
    public Material eyesClosed;
    public Material mouthOpen;
    public Material mouthClosed;

    private string original_name;

    //add the phrases for saying in the ide

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
                eyeOpenTimer = eyeOpenTimerMax;
            }
        }

        #endregion



        #region "Speaking after idle time"

        //if we are talking
        if (speakShowTimer > 0)
        {
            speakShowTimer -= Time.deltaTime;

            #region "Animating mouth whilst speaking

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

            #endregion


            //if the timer has now run out, not talking so close our mouth, reset our name, start the speakHideTimer, and reset the mouth open/close timers ready for the next time
            if (speakShowTimer <= 0)
            {
                name_child.GetComponent<TextMesh>().text = original_name;
                mouth_child.GetComponent<MeshRenderer>().material = mouthClosed;
                speakHideTimer = speakHideTimerMax;
            }
        }

        //else we are not talking at the moment
        else
        {
            speakHideTimer -= Time.deltaTime;

            //if the timer has now run out, xxxxxxxxxxxxxxxx
            if (speakHideTimer <= 0)
            {
                name_child.GetComponent<TextMesh>().text = "HELLO";
                mouth_child.GetComponent<MeshRenderer>().material = mouthOpen;
                speakShowTimer = speakShowTimerMax;
            }
        }













        ////else our eyes are closed
        //else
        //{
        //    eyeCloseTimer -= Time.deltaTime;

        //    //if the timer has now run out, open our eyes and start the open eyes timer
        //    if (eyeCloseTimer <= 0)
        //    {
        //        eye_child.GetComponent<MeshRenderer>().material = eyesOpen;
        //        eyeOpenTimer = eyeOpenTimerMax;
        //    }
        //}

        #endregion

    }
}
