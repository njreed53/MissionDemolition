using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject prefabCloud;
    public int numClouds = 20;
    public Vector3 cloudPosMin = new Vector3(-50,-5,10);
    public Vector3 cloudPosMax = new Vector3(150,100,10);
    public Vector2 cloudScale = new Vector2(1,3);
    public float cloudSpeedMult = 1f;

    private GameObject[] clouds;

    private void Awake() 
    {
        clouds = new GameObject[numClouds];

        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;

        for(int i=0; i<numClouds; i++)
        {
            cloud = Instantiate<GameObject>(prefabCloud);

            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);

            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScale.x, cloudScale.y, scaleU);
            
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            cPos.z = 100- 90*scaleU;

            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform);
            
            clouds[i] = cloud;
        }   
    }

    private void Update() 
    {
        foreach (GameObject cloud in clouds)
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;

            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            if(cPos.x <= cloudPosMin.x) cPos.x = cloudPosMax.x;
            
            cloud.transform.position = cPos;
        }
    }
}
