using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class MazeVisualizer : SingletonMonoBehaviour<MazeVisualizer>
{
    
    [SerializeField] private MazeSprites sprites;
    [SerializeField] private GameObject visualPrefab;
    
    private readonly Dictionary<Vector2Int, VisualNode> visualNodes = new Dictionary<Vector2Int, VisualNode>();

    [Header("Visual Tints")]    
    [SerializeField] private Color defaultTint;
    [SerializeField] private Color startingNodeTint;
    [SerializeField] private Color visitedTint;
    [SerializeField] private Color finishedTint;
    
    protected override void Awake()
    {
        base.Awake();
    }

    public void CreateVisualNode(Vector2Int nodePosition)
    {
        Vector3 spawnPosition = new Vector3(nodePosition.x, nodePosition.y, 0);
        VisualNode visualNode = Instantiate(visualPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<VisualNode>();
        
        visualNode.SetSprite(sprites.noOpenSidesSprite);
        visualNodes.Add(nodePosition, visualNode);
    }

    public void CreateVisualConnection(MazeNode nodeFrom, MazeNode nodeTo)
    {
        visualNodes[nodeFrom.Position].ResetSpriteRotation();
        visualNodes[nodeTo.Position].ResetSpriteRotation();
        
        int numberOrConnections = nodeFrom.GetNumberOfConnections();
        SelectNewSprite(nodeFrom, numberOrConnections);
        
        numberOrConnections = nodeTo.GetNumberOfConnections();
        SelectNewSprite(nodeTo, numberOrConnections);
    }
    
    private void SelectNewSprite(MazeNode node, int numberOfOpennings)
    {
        if (numberOfOpennings == 4)
            ChangeSprite(node.Position, sprites.allOpenSidesSprite);

        else if (numberOfOpennings == 3 || numberOfOpennings == 1)
            ChangeSpriteToOneOpeningOrThree(node, numberOfOpennings == 1);
        
        else if (numberOfOpennings == 2)
            ChangeSpriteToTwoOpenings(node);
    }
    
    private void ChangeSprite(Vector2Int nodePosition, Sprite newSprite, int rotation = 0)
    {
        visualNodes[nodePosition].SetSprite(newSprite);
        visualNodes[nodePosition].UpdateSpriteRotation(rotation);
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
}
