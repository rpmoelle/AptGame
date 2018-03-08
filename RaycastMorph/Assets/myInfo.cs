using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myInfo : MonoBehaviour {
    public bool grabbed;
    GameObject player;
    public bool watched;
    Text myLabel;
    public string label;
    public bool sallyObject;
    public bool wrongCombine;
    public Vector3 startPos;
    public Quaternion startRot;

    //isabella: make a separate text field (label) that shows after it's been wrongly combined to tell the stats. need new bool too

    // Use this for initialization
    void Start() {
        myLabel = GameObject.Find("ScreenCanvas/ItemLabel").GetComponent<Text>();
        wrongCombine = false;
        startPos = this.gameObject.transform.position;
        startRot = this.gameObject.transform.rotation;
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
    }
}
