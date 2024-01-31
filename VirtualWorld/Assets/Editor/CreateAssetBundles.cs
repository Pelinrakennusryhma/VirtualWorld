using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildEditorAssets();
        BuildWebGLAssets();
    }

    static void BuildEditorAssets()
    {
        string assetBundleDirectory = "asset-bundles-dev";
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
    }

    static void BuildWebGLAssets()
    {
        string assetBundleDirectory = "asset-bundles";

        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.WebGL);

    }
}