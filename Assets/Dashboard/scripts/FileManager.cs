using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;

public class FileManager : MonoBehaviour
{
    public static FileManager Instance;

    [SerializeField]
    private GameObject btnprefab;
    [SerializeField]
    private Transform uiModelGroup;
    [SerializeField]
    private Transform gameObjGroup;
    public List<string> prefabNamelist = new List<string>();

#if UNITY_EDITOR
    [Header("Editor mode"), SerializeField]
    private UnityEditor.AddressableAssets.Settings.AddressableAssetSettings settings;
#endif
    [Header("Runtime mode"), SerializeField]
    private string LoadPath;

    private string jsonPath = string.Empty;
    private int uiIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string loadpath = string.Empty;
#if UNITY_EDITOR
        // Get load/build path
        var group = AddressableAssetSettingsDefaultObject.Settings.FindGroup("Packed Assets");
        BundledAssetGroupSchema schema = group.GetSchema<BundledAssetGroupSchema>();
        loadpath = schema.LoadPath.GetValue(settings);
#else
            loadpath = LoadPath;
#endif

        //print($"Loadpath: {schema.LoadPath.GetValue(settings)}");
        //print($"Buildpath: {schema.BuildPath.GetValue(settings)}");

        var info = new DirectoryInfo(loadpath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            if (file.Extension.Equals(".json"))
                jsonPath = Path.Combine(loadpath, file.Name);
        }

        print($"Load path: {jsonPath}");
        GetPrefabFromJson();
    }

    public void LoadPrefab(string name, string address, string uuid, Vector3 pos, Quaternion rot, Vector3 scl)
    {
        StartCoroutine(LoadByAddress(name, address, uuid, pos, rot, scl));
    }
    public void LoadPrefabDefault(string name, string address, string uuid)
    {
        StartCoroutine(LoadByAddressDefault(name, address, uuid));
    }
    public void GetPrefabFromJson()
    {
        foreach (Transform child in uiModelGroup)
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


    private IEnumerator LoadByAddressDefault(string name, string address, string uuid)
    {
        var cc = jsonPath;
        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += obj =>
        {
            var myGameObject = obj.Result;
            GameObject go = Instantiate(myGameObject);

            if (!name.Equals("default"))
                go.name = name;

            go.transform.SetParent(gameObjGroup);
            go.AddComponent<ObjCfg>().SetCfg(address, uuid);
        };
        //print($"LoadByName:{name}Done");
    }

    private IEnumerator LoadByAddress(string name, string address, string uuid, Vector3 pos, Quaternion rot, Vector3 scl)
    {
        var cc = jsonPath;
        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        yield return null;
        yield return null;
        yield return null;
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        handle.Completed += obj =>
        {
            var myGameObject = obj.Result;
            GameObject go = Instantiate(myGameObject);

            if (!name.Equals("default"))
                go.name = name;

            go.transform.SetParent(gameObjGroup);
            go.AddComponent<ObjCfg>().SetCfg(address, uuid);
            go.transform.SetPositionAndRotation(pos, rot);
            go.transform.localScale = scl;
        };
        //print($"LoadByName:{name}Done");
    }

    private void CreateBtn(string name)
    {
        GameObject go =  Instantiate(btnprefab);
        go.name = $"item_{uiIndex++}";
        go.transform.SetParent(uiModelGroup);
        go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        go.GetComponent<ModelBtn>().SetText(name);
    }
}
