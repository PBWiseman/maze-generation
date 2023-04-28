using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    private MazeConstructor constructor;
    [SerializeField] private int rows;
    [SerializeField] private int cols;

    public GameObject playerPrefab;
    public GameObject monsterPrefab;

    private AIController aIController;

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
        aIController = GetComponent<AIController>(); 
    }
    
    void Start()
    {
        constructor.GenerateNewMaze(rows, cols, OnTreasureTrigger);        
        aIController.Graph = constructor.graph;
        aIController.Player = CreatePlayer();
        aIController.Monster = CreateMonster(); 
        aIController.HallWidth = constructor.hallWidth;         
        aIController.StartAI();
    }

    void Update()
    {
        if(Input.GetKeyDown("f"))
        {
            int playerCol = (int)Mathf.Round(aIController.Player.transform.position.x / aIController.HallWidth);
            int playerRow = (int)Mathf.Round(aIController.Player.transform.position.z / aIController.HallWidth);
            List<Node> path = aIController.FindPath(playerRow, playerCol, constructor.goalRow, constructor.goalCol);
            constructor.DisposeOfSpheres();
            foreach(Node node in path)
            {
                constructor.PlaceSphere(node);
            }
        }
    }

    private GameObject CreatePlayer()
    {
        Vector3 playerStartPosition = new Vector3(constructor.hallWidth, 1, constructor.hallWidth);  
        GameObject player = Instantiate(playerPrefab, playerStartPosition, Quaternion.identity);
        player.tag = "Generated";
        return player;
    }

    private GameObject CreateMonster()
    {
        Vector3 monsterPosition = new Vector3(constructor.goalCol * constructor.hallWidth, 0f, constructor.goalRow * constructor.hallWidth);
        GameObject monster = Instantiate(monsterPrefab, monsterPosition, Quaternion.identity);
        monster.tag = "Generated";
        TriggerEventRouter mc = monster.AddComponent<TriggerEventRouter>();
        mc.callback = OnMonsterTrigger;   
        return monster;
    }

    private void OnTreasureTrigger(GameObject trigger, GameObject other)
    { 
        Debug.Log("You Won!");
        aIController.StopAI();
    }

    private void OnMonsterTrigger(GameObject trigger, GameObject other)
    {
        Debug.Log("Gotcha!");
        Start();
    }
}