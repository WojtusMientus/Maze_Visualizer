using UnityEngine;

[CreateAssetMenu(fileName = "MazeSprites", menuName = "Scriptable Objects/MazeSprites")]
public class MazeSprites : ScriptableObject
{
    [Header("All or None")]
    public Sprite noOpenSidesSprite;
    public Sprite allOpenSidesSprite;

    [Header("One Open or One Closed")]
    public Sprite oneOpenSideSprite;
    public Sprite oneClosedSideSprite;

    [Header("2 Openings")]
    public Sprite cornerSprite;
    public Sprite tunnelSprite;
}
