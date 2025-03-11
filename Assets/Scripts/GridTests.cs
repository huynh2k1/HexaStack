using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GridTests : MonoBehaviour
{
    [SerializeField] private Grid grid;

    [Header("SETTINGS")]
    [SerializeField] private Vector3Int gridPos;

    private void OnValidate()
    {
        UpdatePostion();
    }

    private void UpdatePostion()
    {
        transform.position = grid.CellToWorld(gridPos);
        //gridPos = grid.WorldToCell(transform.position);
    }
}
