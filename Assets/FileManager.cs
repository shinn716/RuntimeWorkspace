using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public class FileManager : MonoBehaviour
{  
    
    
    //Add a script with this code to a Gameobject to get a List of assignable asset references
    [SerializeField] private List<AssetReference> references = new List<AssetReference>();

    void Start()
    {
        //Addressables.

        //print("name " + Addressables.);
    }


    [ContextMenu("LoadPrefab")]
    void LoadPrefab()
    {
        StartCoroutine(LoadScene_Co());

        //Addressables.LoadAssetAsync<GameObject>("model").Completed += OnLoadPrefabDone;
    }

    private void OnLoadPrefabDone(AsyncOperationHandle<GameObject> obj)
    {
        // In a production environment, you should add exception handling to catch scenarios such as a null result.
        var myGameObject = obj.Result;
        Instantiate(myGameObject);
        print(myGameObject.name);
    }


    public AsyncOperationHandle loadMapOperation;



    IEnumerator LoadScene_Co()
    {
        //var cc = "https://addessablebundlebucket.s3.ap-northeast-1.amazonaws.com/StandaloneWindows64/catalog_2022.02.11.08.45.44.json";
        var cc = "C:\\Users\\COMPALDEV\\Desktop\\load\\catalog_2022.03.21.09.28.05.json";

        bool catUpdate = false;
        Addressables.LoadContentCatalogAsync(cc).Completed += (res) =>
        {
            catUpdate = true;
        };

        while (!catUpdate)
            yield return null;

        loadMapOperation = Addressables.LoadAssetsAsync<GameObject>("model", DidLoad
            );

        //loadMapOperation = Addressables.LoadSceneAsync("Assets/RPG_FPS_game_assets_industrial/Map_v2.unity", LoadSceneMode.Additive);

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


    private void DidLoad(GameObject result)
    {
        print(result.name);
        // ... Do what you need to do here
    }
}
