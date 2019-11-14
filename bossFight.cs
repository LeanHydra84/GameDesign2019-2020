using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class bossFight : MonoBehaviour
{
    public float BPM;
    private bool isPlaying;
    StreamReader file;
    private Transform player;
    private float smoothness = 10;
    private Vector3 targetPos;
    private Quaternion smoothedRot;
    public Rigidbody projectile;
    public StreamReader str;
    public Transform fire;

    void Start()
    {
        string path = @"";
        isPlaying = true;
        file = new StreamReader(path);
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine(IterateFile());
    }

    void Update()
    {
        //
    }

    IEnumerator IterateFile()
    {
        BPM = float.Parse(file.ReadLine());
        float multiplier = 1/(BPM / 60);
        Debug.Log("BPM: " + BPM);
        string line;
        while(isPlaying)
        {
            while ((line = file.ReadLine()) != null)
            {
                Debug.Log(line);
                if (line == "-")
                {
                    Rigidbody proj = Instantiate(projectile, fire.position, transform.rotation);
                    proj.transform.rotation = smoothedRot;
                    proj.GetComponent<projectileScript>().damage = 1;
                    proj.AddForce(transform.forward * 30, ForceMode.Impulse);
                    yield return 0;
                }
                else
                {
                    float waitBeats = float.Parse(line);
                    waitBeats *= multiplier;
                    yield return new WaitForSeconds(waitBeats);
                }

            }

            file.DiscardBufferedData();
            file.BaseStream.Seek(0, System.IO.SeekOrigin.Begin);
            file.ReadLine();
        }
        
        file.Close();
    }

    void LateUpdate()
    {
        targetPos = player.position - transform.position;
        targetPos.y = 0;
        smoothedRot = Quaternion.LookRotation(targetPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, smoothedRot, smoothness * Time.deltaTime);
    }

}
