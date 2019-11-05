using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public static class PlayerState
{
    static IEnumerator healthDelay()
    {
        canLose = false;
        yield return new WaitForSeconds(0.5f);
        canLose = true;
    }

    static int keys;
    static int health;
	static float time;
    static int seconds;
    static bool canLose;

    static PlayerState()
    {
        keys = 0;
        health = 4;
        time = 0f;
		seconds = 0;
        canLose = true;
    }

    public static int Health
    {
        get { return health; }
        set
        {
            if (canLose && (value >= 0 && value < 5))
            {
                health = value;
                mainScript.instance.StartCoroutine(healthDelay());
            }
        }
    }

    public static int Keys
    {
        get { return keys; }
        set { if (value > keys || value == 0) keys = value; }
    }

    public static float Time
    {
        get { return time; }
        set 
		{ 
			if (value > time) time = value; 
			seconds = (int)time;
		}
    }
    
	public static int Seconds { get; }
	
    public static void Load() 
    {
		health = transDat.health;
		keys = transDat.keys;
		time = transDat.time;
		seconds = (int)time;
    }
	
	public static void Save()
	{
		transDat.keys = keys;
		transDat.health = health;
		transDat.time = time;
		
		transDat.x = instance.x;
		transDat.y = instance.y;
		transDat.z = instance.z;
		
		BinaryFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(Application.persistentDataPath + "//save.txt", FileMode.Create, FileAccess.Write);
		formatter.Serialize(stream, transDat);
		stream.Close();
	}

}

public class mainScript : MonoBehaviour
{

    //Misc
    private Light flashlight;
    public float timeScale;
    public Text time;
    public bool showSeconds;

    //Mask
    public bool maskOn;
    private bool CR_mask;

    public float intensityMult = 2f;

    //GUI
    public Texture2D heart;
    //Array of lights for the mask
    public Light[] lightArray;
    public static mainScript instance;
	
    private void Awake()
    {
        instance = this;
		if(!menu.newGame) 
		{
			PlayerState.Load();
			transform.position = new Vector3(transDat.x, transDat.y, transDat.z);
		}		
    }

    void Start()
    {
        Debug.Log(Screen.width + " x " + Screen.height);
        GameObject[] gos = GameObject.FindGameObjectsWithTag("room_lights");
        lightArray = new Light[gos.Length];
        for (int i = 0; i < gos.Length; i++) lightArray[i] = gos[i].GetComponent<Light>();
        flashlight = GameObject.FindWithTag("flashlight").GetComponent<Light>();

        maskOn = false;
        CR_mask = true;
    }


    IEnumerator mask(bool a)
    {
        CR_mask = false;
        yield return new WaitForSeconds(0.5f);

        if (a)
        {
            charCont.mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("ghosts"));//Enables culling mask for ghosts
            maskOn = false;
        }
        else
        {
            charCont.mainCam.cullingMask |= 1 << LayerMask.NameToLayer("ghosts"); //Enables culling mask for the ghosts
            maskOn = true;
        }

        for (int i = 0; i < lightArray.Length; i++)
            lightArray[i].intensity = maskOn ? (lightArray[i].intensity * intensityMult) : (lightArray[i].intensity / intensityMult);
        CR_mask = true;

    }

    void OnGUI()
    {
        for (int i = 0; i < PlayerState.Health; i++) //Cycles once for every point of health
        {
            //Creates rect positions at a const height (7/8ths of the screen height) and exactly 10 pixels apart -- (numbers will be changed)
            Rect imagePos = new Rect(32 * ((float)i + 0.8f), 25, 21, 21);
            GUI.DrawTexture(imagePos, heart, ScaleMode.StretchToFill, true, 10.0f); //Images will scale to the rect size: hahahaha
        }

        //Flashlight

        Event current = Event.current;
        Vector2 mousePos = new Vector2();

        mousePos.x = current.mousePosition.x;
        mousePos.y = charCont.mainCam.pixelHeight - current.mousePosition.y;
        int lmask = ~(1 << LayerMask.NameToLayer("triggers"));
        RaycastHit hit;
        Ray ray = charCont.mainCam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, charCont.mainCam.nearClipPlane));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lmask)) flashlight.transform.LookAt(hit.point);

        //flashlight.transform.eulerAngles = new Vector3(Mathf.Clamp(flashlight.transform.eulerAngles.x, -20f, 15f), flashlight.transform.eulerAngles.y, 0f);
        // ^ Clamps vertical rotation between two constants. Issue: Currently locks to one constant, doesn't go negative
    }

    string convertTime(int seconds)
    {
        float secs = seconds * timeScale;
        secs %= 24 * 3600;
        int hour = (int)(secs / 3600);
        secs %= 3600;
        int minutes = (int)(secs / 60);
        secs %= 60;

        string returnString = hour + 6 + ":" + minutes.ToString().PadLeft(2, '0');
        if (showSeconds) returnString += ':' + secs.ToString().PadLeft(2, '0');

        return returnString;
    }

    void OnTriggerStay(Collider col) //Has to be OnTriggerStay, OnTriggerEnter only gets called once
    {
        if (Input.GetKeyDown(KeyCode.F))
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
    	PlayerState.Seconds = (int)(PlayerState.Time + Time.deltaTime);
        time.text = convertTime(PlayerState.Seconds);

        //Lose-Death condition
        if (PlayerState.Health <= 0)
        {
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

        if (Input.GetKeyDown(KeyCode.Escape)
		{
			PlayerState.Save();
           		Application.Quit();
		}
    }

}
