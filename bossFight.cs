using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

public class bossFight : MonoBehaviour
{
    public float BPM;
    private bool isPlaying;
    private Transform player;
    private float smoothness = 10;
    private Vector3 targetPos;
    private Quaternion smoothedRot;
    public Rigidbody projectile;
    public Transform fire;
    string[] lines;

    void Start()
    {
        var patt = Resources.Load<TextAsset>(@"Songs/test");
        isPlaying = true;
        lines = Regex.Split(patt.text, "\n|\r|\r\n");
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(IterateFile());
    }

    void Update()
    {
        //
    }

    void Attack(string att)
    {
        /*
         Attacks List:
         '-': Standard, shoots single bullet at you (sprayable)
         '+': Shotgun, fires three offset bullets at you
         '/': Ring of Fire, launches a ring around the entire map
         '$': Shrapnel, single shot that collides with wall and does a miniature "Ring of Fire"
         '!': Ricochet, shot bounces on collision ONCE
         '@': IMPORTANTE, similar to single shot, but can be picked up and used for ammo        
         */       

        //Standard
        if(att == "-")
        {
            Rigidbody proj = Instantiate(projectile, fire.position, transform.rotation);
            proj.transform.rotation = smoothedRot;
            proj.GetComponent<projectileScript>().damage = 1;
            proj.AddForce(transform.forward * 30, ForceMode.Impulse);
        }
        //Shotgun
        else if(att == "+")
        {
            /*
            Quaternion adjustedRot = new Quaternion();
            for (int i=0; i < 1; i++)
            {
                Rigidbody shotProj = Instantiate(projectile, fire.position, transform.rotation); //Wow this for loop won't work for what I want

                if(i==1)
                {
                    adjustedRot = smoothedRot * Quaternion.Euler(Vector3.up * 15);
                } 
                else if(i==2)
                {
                    adjustedRot = smoothedRot * Quaternion.Euler(Vector3.up * -15);
                }


                shotProj.transform.rotation = adjustedRot;
                shotProj.GetComponent<projectileScript>().damage = 1;
                shotProj.AddForce(transform.forward * 30, ForceMode.Impulse);

            }
        */
            Rigidbody shotProj = Instantiate(projectile, fire.position, transform.rotation);
            Rigidbody shotProj1 = Instantiate(projectile, fire.position, transform.rotation * Quaternion.Euler(Vector3.up * 15));
            Rigidbody shotProj2 = Instantiate(projectile, fire.position, transform.rotation * Quaternion.Euler(Vector3.up * -15));

            shotProj.AddForce(shotProj.transform.forward * 30, ForceMode.Impulse);
            shotProj1.AddForce(shotProj1.transform.forward * 30, ForceMode.Impulse);
            shotProj2.AddForce(shotProj2.transform.forward * 30, ForceMode.Impulse);



        }
        //Ring of Fire
        else if (att == "/")
        {

        }
        //Shrapnel
        else if (att == "$")
        {

        }
        //Ricochet
        else if (att == "!")
        {

        }
        //Importante
        else if (att == "@")
        {

        }
    }

    IEnumerator IterateFile()
    {
        BPM = float.Parse(lines[0]);
        float multiplier = 1/(BPM / 60);
        Debug.Log("BPM: " + BPM);
        while(isPlaying)
        {
            for(int i=1; i < lines.Length; i++)
            {

                if(float.TryParse(lines[i], out float waitBeats))
                {
                    waitBeats *= multiplier;
                    yield return new WaitForSeconds(waitBeats);
                } 
                else
                {
                    Attack(lines[i]);
                    yield return 0;
                }

                /*
                Debug.Log(lines[i]);
                if (lines[i] == "-")
                {
                    Rigidbody proj = Instantiate(projectile, fire.position, transform.rotation);
                    proj.transform.rotation = smoothedRot;
                    proj.GetComponent<projectileScript>().damage = 1;
                    proj.AddForce(transform.forward * 30, ForceMode.Impulse);

                }
                else
                {
                    float waitBeats = float.Parse(lines[i]);
                    waitBeats *= multiplier;
                    yield return new WaitForSeconds(waitBeats);
                                       
                } */

            }

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
