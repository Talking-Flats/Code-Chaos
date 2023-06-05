using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Net.Mail;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapGeneration3.models;
using UnityEngine.AI;


public class Program : MonoBehaviour
{
    public GameObject[] tilePrefabs; // Префабы для разных типов тайлов
    public List<GameObject> enemies; 
    public int enemyCount = 0;
    private int tempEnemyCount = 0;

    public NavMeshSurface navMeshSurface; // Reference to the NavMeshSurface component

    void Start()
    {
        GenerateMap();
        //GenerateNavMesh();
    }

    void GenerateMap()
    {
        Map map = new Map();
        System.Random rnd = new System.Random();

        for (int y = 0; y < map.map.Length; y++)
        {
            for (int x = 0; x < map.map[y].Length; x++)
            {
                TileType tileType = map.map[y][x];

                // Получить префаб для соответствующего типа тайла
                GameObject tilePrefab = GetTilePrefab(tileType);

                if (tilePrefab != null)
                {
                    // Создать экземпляр префаба и разместить его на сцене
                    GameObject tileInstance = Instantiate(tilePrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                    if(tileType == TileType.DR) {
                        
                        if(rnd.Next(1, 50) > 25 && tempEnemyCount < enemyCount){
                            GameObject enemy = Instantiate(enemies[0], new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                            tempEnemyCount++;
                        }
                    }
                }
            }
        }
        
    }
    
    void GenerateNavMesh()
    {
        // Enable the NavMeshSurface component
        //navMeshSurface.enabled = true;

        // Build the NavMesh
        //navMeshSurface.BuildNavMesh();
    }

    GameObject GetTilePrefab(TileType tileType)
    {
        // Получить соответствующий префаб на основе типа тайла
        switch (tileType)
        {
            case TileType.BF:
                return tilePrefabs[0]; // Префаб для типа BF
            case TileType.SP:
                return tilePrefabs[1]; // Префаб для типа SP
            case TileType.DR:
                return tilePrefabs[2]; // Префаб для типа DR
            case TileType.HW:
                return tilePrefabs[3]; // Префаб для типа HW
            case TileType.NG:
                return null; // Нет префаба для типа NG
            default:
                return null;
        }
    }
}
