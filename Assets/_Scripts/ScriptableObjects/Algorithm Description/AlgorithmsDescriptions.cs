using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlgorithmsDescription", menuName = "Scriptable Objects/Algorithms Description")]
public class AlgorithmsDescription : ScriptableObject
{
    [SerializeField] public List<MazeAlgorithmDescription> Descriptions;
}

[Serializable]
public struct MazeAlgorithmDescription
{
    public string Title;
    
    [TextArea(5, 10)]
    public string AlgoDescription;
}
