using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.IO;
using System.Text;

public class SystemManager : MonoBehaviour
{
    [Serializable]
    public class ObjectTransform
    {
        public ObjectTransform(string _name, Transform _transform, string _address, string _uuid)
        {
            name = _name;
            position = _transform.position;
            rotation = _transform.rotation;
            scale = _transform.localScale;
            address = _address;
            uuid = _uuid;
        }

        public string name = string.Empty;
        public string address = string.Empty;
        public string uuid = string.Empty;

        public Vector3 position = Vector3.zero;
        public Quaternion rotation = Quaternion.identity;
        public Vector3 scale = Vector3.one;
    }

    [SerializeField] private Transform hierarchy;
    [SerializeField] private Transform gameObjGroup;

    private List<ObjectTransform> objectTransforms = new List<ObjectTransform>();

    private bool exportFinish = false;
    private bool loadFinish = false;

    public void Load()
    {
        if (loadFinish)
            return;
        loadFinish = true;
        string path = Path.Combine(Application.streamingAssetsPath, "data.json");
        StartCoroutine(ProcessLoadData(path));
    }

    public void Export()
    {
        if (exportFinish)
            return;
        exportFinish = true;
        StartCoroutine(ProcessSaveToLocal());
    }


    private IEnumerator ProcessSaveToLocal()
    {
        objectTransforms.Clear();
        foreach (Transform i in gameObjGroup)
        {
            var objcfg = i.GetComponent<ObjCfg>();
            objectTransforms.Add(new ObjectTransform(i.name, i, objcfg.Address, objcfg.UUID));
        }
        yield return null;
        var str = JsonHelper.ToJson(objectTransforms.ToArray());
        yield return null;
        string full = Path.Combine(Application.streamingAssetsPath, $"data.json");
        using (StreamWriter outputFile = new StreamWriter(full, false))
        {
            outputFile.WriteLine(str);
            outputFile.Close();
        }
        exportFinish = false;
        Debug.Log($"[Save to] {full}");
    }
    private IEnumerator ProcessLoadData(string path)
    {
        foreach (Transform i in hierarchy)
            i.SendMessage("MSGDelete");
        yield return null;
        StreamReader reader = new StreamReader(path);
        var json = reader.ReadToEnd();
        reader.Close();
        yield return null;
        objectTransforms = JsonHelper.FromJson<ObjectTransform>(json).ToList();
        yield return null;
        foreach(var i in objectTransforms)
        {
            FileManager.Instance.LoadPrefab(i.name, i.address, i.uuid, i.position, i.rotation, i.scale);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
        loadFinish = false;
    }
}
