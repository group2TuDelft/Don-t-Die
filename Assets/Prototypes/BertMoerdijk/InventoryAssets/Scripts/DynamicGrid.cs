using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour { 

public int rows;
public int cols;

void Update()
{
    RectTransform parentRect = gameObject.GetComponent<RectTransform>();
    GridLayoutGroup gridLayout = gameObject.GetComponent<GridLayoutGroup>();
    gridLayout.cellSize = new Vector2(parentRect.rect.width / cols, parentRect.rect.height / rows);

}

}