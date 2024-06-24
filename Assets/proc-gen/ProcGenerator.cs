using System;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline;
using UnityEngine;

public class ProcGenerator
{

    List<RoomNode> allNodes = new List<RoomNode>();

    private int width, height;

    public ProcGenerator(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public List<Node> Calculate(int maxIterations, int minRoomWdt, int minRoomLength, float roomBottomCornerMod, float roomTopCornerMod, int roomOffset)
    {
        //BSP alg
        BSP bsp = new BSP(width, height);
        allNodes = bsp.PrepareNodes(maxIterations, minRoomWdt, minRoomLength);

        List<Node> roomSpaces = StructureHelper.TraverseGraphToExtractLowestLeaves(bsp.root);

        RoomGenerator roomGenerator = new RoomGenerator(maxIterations, minRoomLength, minRoomWdt);

        List<RoomNode> roomList = roomGenerator.GenerateRoomsInGivenSpace(roomSpaces, roomBottomCornerMod, roomTopCornerMod, roomOffset);

        return new List<Node>(roomList);
    }

}