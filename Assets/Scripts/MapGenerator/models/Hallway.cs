using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapGeneration3.models
{   
    internal class Hallway
    {
        private int length;
        private int xStart, yStart, xEnd, yEnd;
        private HallwayType hallwayType;
        private int roomFrom, roomTo;


        public int Length { get { return length; } set { length = value; } } 
        public int XStart { get { return xStart; } set { xStart = value; } }
        public int YStart { get { return yStart; } set { yStart = value; } }
        public int XEnd { get { return xEnd; } set { xEnd = value; } }
        public int YEnd { get { return yEnd; } set { yEnd = value; } }
        public int RoomFrom { get { return roomFrom; } set { roomFrom = value; } }
        public int RoomTo { get { return roomTo; } set { roomTo = value; } }

        public Hallway(int xStart, int yStart, int xEnd, int yEnd, int roomFrom, int roomTo) {
            this.xStart = xStart;
            this.yStart = yStart;
            this.xEnd = xEnd;
            this.yEnd = yEnd;

            if (xStart - xEnd != 0)
            {
                hallwayType = HallwayType.VERTICAL;
                length = xEnd - xStart;
            }
            else 
            {
                hallwayType = HallwayType.HORIZONTAL;
                length = yEnd - yStart;
            }

            this.roomFrom = roomFrom;
            this.roomTo = roomTo;
        
        }
    }
}