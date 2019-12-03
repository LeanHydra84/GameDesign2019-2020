using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{
    float beginTime;
    public int damage;
    public int bounceCount = 0;
    public bool isShrap;
    public bool pickupAble;

    private void Start()
    {
        beginTime = Time.time;
    }

    private void Update()
    {
        if (Time.time - beginTime > 2.5f)
        {
            Destroy(gameObject);
        }

    }

    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            PlayerState.Health -= damage;
            Destroy(gameObject);
        }
        if(col.gameObject.tag != "boss" && col.gameObject.tag != "Player")
            if(isShrap)
            {
                bossFight.RingAttack(transform, 2, transform.rotation);
            }

        if (bounceCount == 0 && Time.time - beginTime > 0.1f)
            Destroy(gameObject);
        else bounceCount--;
    }
}
