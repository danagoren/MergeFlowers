using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public int rows = 5;
    public int columns = 5;
    public float cellSize = 1.0f;

    private MergeItem[,] grid;

    private void Awake()
    {
        Instance = this;
        grid = new MergeItem[columns, rows];
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        return new Vector2(x * cellSize, y * cellSize);
    }

    public (int x, int y) GetCellFromPosition(Vector2 worldPos)
    {
        int x = Mathf.RoundToInt(worldPos.x / cellSize);
        int y = Mathf.RoundToInt(worldPos.y / cellSize);
        return (x, y);
    }

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public bool IsCellOccupied(int x, int y)
    {
        if (!IsValidCell(x, y)) return false;
        return grid[x, y] != null;
    }

    public void PlaceItem(MergeItem item, int x, int y)
    {
        if (!IsValidCell(x, y)) return;
        grid[x, y] = item;
        item.SetGridPosition(x, y);
    }

    public void RemoveItem(int x, int y)
    {
        if (!IsValidCell(x, y)) return;
        grid[x, y] = null;
    }

    public MergeItem GetItemAt(int x, int y)
    {
        if (!IsValidCell(x, y)) return null;
        return grid[x, y];
    }
}