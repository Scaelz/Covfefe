using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeSizeUI : MonoBehaviour
{
    [SerializeField] GameObject scrollBarFromPanel;
    [SerializeField] int minMenuCount = 2;
    void Start()
    {
        SetActiveScrollBar();
    }

    private void SetActiveScrollBar()
    {
        ScrollRect scrollRect = scrollBarFromPanel.GetComponent<ScrollRect>();
        GridLayoutGroup gridLayout = GetComponent<GridLayoutGroup>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        int cellCount = gameObject.transform.childCount;
        float heightCell = gridLayout.cellSize.y + gridLayout.spacing.y;
        if (cellCount > minMenuCount)
        {
            rectTransform.offsetMin = new Vector2(0, -heightCell * (cellCount - minMenuCount));
            scrollRect.vertical = true;
        }
        else
        {
            rectTransform.offsetMin = new Vector2(0, 0);
            scrollRect.vertical = false;
        }
    }
}
