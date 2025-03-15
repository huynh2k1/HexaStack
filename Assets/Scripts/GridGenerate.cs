using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerate : MonoBehaviour
{
    private Dictionary<Vector2Int, HexaGrid> grids = new Dictionary<Vector2Int, HexaGrid>();
    [SerializeField] Grid grid;
    [SerializeField] HexaGrid hexagrid;

    //[OnValueChanged("GenerateGrid")]
    [SerializeField] int gridSize;
    public const float hexaRadius = 0.86602540378f;

    public Vector2Int[] dyEven = {
            new Vector2Int(0, 1),   // Ô chéo phải trên  (i, j + 1)
            new Vector2Int(-1, 1),  // Ô chéo trái trên  (i - 1, j + 1)
            new Vector2Int(1, 0),   // Ô bên phải        (i + 1, j)
            new Vector2Int(-1, 0),  // Ô bên trái        (i - 1, j)
            new Vector2Int(0, -1),  // Ô chéo phải dưới (i, j - 1)
            new Vector2Int(-1, -1)  // Ô chéo trái dưới (i - 1, j - 1)
    };

    public Vector2Int[] dyOdd = {
            new Vector2Int(1, 1),   // Ô chéo phải trên  (i + 1, j + 1)
            new Vector2Int(0, 1),  // Ô chéo trái trên  (i, j + 1)
            new Vector2Int(1, 0),   // Ô bên phải        (i + 1, j)
            new Vector2Int(-1, 0),  // Ô bên trái        (i - 1, j)
            new Vector2Int(1, -1),  // Ô chéo phải dưới (i + 1, j - 1)
            new Vector2Int(0, -1)  // Ô chéo trái dưới (i, j - 1)
    };

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        transform.Clear();
        for (int x = -gridSize; x <= gridSize; x++)
        {
            for (int y = -gridSize; y <= gridSize; y++)
            {
                Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));

                if (spawnPos.magnitude > grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * gridSize)
                    continue;
                HexaGrid hexaGrid = Instantiate(hexagrid, spawnPos, Quaternion.identity, transform);
                hexaGrid.PosX = x;
                hexaGrid.PosY = y;
                hexaGrid.name = $"({x}, {y})";

                grids.Add(new Vector2Int(x, y), hexaGrid);
            }
        }
    }

    public List<HexaGrid> GetListHexaGridNeightbor(HexaGrid cellCheck)
    {
        List<HexaGrid> neighbors = new List<HexaGrid>();
        if(Mathf.Abs(cellCheck.PosY) % 2 == 0)
        {
            foreach (var dir in dyEven)
            {
                Vector2Int neightBorPos = new Vector2Int(cellCheck.PosX + dir.x, cellCheck.PosY + dir.y);
                if (grids.ContainsKey(neightBorPos))
                {
                    HexaGrid hexaGrid = grids[neightBorPos];
                    if (hexaGrid.IsOccupied == false)
                        continue;
                    neighbors.Add(hexaGrid);
                }
            }
        }
        else
        {
            foreach (var dir in dyOdd)
            {
                Vector2Int neightBorPos = new Vector2Int(cellCheck.PosX + dir.x, cellCheck.PosY + dir.y);

                if (grids.ContainsKey(neightBorPos))
                {
                    HexaGrid hexaGrid = grids[neightBorPos];
                    if (hexaGrid.IsOccupied == false)
                        continue;
                    neighbors.Add(hexaGrid);
                }
            }
        }
        return neighbors;
    }
}
