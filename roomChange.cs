using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System.Linq;

public class roomChange : Monobehaviour {
    
    private Random rand = new Random();
    
    public GameObject[] arr; //Should be the same length as the locations Array
    public Transform[] locations; //These objects have both the location and rotation parameter for each potential spot in them
    
    void Start() {
        //Grabbing all objects with the right tag to be the transforms
        
        
        //Sorting and Instantiating
        arr = arr.OrderBy(x => Random.Range(0,arr.Length).ToArray();
        for(int i=0; i < locations.Length; i++)
            Instantiate(arr[i], locations[i].position, location[i].rotation);
            Destroy(locations[i].gameObject);
    }
    
}
