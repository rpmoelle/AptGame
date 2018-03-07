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


    

    // Use this for initialization
    void Start () {
        myLabel = GameObject.Find("ScreenCanvas/ItemLabel").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        if (grabbed)
        {
            //if grabbed, follow the mother ray
            //become its child
            //this.transform.parent = player.transform;
        }

        if (watched)
        {
          //  if(myLabel != null) myLabel.enabled = true;
        }
        else
        {
            //if (myLabel != null) myLabel.enabled = false;
        }
	}
}
