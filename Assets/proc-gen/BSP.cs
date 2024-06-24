using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

using Random = UnityEngine.Random;

public class BSP
{
    RoomNode rootNode;

    public RoomNode root { get => rootNode; }

    public BSP(int width, int height)
    {
        this.rootNode = new RoomNode(new Vector2Int(0,0), new Vector2Int(width, height), null, 0);
    }

    public List<RoomNode> PrepareNodes(int maxIterations, int minRoomWdt, int minRoomLength)
    {
        Queue<RoomNode> graph = new Queue<RoomNode>();
        List<RoomNode> toReturn = new List<RoomNode>();
        graph.Enqueue(this.rootNode);
        toReturn.Add(this.rootNode);
        int iterations = 0;

        while (iterations < maxIterations && graph.Count > 0) 
        {
            iterations++;
            RoomNode currentNode = graph.Dequeue();
            if (currentNode.Width >= minRoomWdt*2 || currentNode.Length >= minRoomLength * 2)
            {
                SplitTheSpace(currentNode, toReturn, minRoomLength, minRoomWdt, graph);
            }
        }

        return toReturn;

    }

    private void SplitTheSpace(RoomNode currentNode, List<RoomNode> toReturn, 
                                int minRoomLength, int minRoomWdt, Queue<RoomNode> graph)
    {
        Line line = GetLineDivider(currentNode.BottomLeftCorner,
                                   currentNode.TopRightCorner,
                                   minRoomWdt,
                                   minRoomLength);

        RoomNode node1, node2;

        if (line.Orientation == Orientation.Horizontal)
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner,
                new Vector2Int(currentNode.TopRightCorner.x, line.Coords.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(currentNode.BottomLeftCorner.x, line.Coords.y),
                currentNode.TopRightCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        } else
        {
            node1 = new RoomNode(currentNode.BottomLeftCorner,
                new Vector2Int(line.Coords.x, currentNode.TopRightCorner.y),
                currentNode,
                currentNode.TreeLayerIndex + 1);
            node2 = new RoomNode(new Vector2Int(line.Coords.x, currentNode.BottomLeftCorner.y),
                currentNode.TopRightCorner,
                currentNode,
                currentNode.TreeLayerIndex + 1);
        }

        AddNewNodes(toReturn, graph, node1);
        AddNewNodes(toReturn, graph, node2);


    }

    private void AddNewNodes(List<RoomNode> toReturn, Queue<RoomNode> graph, RoomNode node)
    {
        toReturn.Add(node);
        graph.Enqueue(node);
    }

    private Line GetLineDivider(Vector2Int bottomLeftCorner,
                            Vector2Int topRightCorner,
                            int minRoomWdt,
                            int minRoomLength)
    {
        Orientation orientation;

        bool LengthStatus = (topRightCorner.y - bottomLeftCorner.y) >= 2 * minRoomLength;
        bool widthStatus = (topRightCorner.x - bottomLeftCorner.x) >= 2 * minRoomWdt;

        if (LengthStatus && widthStatus)
        {
            orientation = (Orientation)(Random.Range(0, 2));
        }else if (widthStatus)
        {
            orientation = Orientation.Vertical;
        }
        else
        {
            orientation = Orientation.Horizontal;
        }

        return new Line(orientation, GetCoordsForOrientation(orientation, bottomLeftCorner, topRightCorner, minRoomWdt, minRoomLength));

    }

    private Vector2Int GetCoordsForOrientation(Orientation orientation, Vector2Int bottomLeftCorner, Vector2Int topRightCorner, int minRoomWdt, int minRoomLength)
    {
        Vector2Int coords = Vector2Int.zero;
        if (orientation == Orientation.Horizontal)
        {
            coords = new Vector2Int(0, 
                Random.Range(
                (bottomLeftCorner.y + minRoomLength), 
                (topRightCorner.y - minRoomLength))
                );
        }
        else
        {
            coords = new Vector2Int(
                Random.Range(
                             (bottomLeftCorner.x + minRoomWdt),
                             (topRightCorner.x - minRoomWdt)), 
                0
                );
        }

        return coords;
    }
}