using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    private List<Node> _children;

    public List<Node> children { get => _children;}

    public bool visited { get; set; }
    public Vector2Int BottomLeftCorner { get; set; }
    public Vector2Int BottomRightCorner { get; set; }
    public Vector2Int TopRightCorner { get; set; }
    public Vector2Int TopLeftCorner { get; set; }

    public int TreeLayerIndex { get; set; }
    public Node parent;

    public Node(Node parentNode)
    {
        _children = new List<Node>();
        this.parent = parentNode;
        if (parentNode != null)
        {
            parentNode.AddChild(this);
        }
    }

    public void AddChild(Node node)
    {
        children.Add(node);
    }

    public void RemoveChild(Node node)
    {
        children.Remove(node);
    }

}