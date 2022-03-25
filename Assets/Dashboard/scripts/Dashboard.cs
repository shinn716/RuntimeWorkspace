using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dashboard : MonoBehaviour
{
    public static Dashboard Instance;

    [SerializeField] private Transform hierarchy;
    [SerializeField] private Transform gameObjGroup;
    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private ToggleGroup toggleGroup;

    public RuntimeGizmos.TransformGizmo transformGizmo;
    public bool Enable { get; set; } = false;

    private int preCount = 0;
    private int uiIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(Init);
    }

    private void Update()
    {
        if (!Enable)
            return;

        if (gameObjGroup.childCount == 0)
            return;

        if (preCount != gameObjGroup.childCount)
        {
            SetContainer(preCount);
            preCount = gameObjGroup.childCount;
        }
    }

    public void RefleshContainer()
    {
        preCount = gameObjGroup.childCount;
    }

    private bool Init()
    {
        preCount = gameObjGroup.childCount;
        for (var i = 0; i < preCount; i++)
            SetContainer(i);
        Enable = true;
        return Enable;
    }

    private void SetContainer(int index)
    {
        GameObject go = Instantiate(ItemPrefab);
        var name = gameObjGroup.GetChild(index).name;
        go.name = name;
        go.transform.SetParent(hierarchy);

        var item = go.GetComponent<BtnItem>();
        item.name = $"item_{uiIndex++}";
        item.SetName(name);
        item.SetTarget(gameObjGroup.GetChild(index));
        item.ItemButton.group = toggleGroup;
    }

    public void SetToggleGroupDefault()
    {
        toggleGroup.SetAllTogglesOff();
    }
}
