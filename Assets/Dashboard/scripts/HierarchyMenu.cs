using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HierarchyMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menu;
    [SerializeField] private Vector2 origin;
    [SerializeField] private Vector2 close;

    public void Open()
    {
        menu.anchoredPosition = origin;
    }

    public void Close()
    {
        menu.anchoredPosition = close;
    }
}
