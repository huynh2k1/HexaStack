using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerate : MonoBehaviour
{
    [SerializeField] Grid grid;
    [SerializeField] GameObject hexagon;

    [SerializeField] int gridSize;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        transform.Clear();
        for(int x = -gridSize; x <= gridSize; x++)
        {
            for(int y = -gridSize; y <= gridSize; y++)
            {
                Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));

                if (spawnPos.magnitude > grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * gridSize)
                    continue;
                GameObject hexa = Instantiate(hexagon, spawnPos, Quaternion.identity, transform);
                hexa.name = $"({x}, {y})";
            }
        }
    }
}
