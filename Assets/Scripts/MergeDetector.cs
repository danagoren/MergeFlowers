using UnityEngine;
using System.Collections.Generic;

public class MergeDetector : MonoBehaviour
{
    public static MergeDetector Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void CheckMergeAt(int x, int y)
    {
        MergeItem currentItem = GridManager.Instance.GetItemAt(x, y);
        if (currentItem == null) return;

        int targetTier = currentItem.Tier;

        int[] dx = { 1, -1, 0, 0 };
        int[] dy = { 0, 0, 1, -1 };

        for (int i = 0; i < 4; i++)
        {
            int nx = x + dx[i];
            int ny = y + dy[i];

            MergeItem neighbor = GridManager.Instance.GetItemAt(nx, ny);
            if (neighbor != null && neighbor.Tier == targetTier)
            {
                PerformMerge(currentItem, neighbor);
                return;
            }
        }
    }

    private void PerformMerge(MergeItem target, MergeItem other)
    {
        Vector2 newPos = GridManager.Instance.GetCellPosition(target.GridX, target.GridY);
        Destroy(other.gameObject);

        target.Upgrade();
        target.transform.position = newPos;

        GameManager.Instance.AddScore(target.Tier * 10);
    }
}