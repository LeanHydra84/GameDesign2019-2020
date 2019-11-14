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

    IEnumerator IterateFile()
    {
        BPM = float.Parse(lines[0]);
        float multiplier = 1/(BPM / 60);
        Debug.Log("BPM: " + BPM);
        while(isPlaying)
        {
            for(int i=1; i < lines.Length; i++)
            {
                Debug.Log(lines[i]);
                if (lines[i] == "-")
                {
                    Rigidbody proj = Instantiate(projectile, fire.position, transform.rotation);
                    proj.transform.rotation = smoothedRot;
                    proj.GetComponent<projectileScript>().damage = 1;
                    proj.AddForce(transform.forward * 30, ForceMode.Impulse);
                    yield return 0;
                }
                else
                {
                    float waitBeats = float.Parse(lines[i]);
                    waitBeats *= multiplier;
                    yield return new WaitForSeconds(waitBeats);
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
