using UnityEngine;

public class RoomNode : Node
{
    public RoomNode(
        Vector2Int bottomLeftCorner,
        Vector2Int topRightCorner,
        Node parent,
        int index
    ) : base(parent)
    {
        this.BottomLeftCorner = bottomLeftCorner;
        this.TopRightCorner = topRightCorner;
        this.BottomRightCorner = new Vector2Int(TopRightCorner.x, bottomLeftCorner.y);
        this.TopLeftCorner = new Vector2Int(bottomLeftCorner.x, topRightCorner.y);
        this.TreeLayerIndex = index;
    }

    public int Width { get => (int)(TopRightCorner.x - BottomLeftCorner.x); }
    public int Length {get => (int)(TopRightCorner.y - BottomLeftCorner.y); }
}