using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    [Header("Set in Inspector")]
    public GameObject prefabProjectile;
    public float      velocityMult = 8f;
    public LineRenderer leftLineRenderer;
    public LineRenderer rightLineRenderer;
    public Transform leftPost;
    public Transform rightPost;
    public AudioSource fireSound;
    public LineRenderer trajectoryLine;
    public int trajectoryPoints = 5;
    public float timeStep = 0.1f;


    [Header("Set Dynamically")]
    public GameObject launchPoint;
    public Vector3    launchPos;
    public GameObject projectile;
    private Rigidbody projectileRigidbody;
    public bool       aimingMode;

    private void Awake() 
    {   
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void Start()
    {
        trajectoryLine.positionCount = trajectoryPoints;
        
        leftLineRenderer.startWidth = 0.2f;
        leftLineRenderer.endWidth = 0.2f;
        rightLineRenderer.startWidth = 0.2f;
        rightLineRenderer.endWidth = 0.2f;

        leftLineRenderer.positionCount = 2;
        rightLineRenderer.positionCount = 2;

        leftLineRenderer.SetPosition(0, leftPost.transform.position);
        //leftLineRenderer.SetPosition(1, launchPoint.transform.position);

        rightLineRenderer.SetPosition(0, rightPost.transform.position);
        //rightLineRenderer.SetPosition(1, launchPoint.transform.position);

        leftLineRenderer.SetPosition(1, new Vector3(-10.2f, -5.8f, 0f));
        rightLineRenderer.SetPosition(1, new Vector3(-10.2f, -5.9f, -1f));
    }
    private void Update() 
    {
        // Draw slingshot band
        // If we are aiming and there is a projectile
        if ((projectile != null) && aimingMode)
        {
            leftLineRenderer.SetPosition(1, projectile.transform.position);
            rightLineRenderer.SetPosition(1, projectile.transform.position);
        }
        else
        {
            // At rest, draw the bands from slingshot points to launch point
            leftLineRenderer.SetPosition(1, new Vector3(-10.2f, -5.8f, 0f));
            rightLineRenderer.SetPosition(1, new Vector3(-10.2f, -5.9f, -1f));
        }
        if(!aimingMode) return;

        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos;

        float maxMagnitude = GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if(Input.GetMouseButtonUp(0))
        {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired();
            fireSound.Play();
            ProjectileLine.PL.poi = projectile;
            
        }

    }

    void DrawTrajectory(Vector3 startPos, Vector3 velocity)
    {
        trajectoryLine.positionCount = 5;

        for(int i=0; i<5; i++)
        {
            float t=i*timeStep;
            Vector3 point = startPos + velocity*t + 0.5f*Physics.gravity*t*t;
            trajectoryLine.SetPosition(i,point);
        }
    }
    private void OnMouseDrag()
    {
        Vector3 velocity = (launchPos - projectile.transform.position) * 10f;
        DrawTrajectory(projectile.transform.position, velocity);
    }
    private void OnMouseEnter() 
    {
        launchPoint.SetActive(true);
    }

    private void OnMouseExit() 
    {
        launchPoint.SetActive(false);
    }

    private void OnMouseDown() 
    {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    static public Vector3 LAUNCH_POS 
    {
        get
        {
            if(S == null) return Vector3.zero;
            else return S.launchPos;
        }
    }
}
