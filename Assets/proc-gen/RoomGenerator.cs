using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.UIElements;
using UnityEngine;

public class RoomGenerator
{
    private int maxIterations;
    private int minRoomLength;
    private int minRoomWdt;

    public RoomGenerator(int maxIterations, int minRoomLength, int minRoomWdt)
    {
        this.maxIterations = maxIterations;
        this.minRoomLength = minRoomLength;
        this.minRoomWdt = minRoomWdt;
    }

    public List<RoomNode> GenerateRoomsInGivenSpace(List<Node> roomSpaces, float roomBottomCornerMod, float roomTopCornerMod, int roomOffset)
    {
        List<RoomNode> toReturn = new List<RoomNode>();

        foreach (Node space in roomSpaces) 
        {
            Vector2Int newBottomLeftPoint = StructureHelper.GenerateBottomLeftCornerBetween(
                space.BottomLeftCorner, space.TopRightCorner, roomBottomCornerMod, roomOffset); 

            Vector2Int newTopRightPoint = StructureHelper.GenerateTopRightCornerBetween(
                space.BottomLeftCorner, space.TopRightCorner, roomTopCornerMod, roomOffset);
        
            space.BottomLeftCorner = newBottomLeftPoint;
            space.TopRightCorner = newTopRightPoint;
            space.BottomRightCorner = new Vector2Int(newTopRightPoint.x, newBottomLeftPoint.y);
            space.TopLeftCorner = new Vector2Int(newBottomLeftPoint.x, newTopRightPoint.y);

            toReturn.Add((RoomNode)space);

        }

        return toReturn;

    }
}