using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    float beginTime;
    public int damage;
    Rigidbody rb;

    private void Start()
    {
        beginTime = Time.time;
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (Time.time - beginTime > 2.5f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            PlayerState.Health -= damage;
        }
        if(col.gameObject.tag != "boss")
            Destroy(gameObject);
    }

}
