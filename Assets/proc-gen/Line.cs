using System;
using UnityEngine;

public class Line
{
    Orientation orientation;
    Vector2Int coords;

    public Line(Orientation orientation, Vector2Int coords)
    {
        this.Orientation = orientation;
        this.Coords = coords;
    }

    public Orientation Orientation { get => orientation; set => orientation = value; }
    public Vector2Int Coords { get => coords; set => coords = value; }



}

public enum Orientation
{
    Horizontal = 0,
    Vertical = 1,
}