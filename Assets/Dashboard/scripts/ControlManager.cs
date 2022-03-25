using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    [SerializeField] private Transform hierarchy;

    private List<Toggle> toggles = new List<Toggle>();
    private int preCount = 0;

    // Update is called once per frame
    private void Update()
    {
        if (hierarchy.childCount!=preCount)
        {
            toggles.Clear();
            foreach (Transform i in hierarchy)
                toggles.Add(i.GetComponent<Toggle>());
            preCount = hierarchy.childCount;
        }

        for(int i =0; i<=toggles.Count; i++)
        {
            if (Input.GetKeyDown(NumberHotKeyDefine(i)))
            {
                var index = i - 1;
                if (index >= 0)
                    toggles[index].isOn = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
            Dashboard.Instance.SetToggleGroupDefault();
    }
    private KeyCode NumberHotKeyDefine(int num)
    {
        switch (num)
        {
            default: return KeyCode.Alpha0;
            case 1: return KeyCode.Alpha1;
            case 2: return KeyCode.Alpha2;
            case 3: return KeyCode.Alpha3;
            case 4: return KeyCode.Alpha4;
            case 5: return KeyCode.Alpha5;
            case 6: return KeyCode.Alpha6;
            case 7: return KeyCode.Alpha7;
            case 8: return KeyCode.Alpha8;
            case 9: return KeyCode.Alpha9;
        }
    }
}
