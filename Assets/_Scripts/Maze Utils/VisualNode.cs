using UnityEngine;

public class VisualNode : MonoBehaviour
{
    
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    #region METHODS
    
    public void UpdateSpriteRotation(int rotation)
    {
        transform.rotation = Quaternion.Euler(0, 0, -90f * rotation);
    }

    public void ResetSpriteRotation()
    {
        UpdateSpriteRotation(0);
    }

    public void SetSprite(Sprite newSprite, int newRotation = 0)
    {
        spriteRenderer.sprite = newSprite;
        UpdateSpriteRotation(newRotation);
    }

    public void SetTint(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
    
    #endregion
    
}
