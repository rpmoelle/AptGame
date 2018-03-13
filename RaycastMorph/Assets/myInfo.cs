using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myInfo : MonoBehaviour {
    public bool grabbed;
    GameObject player;
    public bool watched;
    Text myLabel;
    public string label;    //item description
    public bool sallyObject;
    public bool wrongCombine;
    public string tagReveal;    //item description with type descriptor (e.g. "explosive sticks" and "tasty")
    public Vector3 startPos;
    public Quaternion startRot;

 

   

  

//isabella: make a separate text field (label) that shows after it's been wrongly combined to tell the stats. need new bool too

// Use this for initialization
void Start() {
        myLabel = GameObject.Find("ScreenCanvas/ItemLabel").GetComponent<Text>();
        wrongCombine = false;
        startPos = this.gameObject.transform.position;
        startRot = this.gameObject.transform.rotation;

        if (tag == "COMBO")
        {
            transform.localRotation *= Quaternion.Euler(0, 180, 0);
        }
    }

    // Update is called once per frame
    void Update() {
        if (grabbed) {
            //if grabbed, follow the mother ray
            //become its child
            //this.transform.parent = player.transform;
        }

        if (watched) {
            //  if(myLabel != null) myLabel.enabled = true;
        }
        else {
            //if (myLabel != null) myLabel.enabled = false;
        }

        //Get the present to look at the player
        
    }
}
