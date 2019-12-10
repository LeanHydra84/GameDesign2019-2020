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
    private SpriteRenderer itsAbigBattle;
    private Sprite startingTexture;
    Camera mc;

    public Sprite StartingTexture
    {
        get { return startingTexture; } 
        set 
        {
            if(itsAbigBattle == null) itsAbigBattle = transform.GetChild(0).GetComponent<SpriteRenderer>();
            startingTexture = value;
            itsAbigBattle.sprite = startingTexture;
        }
    }





    private void Start()
    {
        mc = Camera.main;
        beginTime = Time.time;
    }

    private void Update()
    {
        itsAbigBattle.transform.LookAt(mc.transform);
        if (Time.time - beginTime > 6f)
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
        else if(bounceCount > 0)
        {
            bossFight.spriteNames.TryGetValue("Bounce" + bounceCount, out Sprite tempSprite);
            StartingTexture = tempSprite;
            bounceCount--; 
        }
    }
}
