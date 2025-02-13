using System.Collections.Generic;
using UnityEngine;

public class BaseSolvingAlgorithm
{
    
    public virtual List<MazeNode> Solve(Vector2 startPosition, Vector2 endPosition) => new List<MazeNode>();
    
}
