using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGeneration3.models
{  
    internal class Room
    {
        private int height, width;
        private int xStart, yStart, xEnd, yEnd;
        private TileType roomType;
        private int hallwayNumber;
        private int roomId;
        private Boolean hUp, hDown, hLeft, hRight;


        public int Height { get { return height; } set { height = value; } }
        public int Width { get { return width; } set { width = value; } }
        public int XStart { get { return xStart; } set { xStart = value; } }
        public int YStart { get { return yStart; } set { yStart = value; } }
        public int XEnd { get { return xEnd; } set { xEnd = value; } }
        public int YEnd { get { return yEnd; } set { yEnd = value; } }
        public TileType RoomType { get { return roomType; } set { roomType = value; } }
        public int HallwayNumber { get { return hallwayNumber; } set { hallwayNumber = value;  } }
        public int RoomId { get { return roomId; } set { roomId = value; } }
        public Boolean HUp { get { return hUp; } set { hUp = value; } }
        public Boolean HDown { get { return hDown; } set { hDown = value; } }
        public Boolean HLeft { get { return hLeft; } set { hLeft = value; } }
        public Boolean HRight { get { return hRight; } set { hRight = value; } }

        public Room(int xStart, int yStart, int xEnd, int yEnd, TileType roomType, int roomId) { 
            this.xStart = xStart;
            this.yStart = yStart;
            this.XEnd = xEnd;
            this.YEnd = yEnd;

            height = yEnd - yStart;
            width = xEnd - xStart;

            this.roomType = roomType;

            this.roomId = roomId;

        }
    }
}
