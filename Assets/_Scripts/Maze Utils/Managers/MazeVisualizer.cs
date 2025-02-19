using System.Collections.Generic;
using UnityEngine;

public class MazeVisualizer : SingletonMonoBehaviour<MazeVisualizer>
{
    
    [SerializeField] private MazeSprites sprites;
    [SerializeField] private GameObject visualPrefab;
    
    private readonly Dictionary<Vector2Int, SpriteRenderer> visualNodes = new Dictionary<Vector2Int, SpriteRenderer>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void CreateVisualNode(Vector2Int nodePosition)
    {
        Vector3 spawnPosition = new Vector3(nodePosition.x, nodePosition.y, 0);
        SpriteRenderer visualNode = Instantiate(visualPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<SpriteRenderer>();
        
        visualNode.sprite = sprites.noOpenSidesSprite;
        visualNodes.Add(nodePosition, visualNode);
    }

    public void CreateVisualConnection(MazeNode nodeFrom, MazeNode nodeTo)
    {
        ResetVisualNodeRotation(nodeFrom.Position);
        ResetVisualNodeRotation(nodeTo.Position);
        
        int numberOrConnections = nodeFrom.GetNumberOfConnections();
        SelectNewSprite(nodeFrom, numberOrConnections);
        
        numberOrConnections = nodeTo.GetNumberOfConnections();
        SelectNewSprite(nodeTo, numberOrConnections);
    }

    private void ResetVisualNodeRotation(Vector2Int nodePosition)
    {
        UpdateSpriteRotation(nodePosition, 0);
    }
    
    private void SelectNewSprite(MazeNode node, int numberOrConnections)
    {
        if (numberOrConnections == 4)
            ChangeSprite(node.Position, sprites.allOpenSidesSprite);

        else if (numberOrConnections == 3 || numberOrConnections == 1)
            ChangeSpriteToOneOpeningOrThree(node, numberOrConnections == 1);
        
        else if (numberOrConnections == 2)
            ChangeSpriteToTwoOpenings(node);
    }
    
    private void ChangeSprite(Vector2Int nodePosition, Sprite newSprite, int rotation = 0)
    {
        visualNodes[nodePosition].sprite = newSprite;
        UpdateSpriteRotation(nodePosition, rotation);
    }
    
    private void ChangeSpriteToOneOpeningOrThree(MazeNode node, bool oneOpening)
    {
        Sprite newSprite = oneOpening ? sprites.oneOpenSideSprite : sprites.oneClosedSideSprite;

        for (int i = 0; i < node.Connections.Length; i++)
        {
            if (node.Connections[i] == oneOpening)
            {
                ChangeSprite(node.Position, newSprite, i);
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
                
                ChangeSprite(node.Position, newSprite, correctRotation);
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
    
    private void UpdateSpriteRotation(Vector2Int nodePosition, int rotation)
    {
        visualNodes[nodePosition].transform.rotation = Quaternion.Euler(0, 0, -90f * rotation);
    }



    public void ResetVisualNode(Vector2Int nodePosition)
    {
        ChangeSprite(nodePosition, sprites.noOpenSidesSprite);
    }
}
