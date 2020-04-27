using UnityEngine;

using System;
using System.Collections;

namespace Orbox.Utils
{
    [Flags]
    public enum TileNeighbors
    {
        None = 0,

        Top = 1 << 0,
        Bottom = 1 << 1,
        Left = 1 << 2,
        Right = 1 << 3,
        TopLeft = 1 << 4,
        TopRight = 1 << 5,
        BottomLeft = 1 << 6,
        BottomRight = 1 << 7
    }

    public static class GridHelper
    {
        public static TileNeighbors GetAllNeighbors()
        {
            var all = TileNeighbors.Top
                        | TileNeighbors.Bottom
                        | TileNeighbors.Left
                        | TileNeighbors.Right
                        | TileNeighbors.TopLeft
                        | TileNeighbors.TopRight
                        | TileNeighbors.BottomLeft
                        | TileNeighbors.BottomRight;

            return all;
        }

        public static TileNeighbors GetLeftNeighbors()
        {
            return TileNeighbors.Left | TileNeighbors.TopLeft | TileNeighbors.BottomLeft;
        }

        public static TileNeighbors GetRightNeighbors()
        {
            return TileNeighbors.Right | TileNeighbors.TopRight | TileNeighbors.BottomRight;
        }

        public static TileNeighbors GetTopNeighbors()
        {
            return TileNeighbors.Top | TileNeighbors.TopLeft | TileNeighbors.TopRight;
        }

        public static TileNeighbors GetBottomNeighbors()
        {
            return TileNeighbors.Bottom | TileNeighbors.BottomLeft | TileNeighbors.BottomRight;
        }

        //public static Vector2 GetCoordinate(TileNeighbors neighbors)
        //{

        //}
    }
}