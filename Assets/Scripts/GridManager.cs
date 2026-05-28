using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    public int rows = 3;
    public int columns = 10;
    public float cellSize = 2.5f;
    public int originalColumns = 5;
    public Vector2 gridOffset = new Vector2(2, 0);

    private GameObject[,] slots;
    private Vector2[,] slotPositions;

    private void Awake()
    {
        Instance = this;
        slots = new GameObject[columns, rows];
        slotPositions = new Vector2[columns, rows];

        for (int x = 0; x < columns; x++)
            for (int y = 0; y < rows; y++)
                slotPositions[x, y] = ComputeCellPosition(x, y);

        var parent = GameObject.Find("GridSlots");
        if (parent != null)
        {
            foreach (Transform child in parent.transform)
            {
                string name = child.name;
                if (name.StartsWith("Slot") && int.TryParse(name.Substring(4), out int index))
                {
                    int localIndex = index % 15;
                    int gx = localIndex % originalColumns;
                    int gy = localIndex / originalColumns;
                    if (index >= 15) gx += originalColumns;
                    if (gx < columns && gy < rows)
                        slotPositions[gx, gy] = child.position;
                }
            }
        }
    }

    public Vector2 GetCellPosition(int x, int y)
    {
        if (x >= 0 && x < columns && y >= 0 && y < rows)
            return slotPositions[x, y];
        return ComputeCellPosition(x, y);
    }

    private Vector2 ComputeCellPosition(int x, int y)
    {
        float offsetX = (originalColumns - 1) / 2f;
        float offsetY = (rows - 1) / 2f;
        Vector2 pos = new Vector2((x - offsetX) * cellSize, (y - offsetY) * cellSize - 0.5f);
        if (x >= originalColumns)
            pos += gridOffset;
        return pos;
    }

    public (int x, int y) GetCellFromPosition(Vector2 worldPos)
    {
        int bestX = 0, bestY = 0;
        float bestDist = float.MaxValue;
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < columns; x++)
            {
                float dist = Vector2.Distance(worldPos, GetCellPosition(x, y));
                if (dist < bestDist)
                {
                    bestDist = dist;
                    bestX = x;
                    bestY = y;
                }
            }
        }
        return (bestX, bestY);
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