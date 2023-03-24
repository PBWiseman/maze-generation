using System;
using UnityEngine;

[RequireComponent(typeof(MazeConstructor))]           

public class GameController : MonoBehaviour
{
    private MazeConstructor constructor;

    void Awake()
    {
        constructor = GetComponent<MazeConstructor>();
    }
    
    void Start()
    {
    
    }
}