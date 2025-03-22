using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : SingletonMonoBehaviour<MazeVisualizer>
{
    
    #region VARIABLES
    
    [SerializeField] private MazeSprites sprites;
    [SerializeField] private GameObject visualPrefab;
    
    private readonly Dictionary<Vector2Int, VisualNode> visualNodes = new Dictionary<Vector2Int, VisualNode>();

    [Header("Visual Tints")]    
    [SerializeField] private Color defaultTint;
    [SerializeField] private Color startingNodeTint;
    [SerializeField] private Color visitedTint;
    [SerializeField] private Color finishedTint;
    
    #endregion

    
    #region METHODS
    public void CreateVisualNode(Vector2Int nodePosition)
    {
        Vector3 spawnPosition = new Vector3(nodePosition.x, nodePosition.y, 0);
        VisualNode visualNode = Instantiate(visualPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<VisualNode>();
        
        visualNode.SetSprite(sprites.noOpenSidesSprite);
        visualNodes.Add(nodePosition, visualNode);
    }

    public void CreateVisualConnection(MazeNode nodeFrom, MazeNode nodeTo)
    {
        SelectNewSprite(nodeFrom);
        SelectNewSprite(nodeTo);
    }
    
    private void SelectNewSprite(MazeNode node)
    {
        int numberOfOpenings = node.GetNumberOfConnections();
        
        if (numberOfOpenings == 4)
            ChangeSprite(node.Position, sprites.allOpenSidesSprite);

        else if (numberOfOpenings == 3 || numberOfOpenings == 1)
            ChangeSpriteToOneOpeningOrThree(node, numberOfOpenings == 1);
        
        else if (numberOfOpenings == 2)
            ChangeSpriteToTwoOpenings(node);
    }
    
    private void ChangeSprite(Vector2Int nodePosition, Sprite newSprite, int rotation = 0)
    {
        visualNodes[nodePosition].SetSprite(newSprite, rotation);
    }
    
    private void ChangeSpriteToOneOpeningOrThree(MazeNode node, bool oneOpening)
    {
        Sprite newSprite = oneOpening ? sprites.oneOpenSideSprite : sprites.oneClosedSideSprite;

        for (int i = 0; i < node.Connections.Length; i++)
        {
            if (node.Connections[i] == oneOpening)
            {
                visualNodes[node.Position].SetSprite(newSprite, i);
                break;
            }
        }
    }    
    
    private void ChangeSpriteToTwoOpenings(MazeNode node)
    {
        for (int i = 0; i < node.Connections.Length; i++)
        {
            if (node.Connections[i])
            {
                Sprite newSprite = SelectCornerSpriteOrTunnelSprite(node, i);
                int correctRotation = i;

                if (newSprite == sprites.cornerSprite)
                    correctRotation = AdjustSpriteRotation(node, i);
                
                visualNodes[node.Position].SetSprite(newSprite, correctRotation);
                break;
            }
        }
    }
    
    private Sprite SelectCornerSpriteOrTunnelSprite(MazeNode node, int i)
    {
        Sprite newSprite;

        if (node.Connections[i] == node.Connections[(i + 2) % 4])
            newSprite = sprites.tunnelSprite;
        else
            newSprite = sprites.cornerSprite;
        
        return newSprite;
    }
    
    private int AdjustSpriteRotation(MazeNode node, int currentRotation)
    {
        if (currentRotation == 0 && node.Connections[3])
            return -1;
        
        return currentRotation;
    }

    public void ResetVisualNode(Vector2Int nodePosition)
    {
        ChangeSprite(nodePosition, sprites.noOpenSidesSprite);
        SetVisualTintDefault(nodePosition);
    }

    public void SetVisualTintDefault(Vector2Int nodePosition)
    {
        SetVisualTint(nodePosition, defaultTint);
    }
    
    public void SetVisualTintFinished(Vector2Int nodePosition)
    {
        SetVisualTint(nodePosition, finishedTint);
    }

    public void SetVisualTintVisited(Vector2Int nodePosition)
    {
        SetVisualTint(nodePosition, visitedTint);
    }

    public void SetVisualTintCurrentNode(Vector2Int nodePosition)
    {
        SetVisualTint(nodePosition, startingNodeTint);
    }
    
    private void SetVisualTint(Vector2Int nodePosition, Color newTint)
    {
        visualNodes[nodePosition].SetTint(newTint);
    }
    
    #endregion
    
}
