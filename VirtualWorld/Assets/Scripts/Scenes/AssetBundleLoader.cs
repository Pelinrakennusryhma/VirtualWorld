using Cysharp.Threading.Tasks;
using Scenes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class AssetBundleLoader : MonoBehaviour
{
    [SerializeField] string baseUrl;
    public static AssetBundleLoader Instance { get; private set; }

    // Keeps track of bundled scenes that have been loaded
    List<string> loadedBundledSceneNames = new List<string>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        // Asset bundles for editor are created in a different folder for WindowsStandAlone64 because apparently that's what editor is
        // The folders asset-bundles and asset-bundles-dev from the project root folder must be moved to the server for Apache to serve.
        // Editor downloads asset bundles from https://gameserver.hyvanmielenpelit.fi/virtual-world/asset-bundles-dev, WebGL build from same address without the "-dev"
#if UNITY_EDITOR
        baseUrl += "-dev";
#endif
        if (!baseUrl.EndsWith("/"))
        {
            baseUrl += "/"; 
        }
    }
    void AddLoadedBundledSceneName(string name)
    {
        loadedBundledSceneNames.Add(name);
    }

    bool BundledSceneAlreadyLoaded(string name)
    {
        return loadedBundledSceneNames.Contains(name);
    }

    // If bundled scene has already been loaded before or is successfully loaded now, return true so scene loading can resume.
    public async UniTask<bool> DownloadBundledScene(string sceneName)
    {
        if (BundledSceneAlreadyLoaded(sceneName))
        {
            return true;
        }

        bool success = false;
        await GetAssetBundles(sceneName, (loadedAssetBundles) =>
        {
            if(loadedAssetBundles != null)
            {
                success = true;
            }
        });

        return success;
    }

    // callback is called with either null to inform about failed downloading or with the AssetBundle on success
    IEnumerator GetAssetBundles(string sceneName, UnityAction<AssetBundle> callback)
    {
        string formattedSceneName = sceneName.ToLower();
        string sceneBundlePath = baseUrl + formattedSceneName + "_scene"; // scenes must be bundled separately and loaded before the assets used in it
        string assetsBundlePath = baseUrl + formattedSceneName + "_assets";

        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(sceneBundlePath);
        Debug.Log("Attempting to download bundle from: " + sceneBundlePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback(null);
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
        }

        www = UnityWebRequestAssetBundle.GetAssetBundle(assetsBundlePath);
        Debug.Log("Attempting to download bundle from: " + assetsBundlePath);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            callback(null);
            yield break;
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);

            Debug.Log("--- Loaded Assets --- ");
            //foreach (string assetName in bundle.GetAllAssetNames())
            //{
            //    if (assetName.EndsWith(".mat"))
            //    {
            //        Debug.Log("Loading asset: " + assetName);
            //        bundle.LoadAsset(assetName);
            //    }
            //}

            //foreach (string assetName in bundle.GetAllAssetNames())
            //{
            //    if (!assetName.EndsWith(".mat"))
            //    {
            //        Debug.Log("Loading asset: " + assetName);
            //        bundle.LoadAsset(assetName);
            //    }
            //}

            bundle.LoadAllAssets();

            Debug.Log("--- End Assets --- ");
            AddLoadedBundledSceneName(sceneName);
            callback(bundle);
        }
    }
}