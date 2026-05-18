using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public int rows = 3;
    public int columns = 5;
    public float cellSize = 2.5f;

    private GameObject[,] slots;

    private void Awake()
    {
        Instance = this;
        slots = new GameObject[columns, rows];
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        float offsetX = (columns - 1) / 2f;
        float offsetY = (rows - 1) / 2f;
        return new Vector2((x - offsetX) * cellSize, (y - offsetY) * cellSize);
    }

    public (int x, int y) GetCellFromPosition(Vector2 worldPos)
    {
        float offsetX = (columns - 1) / 2f;
        float offsetY = (rows - 1) / 2f;
        int x = Mathf.RoundToInt(worldPos.x / cellSize + offsetX);
        int y = Mathf.RoundToInt(worldPos.y / cellSize + offsetY);
        return (x, y);
    }

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < columns && y >= 0 && y < rows;
    }

    public bool IsCellEmpty(int x, int y)
    {
        if (!IsValidCell(x, y)) return false;
        return slots[x, y] == null;
    }

    public (int x, int y)? FindRandomEmptySlot()
    {
        var empty = new List<(int x, int y)>();
        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                if (slots[x, y] == null)
                    empty.Add((x, y));

        if (empty.Count == 0) return null;
        return empty[Random.Range(0, empty.Count)];
    }

    public void PlaceItem(GameObject item, int x, int y)
    {
        if (!IsValidCell(x, y)) return;
        slots[x, y] = item;
    }

    public void RemoveItem(int x, int y)
    {
        if (!IsValidCell(x, y)) return;
        slots[x, y] = null;
    }

    public GameObject GetItemAt(int x, int y)
    {
        if (!IsValidCell(x, y)) return null;
        return slots[x, y];
    }
}