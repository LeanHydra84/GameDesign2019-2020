using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class roomChange : MonoBehaviour {
    
    private Random rand = new Random(); //This line does literally nothing but I feel like it's existence validates my programming skill
    
    public GameObject[] arr; //Should be the same length as the locations Array
    public Transform[] locations; //These objects have both the location and rotation parameter for each potential spot in them
    
    void Start() {
        //Grabbing all objects with the right tag to be the transforms
        GameObject[] go = GameObject.FindGameObjectsWithTag("roomArea");
		locations = new Transform[go.Length];
		for(int j=0; j < go.Length; j++) {
			locations[j] = go[j].transform;
		}
        //Sorting and Instantiating
        arr = arr.OrderBy(x => Random.Range(0,arr.Length)).ToArray();
        for(int i=0; i < locations.Length; i++) {
			Instantiate(arr[i], locations[i].position, locations[i].rotation);
            Destroy(locations[i].gameObject);
		}
    }
}
