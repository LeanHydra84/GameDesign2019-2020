using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

/*
To do:
Picking up piano keys
Death scenario
Creating saves - serializable save class (saved to Application.persistentDataPath)
Bullet hell boss fights
Room randomization
//This bitch is NOT READY
public class saveData
{

    public void Load()
    {
        //Gets values from file
        Stream stream = new FileStream(Application.persistentDataPath + "//save.txt", FileMode.Open, FileAccess.Read);

    }

    public void Save()
    {
        //Saves values to file
        BinaryFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(Application.persistentDataPath + "//save.txt", FileMode.Create, FileAccess.Write);

        //formatter.Serialize(stream, ); //Maybe make a new serializable class that I can format?
        stream.Close();
    }

}
*/

//QUESTION: CAN I JUST SERIALIZE PlayerState??? -- Probably not
public static class PlayerState
{
    static IEnumerator healthDelay()
    {
        canLose = false;
        yield return new WaitForSeconds(0.5f);
        canLose = true;
    }
    
    static int keys = 0;
    static int health = 4;
    static bool canLose = true;
    public static int Health
    {
        get { return health; }
        set
        {
            if (canLose && (health > 0 && health < 4))
            {
                mainScript.instance.StartCoroutine(healthDelay());
                health = value;
            }
        }
    }
    
    public static int Keys
    {
		get { return keys; }
		set { if(value > keys || value == 0) keys = value; }
    }
    
}

public class mainScript : MonoBehaviour
{

    //Misc
    private Light flashlight;
    private Camera mainCam;
    private saveData data;

    //Mask
    public bool maskOn;
    private bool CR_mask;

    const float Off_intensity = 0.5f;
    const float On_intensity = 1f;

    //GUI
    public Texture2D heart;

    //Array of lights for the mask
    public Light[] lightArray;

    public static mainScript instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("room_lights");
        lightArray = new Light[gos.Length];
        for(int i=0; i<gos.Length; i++)
        {
            lightArray[i] = gos[i].GetComponent<Light>();
        }
        flashlight = GameObject.FindWithTag("flashlight").GetComponent<Light>();

        mainCam = Camera.main;
        maskOn = false;
        CR_mask = true;
    }

    IEnumerator mask(bool a)
    {
        CR_mask = false;
        yield return new WaitForSeconds(0);

        if (a)
        {
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("ghosts"));//Enables culling mask for ghosts
            maskOn = false;
        }
        else 
        {
            mainCam.cullingMask |= 1 << LayerMask.NameToLayer("ghosts"); //Enables culling mask for the ghosts
            maskOn = true;
        }

        for (int i = 0; i < lightArray.Length; i++) lightArray[i].intensity = maskOn ? On_intensity : Off_intensity;
        CR_mask = true;

    }

    void OnGUI()
    {
        for (int i = 0; i < PlayerState.Health; i++) //Cycles once for every point of health
        {
            //Creates rect positions at a const height (7/8ths of the screen height) and exactly 10 pixels apart -- (numbers will be changed)
            Rect imagePos = new Rect(i * 10, Screen.height * (7 / 8), 10, 10);
            GUI.DrawTexture(imagePos, heart, ScaleMode.ScaleToFit, true, 10.0f); //Images will scale to the rect size
        }
    }

    void OnTriggerStay(Collider col) //Has to be OnTriggerStay, OnTriggerEnter only gets called once
    {
        if (Input.GetKeyDown("pickup"))
        {
            if (col.gameObject.tag == "healthPickup")
            {
                PlayerState.Health += 1; //Picking up hearts
                Destroy(col.gameObject);
            }
            else if (col.gameObject.tag == "pianoKey")
            {
                PlayerState.Keys += 1;
		Destroy(col.gameObject);
            }
        }

    }


    void Update()
    {
        //Lose-Death condition
        if (PlayerState.Health <= 0)
        {
            //DEATH STATE CODE
            SceneManager.LoadScene("loseCondition"); //Very tenuous

        }

        //Flashlight toggle
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.enabled = !flashlight.enabled;
            flashlight.gameObject.GetComponent<AudioSource>().Play();
        }

        if (Input.GetKeyDown(KeyCode.M) && CR_mask)
            StartCoroutine(mask(maskOn));
    }
}
