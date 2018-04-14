using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterInfo : MonoBehaviour
{
    public PlayerControlStickyGaze playerScript;
    public bool isSally;
    AudioSource audio;

    private void OnCollisionEnter(Collision collision)
    {
        //get the task requester:
        string requestor = playerScript.currentRequestor;
        Debug.Log("EAT"+ collision.gameObject.name);
        if (collision.gameObject.tag == "COMBO")
        {
            Debug.Log("COMBO..." + requestor);
            //if this is a sally object
            if (this.gameObject.name == requestor)
            {
                //correctly gave to right person.
                Debug.Log("Got here.");
                audio.Play();
                //destroy combo object and gain a point
                //Destroy(collision.gameObject);

                playerScript.detachItems();
                playerScript.cleanCam();
                playerScript.nextTask();

                if(collision.gameObject.name == "SlimyCucumber")
                {
                    //OPEN APT2
                    // Destroy(playerScript.door1);
                    playerScript.door1.transform.position = new Vector3(playerScript.door1OpenPosition.x, playerScript.door1OpenPosition.y, playerScript.door1OpenPosition.z);
                    playerScript.door1.transform.eulerAngles = playerScript.door1OpenRotation;
                }
                if (collision.gameObject.name == "SpicyPoops")
                {
                    //OPEN APT3
                    // Destroy(playerScript.door1);
                   // playerScript.door2.transform.position = new Vector3(playerScript.door2OpenPosition.x, playerScript.door2OpenPosition.y, playerScript.door2OpenPosition.z);
                    playerScript.door2.transform.eulerAngles = playerScript.door2OpenRotation;
                }
                if (collision.gameObject.name == "Popcorn")
                {
                    //OPEN Theatre
                    // Destroy(playerScript.door1);
                    playerScript.door3.transform.position = new Vector3(playerScript.door3OpenPosition.x, playerScript.door3OpenPosition.y, playerScript.door3OpenPosition.z);
                    playerScript.door3.transform.eulerAngles = playerScript.door3OpenRotation;
                }
            }
            if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                playerScript.gameObject.GetComponent<AudioSource>().Play();
            }



            /* //if this is not a sally object
             if (!isSally)
             {
                 //correctly gave to right person (Bob).
                 Debug.Log("Got here.");
                 audio.Play();
                 //destroy combo object and gain a point
                 playerScript.detachItems();
                 playerScript.cleanCam();
                // Destroy(collision.gameObject);
                 playerScript.nextTask();
             }*/
            else
            {
                //wrong person!
                if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
                {
                    playerScript.gameObject.GetComponent<AudioSource>().Play();
                }

            }



        }
        else
        {
            if (!playerScript.gameObject.GetComponent<AudioSource>().isPlaying)
            {
                playerScript.gameObject.GetComponent<AudioSource>().Play();
            }

        }
    }
    // Use this for initialization
    void Start()
    {
        //playerScript = GameObject.Find("Main Camera").GetComponent<PlayerControlStickyGaze>();
        audio = this.gameObject.GetComponent<AudioSource>();
        Debug.Log("FUCK");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
