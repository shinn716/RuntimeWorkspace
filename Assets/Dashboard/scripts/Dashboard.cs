using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dashboard : MonoBehaviour
{
    public static Dashboard Instance;

    [SerializeField] private Transform Container;
    [SerializeField] private Transform ObjectGroup;
    [SerializeField] private GameObject ItemPrefab;

    public RuntimeGizmos.TransformGizmo transformGizmo;
    public bool Enable { get; set; } = false;

    private int preCount = 0;

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

        if (ObjectGroup.childCount == 0)
            return;

        if (preCount != ObjectGroup.childCount)
        {
            SetContainer(preCount);
            preCount = ObjectGroup.childCount;
        }
    }

    public void RefleshContainer()
    {
        preCount = ObjectGroup.childCount;
    }

    private bool Init()
    {
        preCount = ObjectGroup.childCount;
        for (var i = 0; i < preCount; i++)
            SetContainer(i);
        Enable = true;
        return Enable;
    }

    private void SetContainer(int index)
    {
        GameObject go = Instantiate(ItemPrefab);
        var name = ObjectGroup.GetChild(index).name;
        go.name = name;
        go.transform.SetParent(Container);

        var item = go.GetComponent<Item>();
        item.SetName(name);
        item.SetTarget(ObjectGroup.GetChild(index));
        item.ItemButton.group = GetComponent<UnityEngine.UI.ToggleGroup>();
    }
}
