using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

//AssetBundle�̃r���h���s���E�B���h�E
public class AssetBundleBuilder : EditorWindow
{
    [UnityEditor.MenuItem("Window/AssetBundleBuilder")]

    static void Open()
    {
        var window = GetWindow<AssetBundleBuilder>();
        window.titleContent = new GUIContent("AssetBundleBuild");
    }

    //�r���h�Ώۂ�Asset�AAssetBundle�̖��́A�o�̓f�B���N�g���̕ϐ�
    private Object InputAsset, OutputDirectory;
    private string AssetBundleName;

    private void OnGUI()
    {
        //GUI��ŕ\��
        GUILayout.BeginHorizontal();
        GUILayout.Label("Input Asset:", GUILayout.Width(110));
        InputAsset = EditorGUILayout.ObjectField(InputAsset, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("AssetBundleName:", GUILayout.Width(110));
        AssetBundleName = EditorGUILayout.TextField(AssetBundleName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("OutputDirectory:", GUILayout.Width(110));
        OutputDirectory = EditorGUILayout.ObjectField(OutputDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if(GUILayout.Button("Build AssetBundle"))
        {
            BuildAssetBundle(InputAsset, AssetBundleName, OutputDirectory);
        }
    }

    public void BuildAssetBundle(Object Input, string Name, Object Output)
    {
        var builds = new List<AssetBundleBuild>();

        // AssetBundle���Ƃ���Ɋ܂߂�A�Z�b�g���w�肷��
        var build = new AssetBundleBuild();
        build.assetBundleName = "SampleJson4";
        build.assetNames = new string[1] {"Aseets/Mapdata/SampleJson4.json"};

        builds.Add(build);

        // ���ʕ����o�͂���t�H���_���w�肷��i�v���W�F�N�g�t�H���_����̑��΃p�X�j
        var targetDir = "StreamingAssets";
        if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

        // Android�p�ɏo��
        var buildTarget = BuildTarget.Android;

        // LZ4�ň��k����悤�ɂ���
        var buildOptions = BuildAssetBundleOptions.ChunkBasedCompression;

        BuildPipeline.BuildAssetBundles(targetDir, builds.ToArray(), buildOptions, buildTarget);
    }
}
