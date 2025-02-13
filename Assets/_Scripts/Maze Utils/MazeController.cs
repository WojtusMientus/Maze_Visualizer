using System;
using System.Collections.Generic;
using UnityEngine;

public class MazeController : MonoBehaviour
{

    private Dictionary<Vector2, MazeNode> nodes = new Dictionary<Vector2, MazeNode>();

    [SerializeField] private int mazeSize = 10;
    
    
    private void Start()
    {
        
    }


    public void ClearMaze() { }


    private void OnValidate()
    {
        Debug.Log(mazeSize);
        
        // UpdateMazeSize();
    }

    private void UpdateMazeSize()
    {

    }

    private void ClearMAze()
    {
        
    }
}


