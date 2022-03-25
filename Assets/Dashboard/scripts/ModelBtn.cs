using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBtn : MonoBehaviour
{
    public Button buttonAdd;
    private string address = string.Empty;

    void Start()
    {
        buttonAdd.onClick.AddListener(delegate { OnClickAdd(); });
    }

    public void SetText(string name)
    {
        address = name;
        buttonAdd.transform.GetChild(0).GetComponent<Text>().text = name; 
    }

    private void OnClickAdd()
    {
        FileManager.Instance.LoadPrefabDefault("default", address, Utils.CreateUUID());
        Dashboard.Instance.SetToggleGroupDefault();
    }
}
