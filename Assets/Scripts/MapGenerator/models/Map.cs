using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MapGeneration3.utils;

namespace MapGeneration3.models
{  
    internal class Map
    {
        public List<Room> rooms; 
        public List<Hallway> hallways;
        Boolean[][] adjacencyMatrix = new Boolean[Constants.MAX_ROOM_COUNT + 1][];
        Boolean goodGeneration = false;

        public TileType[][] map = new TileType[32][];

        public Map() {

            for (int i = 0; i < Constants.MAX_ROOM_COUNT; i++)
            {
                adjacencyMatrix[i] = new Boolean[Constants.MAX_ROOM_COUNT];
                for (int j = 0; j < Constants.MAX_ROOM_COUNT; j++)
                {
                    adjacencyMatrix[i][j] = false;
                }
            }

            while (!goodGeneration)
            {
                rooms = new List<Room>();
                hallways = new List<Hallway>();
                for (int i = 0; i < 32; i++)
                {
                    map[i] = new TileType[32];
                    for (int j = 0; j < 32; j++)
                    {
                        map[i][j] = TileType.NG;
                    }
                }
                for (int i = 0; i < Constants.MAX_ROOM_COUNT; i++) {
                    for (int j = 0; j < Constants.MAX_ROOM_COUNT; j++) {
                        adjacencyMatrix[i][j] = false;
                    }
                }
                generateRooms();
                generateHallways();
                goodGeneration = IsConnected();
            }
        }

        private void generateRooms() {

            generateRoom(TileType.SP, 0);

            for (int i = 1; i < Constants.MAX_ROOM_COUNT - 1; i++)
            {
                generateRoom(TileType.DR, i);
            }

            generateRoom(TileType.BF, Constants.MAX_ROOM_COUNT);


        }

        public Boolean checkValidPlace(int x1, int y1, int x2, int y2)
        {
            for (int i = y1 - 3; i < y2 + 3; i++)
            {
                for (int j = x1 - 3; j < x2 + 3; j++)
                {
                    if (map[i][j] != TileType.NG)
                        return true;
                }
            }
            return false;
        }

        public void generateRoom(TileType tileType, int id) {
            Boolean f = true;
            System.Random rnd = new System.Random();
            int x1 = 0, x2 = 0, y1 = 0, y2 = 0;
            while (f)
            {
                x1 = rnd.Next(1 + Constants.ROOM_MAX_WIDTH, 32 - Constants.ROOM_MAX_WIDTH - 3);
                y1 = rnd.Next(1 + Constants.ROOM_MAX_WIDTH, 32 - Constants.ROOM_MAX_WIDTH - 3);

                x2 = rnd.Next(x1 + Constants.ROOM_MIN_WIDTH, x1 + Constants.ROOM_MAX_WIDTH + 2);
                y2 = rnd.Next(y1 + Constants.ROOM_MIN_HEIGHT, y1 + Constants.ROOM_MAX_HEIGHT + 2);

                if (x1 == x2)
                {
                    x2 += Constants.ROOM_MAX_WIDTH;
                }
                if (y1 == y2)
                {
                    y2 += Constants.ROOM_MAX_HEIGHT;
                }
                f = checkValidPlace(x1, y1, x2, y2);
            }

            Log("Координаты комнаты " + id.ToString() + " " + tileType.ToString() + ": " + x1.ToString() + ", " + y1.ToString() + ", " + x2.ToString() + ", " + y2.ToString());


            for (int n = y1; n < y2; n++)
            {
                for (int m = x1; m < x2; m++)
                {
                    map[n][m] = tileType;
                }
            }

            rooms.Add(new Room(x1, y1, x2, y2, tileType, id));

        }

