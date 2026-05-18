using UnityEngine;

public class DragHandler : MonoBehaviour
{
    private Vector3 startPosition;
    private bool isDragging = false;
    private MergeItem mergeItem;

    private void Awake()
    {
        mergeItem = GetComponent<MergeItem>();
    }

    private void OnMouseDown()
    {
        startPosition = transform.position;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePos.x, mousePos.y, startPosition.z);
    }

    private void OnMouseUp()
    {
        isDragging = false;
        HandleDrop();
    }

    private void HandleDrop()
    {
        var (x, y) = GridManager.Instance.GetCellFromPosition(transform.position);

        if (GridManager.Instance.IsValidCell(x, y) && !GridManager.Instance.IsCellOccupied(x, y))
        {
            GridManager.Instance.RemoveItem(mergeItem.GridX, mergeItem.GridY);
            GridManager.Instance.PlaceItem(mergeItem, x, y);
            transform.position = GridManager.Instance.GetCellPosition(x, y);

            MergeDetector.Instance.CheckMergeAt(x, y);
        }
        else
        {
            transform.position = GridManager.Instance.GetCellPosition(mergeItem.GridX, mergeItem.GridY);
        }
    }
}