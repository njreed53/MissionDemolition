using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum GameMode
{
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour
{
    static private MissionDemolition MD;

    [Header("Set in Inspector")]
    public Text         uitLevel;
    public Text         uitShots;
    public Text         uitButton;
    public Vector3      castlePos;
    public GameObject[] castles;

    [Header("Set Dynamically")]
    public int        level;
    public int        levelMax;
    public int        shotsTaken;
    public GameObject castle;
    public GameMode   mode = GameMode.idle;
    public string     showing = "Show Slingshot";
    public int        maxShots = 5;

    private void Start()
    {
        MD = this;

        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    private void Update() {
        UpdateGUI();

        if((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd;
            SwitchView("Show Both");
            Invoke("NextLevel", 2f);
        }
    }

    private void StartLevel()
    {
        if(castle != null) Destroy(castle);

        GameObject[] oldGos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject go in oldGos)
        {
            Destroy(go);
        }

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;

        SwitchView("Show Both");
        ProjectileLine.PL.Clear();

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;
    }

    private void NextLevel(){
        level++;
        if(level == levelMax) level = 0;
        maxShots++;
        StartLevel();
    }

    public void SwitchView(string eView = "")
    {
        if(eView == "") eView = uitButton.text;

        showing = eView;
        switch(showing)
        {
            case "Show Slingshot":
                FollowCam.POI = null;
                uitButton.text = "Show Castle";
                break;
            
            case "Show Castle":
                FollowCam.POI = MD.castle;
                uitButton.text = "Show Both";
                break;

            case "Show Both":
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot";
                break;
        }
    }

    public static void ShotFired()
    {
        MD.shotsTaken++;

        if(MD.shotsTaken >= MD.maxShots)
        {
        FindObjectOfType<GameManager>().GameOver();
        }
    }

    private void UpdateGUI()
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        //uitShots.text = "Shots Taken: " + shotsTaken;
        uitShots.text = "Shots Left: " + (maxShots - shotsTaken);
    }
}