using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public static class transDat
{
	static transDat() 
	{
		keys = 0;
		health = 4;
		x = 0;
		y = 0;
		z = 0;
	}
	
	static int keys;
	static float health;
	static int seconds;
	
	static float x;
	static float y;
	static float z;
}

public class menu : MonoBehaviour 
{
	static bool newGame;
	BinaryFormatter formatter = new BinaryFormatter();
	
	void OnGUI() 
	{
		if(GUI.Button(new Rect(100, 100, 100, 100), "New Game"))
		{
			newGame = true;
			SceneManager.LoadScene("main");
		}
		
		if(File.Exists(Application.persistentDataPath + "//save.txt"))
		{
			if(GUI.Button(new Rect(100, 200, 100, 100), "Continue")) 
			{
				newGame = false;
				Stream stream = new FileStream(Application.persistentDataPath + "//save.txt", FileMode.Open, FileAccess.Read);
				var saveObj = formatter.Deserialize(stream);
				
				transDat.keys = saveObj.keys
				transDat.health = saveObj.health;
				transDat.seconds = saveObj.seconds;
				transDat.x = saveObj.x;
				transDat.y = saveObj.y;
				transDat.z = saveObj.z;
				stream.Close();
				
				SceneManager.LoadScene("main");
			}
		}
		
	}
}
