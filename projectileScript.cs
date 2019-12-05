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

    public Sprite startingTexture;
    private SpriteRenderer itsAbigBattle;



    private void Start()
    {
        itsAbigBattle = transform.GetChild(0).GetComponent<SpriteRenderer>();
        beginTime = Time.time;
        if (startingTexture != null) itsAbigBattle.sprite = startingTexture;
    }

    private void Update()
    {
        itsAbigBattle.transform.LookAt(Camera.main.transform);
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

        if(col.gameObject.tag != "boss" && col.gameObject.tag != "Player" && isShrap)
        {
            bossFight.RingAttack(transform, 2, transform.rotation);
        }
            

        if (bounceCount == 0 && Time.time - beginTime > 0.1f)
            Destroy(gameObject);
        else bounceCount--;
    }
}
