using UnityEngine;

public class MergeItem : MonoBehaviour
{
    public int Tier { get; private set; } = 1;
    public int GridX { get; private set; }
    public int GridY { get; private set; }

    public void SetGridPosition(int x, int y)
    {
        GridX = x;
        GridY = y;
    }

    public void Upgrade()
    {
        Tier++;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        transform.localScale = Vector3.one * (1f + Tier * 0.1f);
    }
}