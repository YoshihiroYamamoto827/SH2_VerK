using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

//MapEditor

public class MapEditor : EditorWindow
{
    //�摜�f�B���N�g��
    private Object imgDirectory;
    //�o�͐�f�B���N�g��(���w��̏ꍇAsset���ɏo��)
    private Object outputDirectory;
    //�}�b�v�G�f�B�^�̃}�X�̐�
    private int mapSize = 10;
    //�O���b�h�̑傫��
    private float gridSize = 50.0f;
    //�o�̓t�@�C����
    private string outputFileName;
    //�I�������摜�̃p�X
    private string selectedImagePath;
    //�T�u�E�B���h�E
    private MapEditorSubWindow subWindow;

    [UnityEditor.MenuItem("Window/Mapcreater")]
    static void ShowTestMainWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
    }

    private void OnGUI()
    {
        //GUI��ŕ\��
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image Directory:", GUILayout.Width(110));
        imgDirectory = EditorGUILayout.ObjectField(imgDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map Size:", GUILayout.Width(110));
        mapSize = EditorGUILayout.IntField(mapSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Size:", GUILayout.Width(110));
        gridSize = EditorGUILayout.FloatField(gridSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Directory:", GUILayout.Width(110));
        outputDirectory = EditorGUILayout.ObjectField(outputDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Filename:", GUILayout.Width(110));
        outputFileName = (string)EditorGUILayout.TextField(outputFileName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        DrawImageParts();

        DrawSelectedImage();

        DrawMapWindowButton();
    }

    //�摜�ꗗ���{�^���Ƃ��ďo��
    private void DrawImageParts()
    {
        if(imgDirectory != null)
        {
            float x = 0.0f;
            float y = 00.0f;
            float w = 50.0f;
            float h = 50.0f;
            float maxW = 300.0f;

            string path = AssetDatabase.GetAssetPath(imgDirectory);
            string[] names = Directory.GetFiles(path, "*.png");
            EditorGUILayout.BeginVertical();
            foreach(string d in names)
            {
                if (x > maxW)
                {
                    x = 0.0f;
                    y += h;
                    EditorGUILayout.EndHorizontal();
                }

                if(x == 0.0f)
                {
                    EditorGUILayout.BeginHorizontal();
                }

                GUILayout.FlexibleSpace();
                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(d, typeof(Texture2D));
                if (GUILayout.Button(tex, GUILayout.MaxWidth(w), GUILayout.MaxHeight(h), GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    selectedImagePath = d;
                }
                GUILayout.FlexibleSpace();
                x += w;
            }
            EditorGUILayout.EndVertical();
        }
    }

    //�I�������摜�f�[�^��\��
    private void DrawSelectedImage()
    {
        if (selectedImagePath != null)
        {
            Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(selectedImagePath, typeof(Texture2D));
            EditorGUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("select:" + selectedImagePath);
            GUILayout.Box(tex);
            EditorGUILayout.EndVertical();
        }
    }

    //�}�b�v�E�B���h�E���J���{�^���𐶐�
    private void DrawMapWindowButton()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("open map editor"))
        {
            if(subWindow == null)
            {
                subWindow = MapEditorSubWindow.WillAppear(this);
            }
            else
            {
                subWindow.Focus();
            }
        }
        EditorGUILayout.EndVertical();
    }

    public string SelectedImagePath
    {
        get { return selectedImagePath; }
    }

    public int MapSize
    {
        get { return mapSize; }
    }

    public float GridSize
    {
        get { return gridSize; }
    }

    //�o�͐�p�X�𐶐�
    public string OutputFilePath()
    {
        string resultPath = "";
        if (outputDirectory != null)
        {
            resultPath = AssetDatabase.GetAssetPath(outputDirectory);
        }
        else
        {
            resultPath = Application.dataPath;
        }
        return resultPath + "/" + outputFileName + ".txt";
    }
}

//MapEditor SubWindow
public class MapEditorSubWindow : EditorWindow
{
    //�}�b�v�E�B���h�E�̃T�C�Y
    const float WINDOW_W = 750.0f;
    const float WINDOW_H = 750.0f;
    //�}�b�v�̃O���b�h��
    private int mapSize = 0;
    //�O���b�h�T�C�Y
    private float gridSize = 0.0f;
    //�}�b�v�f�[�^
    private string[,] map;
    //�O���b�h�̎l�p
    private Rect[,] GetRect;
    //�e�E�B���h�E�̎Q��
    private MapEditor parent;

    //�T�u�E�B���h�E���J��
    public static MapEditorSubWindow WillAppear(MapEditor _parent)
    {
        MapEditorSubWindow window = (MapEditorSubWindow)EditorWindow.GetWindow(typeof(MapEditorSubWindow), false);
        window.Show();
        window.minSize = new Vector2(WINDOW_W, WINDOW_H);
        window.SetParent(_parent);
        window.init();
        return window;
    }

    private void SetParent(MapEditor _parent)
    {
        parent = _parent;
    }
}

