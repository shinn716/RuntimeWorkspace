using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;

    public string jsonPath = "C:\\Users\\COMPALDEV\\Desktop\\load\\catalog_2022.03.23.02.35.06.json";
    public List<string> prefabNamelist = new List<string>();

    [SerializeField]
    private GameObject btnprefab;
    [SerializeField]
    private Transform uiParent;
    [SerializeField]
    private Transform gameParent;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GetPrefabFromJson();
    }

    public void Load(string address)
    {
        StartCoroutine(LoadByName(address));
    }
    public void GetPrefabFromJson()
    {
        foreach (Transform child in uiParent)
            Destroy(child.gameObject);

        prefabNamelist.Clear();

        try
        {
            StreamReader reader = new StreamReader(jsonPath);
            string txt = reader.ReadToEnd();
            var resultString1 = Regex.Split(txt, "prefab_", RegexOptions.IgnoreCase);
            //print(resultString1[1]);
            for (int i = 1; i < resultString1.Length; i++)
            {
                var resultString2 = resultString1[i].Split('"');
                //print(resultString2[0]);
                var name = $"Assets/prefab_{resultString2[0].Trim()}";
                prefabNamelist.Add(name);
                CreateBtn(name);
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }



    private IEnumerator LoadByName(string name)
    {
        var cc = jsonPath;
        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(name);
        handle.Completed += obj =>
        {
            var myGameObject = obj.Result;
            GameObject go = Instantiate(myGameObject);
            go.transform.SetParent(gameParent);
        };
        print("LoadByName:{name}Done");
    }
    private IEnumerator LoadAll()
    {
        var cc = jsonPath;
        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        foreach (var i in prefabNamelist)
        {
            Addressables.LoadAssetsAsync<GameObject>(i,
                obj =>
                {
                    Debug.Log(obj.name);
                    GameObject go = Instantiate(obj);
                    go.transform.SetParent(gameParent);
                }, false);
        }
        print("LoadAll:Done");
    }

    private void CreateBtn(string name)
    {
        GameObject go =  Instantiate(btnprefab);
        go.transform.SetParent(uiParent);
        go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        go.GetComponent<ModelBtn>().SetText(name);
    }
}
