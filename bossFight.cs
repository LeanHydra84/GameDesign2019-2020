using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossFight : MonoBehaviour
{
    private Transform player;
    private float smoothness = 10;
    private Vector3 targetPos;
    private Quaternion smoothedRot;
    float OldTime;
    public Rigidbody projectile;

    public Transform fire;

    void Start()
    {
        OldTime = Time.time;
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if(Time.time - OldTime > .1f)
        {
            OldTime = Time.time;
            Rigidbody proj = Instantiate(projectile, fire.position, transform.rotation);
            //proj.transform.LookAt(player);
            proj.transform.rotation = smoothedRot;
            proj.GetComponent<projectileScript>().damage = 1;
            proj.AddForce(transform.forward * 50, ForceMode.Impulse);
        }
    }

    void LateUpdate()
    {
        targetPos = player.position - transform.position;
        targetPos.y = 0;
        smoothedRot = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, smoothedRot, smoothness * Time.deltaTime);
    }

}
