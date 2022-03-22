using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.SceneManagement;

public class AddressableTest : MonoBehaviour
{
    public AsyncOperationHandle loadMapOperation;



    private void Start()
    {

    }


    //[ContextMenu("LoadScene")]
    //public void LoadScene()
    //{
    //    //UnityEngine.Caching.ClearCache();
    //    StartCoroutine(LoadScene_Co());
    //}

    [ContextMenu("LoadPrefab")]
    void LoadPrefab()
    {
        StartCoroutine(LoadScene_Co());
        //Addressables.LoadAssetAsync<GameObject>("Assets/M2 x 0.4mm Thread 5mm LONG SOCKET HEAD CAP SCREW.STL.prefab").Completed += OnLoadPrefabDone;
    }

    private void OnLoadPrefabDone(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        var myGameObject = obj.Result;
        Instantiate(myGameObject);
        print(myGameObject.name);
    }


    IEnumerator LoadScene_Co()
    {
        var cc = "https://addessablebundlebucket.s3.ap-northeast-1.amazonaws.com/StandaloneWindows64/catalog_2022.02.11.08.45.44.json";
        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        loadMapOperation = Addressables.LoadSceneAsync("Assets/RPG_FPS_game_assets_industrial/Map_v2.unity", LoadSceneMode.Additive);

        yield return null;
        var percentDone = 0.0f;
        while (!loadMapOperation.IsDone)
        {
            percentDone = Mathf.Floor(loadMapOperation.PercentComplete * 100.0f);

            Debug.Log($"percentDone - {percentDone}");
            yield return null;
        }
        // else scene should load automatically, right?
    }
}