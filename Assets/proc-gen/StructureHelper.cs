using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class StructureHelper
{
    public static List<Node> TraverseGraphToExtractLowestLeaves(RoomNode parentNode)
    {
        Queue<Node> nodesToCheck = new Queue<Node>();
        List<Node> toReturn = new List<Node>();

        if (parentNode.children.Count == 0)
        {
            return new List<Node> { parentNode };
        }
        foreach (var child in parentNode.children)
        {
            nodesToCheck.Enqueue(child);
        }

        while (nodesToCheck.Count > 0)
        {
            var currentNode = nodesToCheck.Dequeue();
            if (currentNode.children.Count == 0)
            {
                toReturn.Add(currentNode);
            }
            else
            {
                foreach (var child in currentNode.children)
                {
                    nodesToCheck.Enqueue(child);
                }
            }


        }

        return toReturn;

    }


    internal static Vector2Int GenerateBottomLeftCornerBetween(Vector2Int boundaryLeftPoint, 
                                                               Vector2Int boundaryRightPoint, 
                                                               float pointModifier, 
                                                               int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(Random.Range(minX, (int)(minX + (maxX - minX) * pointModifier)),
                              Random.Range(minY, (int)(minY + (minY - minY) * pointModifier)) //!!!!!

            );
    }

    internal static Vector2Int GenerateTopRightCornerBetween(Vector2Int boundaryLeftPoint,
                                                           Vector2Int boundaryRightPoint,
                                                           float pointModifier,
                                                           int offset)
    {
        int minX = boundaryLeftPoint.x + offset;
        int maxX = boundaryRightPoint.x - offset;
        int minY = boundaryLeftPoint.y + offset;
        int maxY = boundaryRightPoint.y - offset;

        return new Vector2Int(
            Random.Range((int)(minX + (maxX - minX) * pointModifier), maxX),
            Random.Range((int)(minY + (maxY - minY) * pointModifier), maxY)
            );
    }
}





            