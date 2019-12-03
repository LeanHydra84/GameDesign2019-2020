using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;

/*
Attacks List:
    '-': Standard, shoots single bullet at you (sprayable)
    '+': Shotgun, fires three offset bullets at you
    '/': Ring of Fire, launches a ring around the entire map
    '$': Shrapnel, single shot that collides with wall and does a miniature "Ring of Fire"
    '!': Ricochet, shot bounces on collision ONCE
    '@': IMPORTANTE, similar to single shot, but can be picked up and used for ammo        
*/

public class bossFight : MonoBehaviour
{
    public float BPM;
    private bool isPlaying;
    private Transform player;
    private float smoothness = 10;
    private Vector3 targetPos;
    private Quaternion smoothedRot;
    public Rigidbody projE;
    public static Rigidbody projectile;
    public Transform fire;
    string[] lines;
    public static int[] anglesX;
    int[] SG_angles = new[] { 0, 15, -15 };

    void Start()
    {
        projectile = projE;
        var patt = Resources.Load<TextAsset>(@"Songs/FileOut");
        isPlaying = true;
        lines = Regex.Split(patt.text, "\r\n ?|\n");
        foreach (string a in lines) Debug.Log(a);
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(IterateFile());
        anglesX = GetAngles();
    }

    int[] GetAngles()
    {
        int angleDifference = 15;
        int[] ang = new int[24];

        for (int i = 0; i < ang.Length; i++)
        {
            if (i != 0)
            {
                ang[24 - i] = 360 - angleDifference * i;
            }
            else
                ang[i] = 0;
        }

        return ang;
    }

    public static void RingAttack(Transform t, int seperator, Quaternion startingRot)
    {
        Quaternion adjustedRot = new Quaternion();
        for (int i = 0; i < anglesX.Length; i += seperator)
        {
            Rigidbody shotProj = Instantiate(projectile, t.position, startingRot);
            adjustedRot = startingRot * Quaternion.Euler(Vector3.up * anglesX[i]);
            shotProj.transform.rotation = adjustedRot;
            shotProj.GetComponent<projectileScript>().damage = 1;
            shotProj.AddForce(shotProj.transform.forward * 15, ForceMode.Impulse);
        }
    }

    void SingleAttack(Transform t, bool shrap, bool pickup, int bc)
    {
        Rigidbody proj = Instantiate(projectile, t.position, t.rotation);
        projectileScript ps = proj.GetComponent<projectileScript>();
        proj.transform.rotation = smoothedRot;
        ps.damage = 1;
        ps.isShrap = shrap;
        ps.bounceCount = bc;
        ps.pickupAble = pickup;
        proj.AddForce(transform.forward * 30, ForceMode.Impulse);
    }

    void ShotgunAttack()
    {
        Quaternion adjustedRot = new Quaternion();
        for (int i = 0; i < 3; i++)
        {
            Rigidbody shotProj = Instantiate(projectile, fire.position, transform.rotation);
            adjustedRot = smoothedRot * Quaternion.Euler(Vector3.up * SG_angles[i]);
            shotProj.transform.rotation = adjustedRot;
            shotProj.GetComponent<projectileScript>().damage = 1;
            shotProj.AddForce(shotProj.transform.forward * 30, ForceMode.Impulse);
        }
    }

    IEnumerator IterateFile()
    {
        BPM = float.Parse(lines[0]);
        float multiplier = 1 / (BPM / 60);
        Debug.Log("BPM: " + BPM);
        while (isPlaying)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                if (float.TryParse(lines[i], out float waitBeats))
                {
                    waitBeats *= multiplier;
                    //Debug.Log("Waiting at line: " + lines[i]);
                    yield return new WaitForSeconds(waitBeats);
                }
                else
                {
                    //Debug.Log("Switching at line: " + lines[i]);
                    switch (lines[i])
                    {
                        case "=":
                            SingleAttack(fire, false, false, 0);
                            break;
                        case "+":
                            ShotgunAttack();
                            break;
                        case "/":
                            RingAttack(transform, 1, smoothedRot);
                            break;
                        case "$":
                            SingleAttack(fire, true, false, 0);
                            break;
                        case "!":
                            SingleAttack(fire, false, false, 1);
                            break;
                        case "@":
                            SingleAttack(fire, false, true, 0);
                            break;
                        default:
                            Debug.Log($"Error at line {i}: Illegal input {lines[i]}.");
                            break;
                            //throw new System.Exception($"Error at line {i}: Illegal input {lines[i]}.");
                    }
                    yield return 0;
                }
                
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
