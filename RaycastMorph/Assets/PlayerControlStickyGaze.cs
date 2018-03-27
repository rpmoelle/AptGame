using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerControlStickyGaze : MonoBehaviour {

    public Camera cam;
    public Collider testCollider;
    public Collider floor;
    public GameObject player;
    int timer;
    bool timeme;
    bool played = false;
    bool moved = false;
    public AudioSource testAudio1;
    public AudioSource testAudio2;
    public MeshRenderer testswap;
    public Text evil;
    public Text good;
    public Text taskDisplay;
    GameObject greenCube;
    public GameObject TEMPNEWOBJ;

    List<GameObject> MyObjects = new List<GameObject>();
    myInfo objectInfo; //info on the object from MyObjects[0]

    public Text WorldLabel;
    public bool inSallysRoom;
    public Text task;
    public string currentRequestor;

    List<myInfo> AllObjsWithInfo = new List<myInfo>();  //reference list of all the "MyInfo" scripts in the scene

    //Public Tasks
    int taskNum = 1;

   

    // Use this for initialization
    void Start() {
        currentRequestor = "Sally";

        for (int i = 0; i < FindObjectsOfType<myInfo>().Length; i++) {  //populate the list of "MyInfo" scripts
            AllObjsWithInfo.Add(FindObjectsOfType<myInfo>()[i]);
        }
        //Debug.Log(AllObjsWithInfo[0].gameObject.name);
    }

    void Update() {
        //Debug.Log("my obj" + MyObjects.Count);
        Cursor.lockState = CursorLockMode.Locked;
        
        ResetGame();    //Restart Scene on Escape Press/P press
        Movement();     //Move the player
        CastRay();      //Cast a ray from screen center into space
        LaunchStuff();  //push r to launch stuff in the direction of the camera
        Detach();       //push space to release stuff
        Combine();      //check combining objects
    }

    public void cleanCam() {
        //safely eject items from the camera load
        //deleting
        foreach (GameObject go in MyObjects) {
            if (gameObject.name != "Player") {
                Debug.Log("im deleting:" + go.name);
                Destroy(go);
            }
        }
        MyObjects.Clear();
        cam.transform.DetachChildren();
    }

    public void nextTask() {
        //sets up next task
        taskNum++;
        taskDisplay.text = getTaskText();
        //assign the requestor character:
        switch (taskNum)
        {
            case 1:
                {
                    currentRequestor = "Sally";
                    break;
                }
            case 2:
                {
                    currentRequestor = "Bob";
                    break;
                }
            case 3:
                {
                    currentRequestor = "Petunia";
                    break;
                }
        }
    }

    string getTaskText() {
        switch (taskNum)
        {
            case 1:
                {
                    return "ITEM REQUEST: Sally - My honey, the executive, is coming over. BRING me something DIRTY to get me in the mood, but also CLEAN to keep it classy.";
                    break;
                }
            case 2:
                {
                    return "MAKE REQUEST: Bob - About to live tweet the fireworks show! MAKE me something TASTY and EXPLOSIVE to eat during the show.";
                    break;
                }
            case 3:
                {
                    return "FIND ME REQUEST: Petunia - I'm locked in the theatre watching a terrible film! Bring me a COMEDIC and DRAMATIC film that's better!";
                    break;
                }
        }
        return "";
    }

    void turnOffAllLabels() {
        //Text[] labels = WorldLabels.GetComponentsInChildren<Text>();
        //turn off all labels
        /* foreach(Text t in labels)
         {
             t.enabled = false;
         }*/
        WorldLabel.text = "";
    }

    bool checkMatchingTags(string key1, string key2) {
        //compare tags and return if the game objects grabbed match those tags.
        bool got1 = false;
        bool got2 = false;

        //check if one object is from each side
        //Debug.Log("ITS ME" + MyObjects.Count);

        foreach (GameObject g in MyObjects) {
            //Debug.Log("ITS ME" + g.name);
            //Debug.Log("ITS ME " + g.tag); //problem: i think its going through every childNOPE
            if (!got1 || !got2) {
                if (g.tag == key1) {
                    got1 = true;
                    Debug.Log("My tag is xxxxxx " + g.tag);

                }
                else {
                    Debug.Log("My tag is " + g.tag);
                }
            }
            if (!got2) {
                if (g.tag == key2) {
                    got2 = true;

                }
                else {
                    Debug.Log("My tag is " + g.tag);
                }
            }
        }

        if (got1 && got2) {
            return true;
        }
        else {
            return false;
        }

    }

    public void detachItems() {
        //remove items from sticky gaze
        Rigidbody[] items = cam.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody i in items) {
            i.useGravity = true;
            if (i.gameObject.GetComponent<myInfo>() != null) {
                i.gameObject.GetComponent<myInfo>().grabbed = false;
                i.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                i.gameObject.GetComponent<Rigidbody>().useGravity = true;
            }
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other.tag == "reset") {
            //Debug.Log("this worked");

            for (int i = 0; i < AllObjsWithInfo.Count; i++) {
                AllObjsWithInfo[i].gameObject.transform.position = AllObjsWithInfo[i].startPos;
                AllObjsWithInfo[i].gameObject.transform.rotation = AllObjsWithInfo[i].startRot;
            }
        }
    }

    void ResetGame() {
        //Restart Scene on Escape Press
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();

        }
        if (Input.GetKeyDown(KeyCode.P)) {
            SceneManager.LoadScene(0);
        }
    }

    void Movement() {
        float z = Input.GetAxis("Vertical") * Time.deltaTime;
        gameObject.transform.position += z * transform.forward * 2f;
        float x = Input.GetAxis("Horizontal") * Time.deltaTime;
        gameObject.transform.position += x * transform.right * 2f;

        this.gameObject.transform.position = new Vector3(gameObject.transform.position.x, 0.93f, gameObject.transform.position.z);

        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x, player.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 2f, player.transform.localEulerAngles.z);
        player.transform.localEulerAngles = new Vector3(player.transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * -2f, player.transform.localEulerAngles.y, player.transform.localEulerAngles.z);

        //cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x, cam.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 2f, cam.transform.localEulerAngles.z);
        //cam.transform.localEulerAngles = new Vector3(cam.transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * -2f, cam.transform.localEulerAngles.y, cam.transform.localEulerAngles.z);
    }

    void CastRay() {
        Ray ray = cam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 10f, Color.red);  //show the debug ray
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 3f)) {    //the 10f is the length the ray extends in distance 
            //A collision occured between the ray and a thing
            if (hit.collider != null && hit.collider != floor && hit.collider.gameObject != cam && Input.GetKeyDown(KeyCode.LeftShift)) {
                hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                hit.collider.transform.parent = cam.transform;
                MyObjects.Add(hit.collider.gameObject);
                if (hit.collider.GetComponent<myInfo>() != null) {
                    hit.collider.GetComponent<myInfo>().grabbed = true;
                }
                //hit.collider.transform.GetComponent<Rigidbody>().velocity = cam.transform.GetComponent<Rigidbody>().velocity;
            }
            else if (hit.collider != null && hit.collider != floor && hit.collider.gameObject != cam) {
                //display the label
                //showLabel();
                if (Input.GetKeyDown(KeyCode.Mouse0)) {
                    //click to release
                    if (hit.collider.tag == "combined") {
                        //this is a combined object, break it it
                    }
                }
                if (hit.collider.gameObject.GetComponent<myInfo>() != null) {
                    hit.collider.gameObject.GetComponent<myInfo>().watched = true;
                    if (hit.collider.gameObject.GetComponent<myInfo>().label != null) {
                        //Debug.Log("WOOO");

                        WorldLabel.enabled = true;
                        //if (hit.collider.gameObject.GetComponent<myInfo>().wrongCombine) {
                        //    WorldLabel.enabled = true;
                        //}
                        //else {
                        //    WorldLabel.enabled = false;
                        //}
                        WorldLabel.text = hit.collider.gameObject.GetComponent<myInfo>().label;
                    }
                }
            }
        }
        else {
            //didnt catch anything on ray
            turnOffAllLabels();
        }
    }

    void LaunchStuff() {
        if (Input.GetKey(KeyCode.R)) {
            Transform[] grabbed = cam.GetComponentsInChildren<Transform>();
            cam.transform.DetachChildren();
            detachItems();
            foreach (Transform t in grabbed) {
                if (t.GetComponent<Rigidbody>() != null) {
                    t.GetComponent<Rigidbody>().gameObject.GetComponent<myInfo>().grabbed = false;
                    t.GetComponent<Rigidbody>().AddRelativeForce(cam.transform.forward * 300f);
                    t.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }
    }

    void Detach() {
        if (Input.GetKey(KeyCode.Space)) {
            //this.transform.parent = null;
            detachItems();
            cam.transform.DetachChildren();
            //Remove from the MyObjects list
            MyObjects.Clear();
            //Debug.Log("YOU DOOD IT");
        }
    }

    void Combine() {
        //this is to combine objects with tags
        if (Input.GetKeyDown(KeyCode.Q)) {

            objectInfo = MyObjects[0].GetComponent<myInfo>();    //the label of the object we're referring to a lot here on out

            switch (taskNum) {
                case 1: {
                        //find clean and dirty
                        if (checkMatchingTags("clean", "dirty")) {
                            //success
                            Debug.Log("YOU COMBINED CORRECTLY");

                            //Remove old objects for new one
                            Vector3 pos = MyObjects[0].transform.position;//standardize this to be a uniform location infront of camera
                            GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);//move this to infront of camera

                            temp.name = "Clean and dirty item.";
                            temp.GetComponent<myInfo>().sallyObject = true;
                            detachItems();
                            cleanCam();
                            temp.GetComponent<Rigidbody>().useGravity = true;
                            temp.GetComponent<Rigidbody>().isKinematic = false;
                            temp.GetComponent<Rigidbody>().freezeRotation = true;
                            temp.GetComponent<Rigidbody>().angularDrag = 0f;
                            temp.GetComponent<Rigidbody>().mass = 1f;
                            //Show the item's label on the present's tag
                            Debug.Log(temp.transform.GetChild(0).name);
                            Debug.Log(temp.transform.GetChild(0).transform.GetChild(0).name);
                            TextMeshProUGUI tmpro = temp.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();//sorry this is because it defaults the text like three children down :(
                            tmpro.SetText(temp.name);

                            //Debug.Log(MyObjects.Count);
                        }
                        else {
                            Debug.Log("COMBO DIDN'T WORK");
                            if (objectInfo.wrongCombine == false) {
                                objectInfo.wrongCombine = true;
                                objectInfo.label += " (" + objectInfo.tag + ")";
                            }
                            this.gameObject.GetComponent<AudioSource>().Play();
                        }

                        break;
                    }
                case 2: {
                        Debug.Log("ENTERING CASE 2");
                        //find tasty and explosive
                        if (checkMatchingTags("tasty", "explosive")) {
                            //success
                            Debug.Log("YOU COMBINED CORRECTLY");

                            //Remove old objects for new one
                            Vector3 pos = MyObjects[0].transform.position;
                            GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);    //move this to infront of camera

                            temp.GetComponent<myInfo>().label = "Popcorn";
                            temp.name = "Popcorn";
                            temp.GetComponent<myInfo>().sallyObject = false;

                            detachItems();
                            cleanCam();
                            temp.GetComponent<Rigidbody>().useGravity = true;
                            temp.GetComponent<Rigidbody>().isKinematic = false;
                            temp.GetComponent<Rigidbody>().freezeRotation = true;
                            temp.GetComponent<Rigidbody>().angularDrag = 0f;
                            temp.GetComponent<Rigidbody>().mass = 1f;

                            //Show the item's label on the present's tag
                            Debug.Log(temp.transform.GetChild(0).name);
                            Debug.Log(temp.transform.GetChild(0).transform.GetChild(0).name);
                            TextMeshProUGUI tmpro = temp.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();//sorry this is because it defaults the text like three children down :(
                            tmpro.SetText(temp.name);
                        }
                        else {
                            Debug.Log("COMBO DIDN'T WORK");
                            if (objectInfo.wrongCombine == false) {
                                objectInfo.wrongCombine = true;
                                objectInfo.label += " (" + objectInfo.tag + ")";
                            }
                            this.gameObject.GetComponent<AudioSource>().Play();
                        }

                        break;
                    }
                case 3: {
                        //find comedic and dramatic
                        if (checkMatchingTags("comedic", "dramatic")) {
                            //success
                            Debug.Log("YOU COMBINED CORRECTLY");

                            //Remove old objects for new one
                            Vector3 pos = MyObjects[0].transform.position;
                            GameObject temp = Instantiate(TEMPNEWOBJ, transform.position + (transform.forward * 2), transform.rotation);//move this to infront of camera
                            temp.GetComponent<myInfo>().label = "Better Film";
                            temp.name = "BetterFilm";
                            temp.GetComponent<myInfo>().sallyObject = true;

                            detachItems();
                            cleanCam();
                            temp.GetComponent<Rigidbody>().useGravity = true;
                            temp.GetComponent<Rigidbody>().isKinematic = false;
                            temp.GetComponent<Rigidbody>().freezeRotation = true;
                            temp.GetComponent<Rigidbody>().angularDrag = 0f;
                            temp.GetComponent<Rigidbody>().mass = 1f;

                            //Show the item's label on the present's tag
                            Debug.Log(temp.transform.GetChild(0).name);
                            Debug.Log(temp.transform.GetChild(0).transform.GetChild(0).name);
                            TextMeshProUGUI tmpro = temp.transform.GetChild(0).transform.GetChild(0).GetComponent<TextMeshProUGUI>();//sorry this is because it defaults the text like three children down :(
                            tmpro.SetText(temp.name);
                        }
                        else {
                            Debug.Log("COMBO DIDN'T WORK");
                            if (objectInfo.wrongCombine == false) {
                                objectInfo.wrongCombine = true;
                                objectInfo.label += " (" + objectInfo.tag + ")";
                            }
                            this.gameObject.GetComponent<AudioSource>().Play();
                        }

                        break;
                    }
            }
        }
    }
}