        public void generateHallways() {

            // TODO сделать проверку на то, что комната рядом по вертикали (по горизонтали работает, по вертикали нет)
            for (int i = 0; i < rooms.Count(); i++) {
                Boolean logic = true;
                if (rooms[i].HallwayNumber != 4)
                {
                    // коридор влево
                    if (!rooms[i].HLeft)
                    {
                        for (int y = rooms[i].YStart; y < rooms[i].YEnd; y++)
                        {
                            if (logic)
                            {
                                for (int x = rooms[i].XStart - 1; x > 0; x--)
                                {
                                    if (map[y][x] != TileType.NG && map[y][x] != TileType.HW)
                                    {
                                        for (int k = 0; k < rooms.Count(); k++)
                                        {
                                            if (y <= rooms[k].YEnd && y >= rooms[k].YStart && x == rooms[k].XEnd && !rooms[k].HRight)
                                            {
                                                Boolean isMinLength = true;

                                                for (int xd = rooms[i].XStart - 1; xd > rooms[k].XEnd; xd--)
                                                {
                                                    if (map[y - 1][xd] != TileType.NG && map[y + 1][xd] != TileType.NG && map[y][xd] != TileType.NG)
                                                    {
                                                        isMinLength = false;
                                                        break;
                                                    }
                                                }
                                                if (isMinLength)
                                                {
                                                    adjacencyMatrix[i][k] = true;
                                                    adjacencyMatrix[k][i] = true;
                                                    rooms[i].HLeft = true;
                                                    rooms[k].HRight = true;
                                                    rooms[i].HallwayNumber += 1;
                                                    rooms[k].HallwayNumber += 1;
                                                    hallways.Add(new Hallway(rooms[i].XStart, y, x, y, i, k));
                                                    for (int xHallway = rooms[i].XStart; xHallway > rooms[k].XEnd; xHallway--)
                                                    {
                                                        map[y][xHallway] = TileType.HW;
                                                    }
                                                    logic = false;
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    logic = true;
                    // коридор вправо
                    if (!rooms[i].HRight)
                    {
                        for (int y = rooms[i].YStart; y < rooms[i].YEnd; y++)
                        {
                            if (logic)
                            {
                                for (int x = rooms[i].XEnd + 1; x < 32; x++)
                                {
                                    if (map[y][x] != TileType.NG && map[y][x] != TileType.HW)
                                    {
                                        for (int k = 0; k < rooms.Count(); k++)
                                        {
                                            if (y <= rooms[k].YEnd + 1 && y >= rooms[k].YStart && x == rooms[k].XStart && !rooms[k].HLeft)
                                            {
                                                Boolean isMinLength = true;

                                                for (int xd = rooms[i].XEnd + 1; xd < rooms[k].XStart; xd++)
                                                {
                                                    if (map[y - 1][xd] != TileType.NG && map[y + 1][xd] != TileType.NG && map[y][xd] != TileType.NG)
                                                    {
                                                        isMinLength = false;
                                                        break;
                                                    }
                                                }
                                                if (isMinLength)
                                                {
                                                    adjacencyMatrix[i][k] = true;
                                                    adjacencyMatrix[k][i] = true;
                                                    rooms[i].HRight = true;
                                                    rooms[k].HLeft = true;
                                                    rooms[i].HallwayNumber += 1;
                                                    rooms[k].HallwayNumber += 1;
                                                    hallways.Add(new Hallway(rooms[i].XStart, y, x, y, i, k));
                                                    for (int xHallway = rooms[i].XEnd; xHallway < rooms[k].XStart; xHallway++)
                                                    {
                                                        map[y][xHallway] = TileType.HW;
                                                    }
                                                    logic = false;
                                                    //break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    logic = true;
                    // коридор вниз
                    if (!rooms[i].HDown)
                    {
                        for (int x = rooms[i].XStart; x < rooms[i].XEnd; x++)
                        {
                            if (logic)
                            {
                                for (int y = rooms[i].YEnd + 1; y < 32; y++)
                                {
                                    if (map[y][x] != TileType.NG && map[y][x] != TileType.HW)
                                    {
                                        for (int k = 0; k < rooms.Count(); k++)
                                        {
                                            if (x <= rooms[k].XEnd && x >= rooms[k].XStart && y == rooms[k].YStart && !rooms[k].HUp)
                                            {
                                                Boolean isMinLength = true;

                                                for (int xd = rooms[i].YEnd + 1; xd < rooms[k].YStart; xd++)
                                                {
                                                    if (map[xd][x - 1] != TileType.NG && map[xd][x + 1] != TileType.NG && map[xd][x] != TileType.NG)
                                                    {
                                                        isMinLength = false;
                                                        break;
                                                    }
                                                }
                                                if (isMinLength)
                                                {
                                                    adjacencyMatrix[i][k] = true;
                                                    adjacencyMatrix[k][i] = true;
                                                    rooms[i].HDown = true;
                                                    rooms[k].HUp = true;
                                                    rooms[i].HallwayNumber += 1;
                                                    rooms[k].HallwayNumber += 1;
                                                    hallways.Add(new Hallway(x, rooms[i].YStart, x, y, i, k));
                                                    for (int yHallway = rooms[i].YEnd; yHallway < rooms[k].YStart; yHallway++)
                                                    {
                                                        map[yHallway][x] = TileType.HW;
                                                    }
                                                    logic = false;
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }

                    logic = true;
                    // коридор вверх
                    if (!rooms[i].HUp)
                    {
                        for (int x = rooms[i].XStart; x < rooms[i].XEnd; x++)
                        {
                            if (logic)
                            {
                                for (int y = rooms[i].YStart - 1; y > 0; y--)
                                {
                                    if (map[y][x] != TileType.NG && map[y][x] != TileType.HW)
                                    {
                                        for (int k = 0; k < rooms.Count(); k++)
                                        {
                                            if (x <= rooms[k].XEnd && x >= rooms[k].XStart && y == rooms[k].YEnd && !rooms[k].HDown)
                                            {
                                                Boolean isMinLength = true;

                                                for (int xd = rooms[i].YStart - 1; xd > rooms[k].YEnd; xd++)
                                                {
                                                    if (map[xd][x - 1] != TileType.NG && map[xd][x + 1] != TileType.NG && map[xd][x] != TileType.NG)
                                                    {
                                                        isMinLength = false;
                                                        break;
                                                    }
                                                }
                                                if (isMinLength)
                                                {
                                                    adjacencyMatrix[i][k] = true;
                                                    adjacencyMatrix[k][i] = true;
                                                    rooms[i].HUp = true;
                                                    rooms[k].HDown = true;
                                                    rooms[i].HallwayNumber += 1;
                                                    rooms[k].HallwayNumber += 1;
                                                    hallways.Add(new Hallway(x, rooms[i].YEnd, x, y, i, k));
                                                    for (int yHallway = rooms[i].YStart; yHallway < rooms[k].YEnd; yHallway--)
                                                    {
                                                        map[yHallway][x] = TileType.HW;
                                                    }
                                                    logic = false;
                                                    break;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < Constants.MAX_ROOM_COUNT; i++)
            {
                for (int j = 0; j < Constants.MAX_ROOM_COUNT; j++)
                {
                    if(adjacencyMatrix[i][j])
                        Console.Write(1.ToString() + " ");
                    else 
                        Console.Write(0.ToString() + " ");
                }
                Console.WriteLine();
            }

            for (int i = 0; i < hallways.Count(); i++) {
                Console.WriteLine(hallways[i].XStart.ToString() + " " + hallways[i].YStart.ToString() + " " + hallways[i].XEnd.ToString() + " " + hallways[i].YEnd.ToString() + " ");
            }

        }

        public bool IsConnected()
        {

            bool[] visited = new bool[Constants.MAX_ROOM_COUNT];
            DFS(0, visited);

            // Check if all vertices were visited
            for (int i = 0; i < Constants.MAX_ROOM_COUNT; i++)
            {
                if (!visited[i])
                    return false;
            }

            return true;
        }

        private void DFS(int vertex, bool[] visited)
        {
            visited[vertex] = true;

            for (int i = 0; i < Constants.MAX_ROOM_COUNT; i++)
            {
                if (adjacencyMatrix[vertex][i] == true && !visited[i])
                {
                    DFS(i, visited);
                }
            }
        }

        public void show() {
            for (int i = 0; i < 32; i++) {
                for (int j = 0; j < 32; j++) {
                    if (map[i][j] == TileType.NG)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else 
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.Write(map[i][j]);
                }
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
            }

        }

        public void Log(String message) { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("##LOG##: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);
        }



    }
}
