using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class transDat
{
	public int keys;
	public int health;
	public float time;

    public float[] HoldPosition = new float[2];
	public float x;
    public float y;
    public float z;
}

public class menu : MonoBehaviour 
{

    static menu()
    {
        newGame = true;
    }

	public static bool newGame;

    public void ContinueGame()
    {
        newGame = false;
        SceneManager.LoadScene("Alright");
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    void OnGUI() 
	{
		if(GUI.Button(new Rect(100, 100, 100, 100), "New Game"))
		{
			newGame = true;
			SceneManager.LoadScene("Alright");
		}
		
		if(File.Exists(Application.persistentDataPath + "//save.txt"))
		{
			if(GUI.Button(new Rect(100, 200, 100, 100), "Continue")) 
			{
                ContinueGame();
			}
		}

        if (File.Exists(Application.persistentDataPath + "//save.txt"))
        {
            if(GUI.Button(new Rect(100,300,100,100), "Delete Save"))
            {
                File.Delete(Application.persistentDataPath + "//save.txt");
            }
        }


    }
}
