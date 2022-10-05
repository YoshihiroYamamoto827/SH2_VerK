using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;


//json�ɏo�͂���z��
[System.Serializable]
public class Jsondata
{
    public Mapdata[] mapdata; 
}

//json�f�[�^�̃t�H�[�}�b�g
[System.Serializable]
public class Mapdata
{
    public int xcoor;
    public int ycoor;
    public string objectname;
}

[System.Serializable]
public class MapInfo
{
    public int mapsize;
    public string date;
}


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
    private int gridSize = 50;
    //�o�̓t�H���_��
    private string outputFolderName;
    //�I�������摜�̃p�X
    private string selectedImagePath;
    //�T�u�E�B���h�E
    private MapEditorSubWindow subWindow;

    [UnityEditor.MenuItem("Window/MapEditor")]
    static void ShowTestMainWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
    }

    private void OnGUI()
    {
        //GUI��ŕ\��
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image Directory:", GUILayout.Width(150));
        imgDirectory = EditorGUILayout.ObjectField(imgDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Map Size:", GUILayout.Width(150));
        mapSize = EditorGUILayout.IntField(mapSize);
        if (mapSize > 100)
        {
            mapSize = 100;
        }
        else
        {
         if (mapSize < 5) mapSize = 5;
        }
        
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Grid Size:", GUILayout.Width(150));
        gridSize = EditorGUILayout.IntField(gridSize);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save Directory:", GUILayout.Width(150));
        outputDirectory = EditorGUILayout.ObjectField(outputDirectory, typeof(UnityEngine.Object), true);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Save DirectoryName:", GUILayout.Width(150));
        outputFolderName = (string)EditorGUILayout.TextField(outputFolderName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        DrawImageParts();

        DrawSelectedImage();

        DrawMapWindowButton();
    }

    //�摜�ꗗ���{�^���Ƃ��ďo��
    private void DrawImageParts()
    {
        if (imgDirectory != null)
        {
            float x = 0.0f;
            float y = 00.0f;
            float w = 50.0f;
            float h = 50.0f;
            float maxW = 300.0f;

            string path = AssetDatabase.GetAssetPath(imgDirectory);
            string[] names = Directory.GetFiles(path, "*.png");
            EditorGUILayout.BeginVertical();
            foreach (string d in names)
            {
                if (x > maxW)
                {
                    x = 0.0f;
                    y += h;
                    EditorGUILayout.EndHorizontal();
                }

                if (x == 0.0f)
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
        if (GUILayout.Button("open map editor"))
        {
            if (subWindow == null)
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

    public int GridSize
    {
        get { return gridSize; }
    }

    //�o�͐�p�X�𐶐�
    public string OutputFilePath()
    {
        string resultPath = "";
        if (outputDirectory != null)
        {
            if (System.IO.Directory.Exists(AssetDatabase.GetAssetPath(outputDirectory) + "/" + outputFolderName) == false)
            {
                Debug.Log("�쐬");
                System.IO.Directory.CreateDirectory(AssetDatabase.GetAssetPath(outputDirectory) + "/" + outputFolderName);
            }

            resultPath = AssetDatabase.GetAssetPath(outputDirectory);
        }
        else
        {
            resultPath = Application.dataPath;
        }
        return resultPath + "/" + outputFolderName;
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
    private int gridSize = 0;
    //�}�b�v�f�[�^
    private string[,] map;
    //�}�b�v�̃}�X��Texture
    private Texture _texture;
    //�}�b�v�\���G���A�̗]��
    private int Areamargin;
    //�}�X�̑傫��
    private Rect[] MapRects; 
    //�}�b�v�������̃}�X��x���W��y���W
    private int measureX, measureY;
    //�e�E�B���h�E�̎Q��
    private MapEditor parent;
    //�X�N���[���ʒu���L�^
    private Vector2 scrollPos = Vector2.zero;
    //�X�N���[���̒��x�����m���邽�߂̕ϐ�
    private Vector2 scrollmonitor = Vector2.zero;

    Jsondata json = new Jsondata();
    MapInfo info = new MapInfo();


    //��������json�f�[�^�̕�����̒�`
    public string jsonstr;
    public string mapinfostr;

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

    //�T�u�E�B���h�E�̏�����
    public void init()
    {
        mapSize = parent.MapSize;
        Debug.Log(mapSize);
        gridSize = parent.GridSize;
        Areamargin = 10;

        json.mapdata = new Mapdata[mapSize * mapSize];

        //�}�b�v�f�[�^�A��������Json�f�[�^�̔z���������
        map = new string[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                json.mapdata[i * mapSize + j] = new Mapdata();
                map[i, j] = "";
            }
        }

        //Map�̃}�X��`�悷��Rects��Texture�̏�����
        MapRects = new Rect[mapSize * mapSize];
        var measureTexture = new Texture2D(1, 1);
        measureTexture.SetPixel(0, 0, Color.white);
        measureTexture.Apply();
        _texture = measureTexture;

        

    }

    void OnGUI()
    {
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
        {
            scrollPos = scrollView.scrollPosition;

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                        //�O���b�h����`�悷��
                        for (int yy = 0; yy < mapSize; yy++)
                        {
                            for (int xx = 0; xx < mapSize; xx++)
                            {
                                measureX = Areamargin * 3 + gridSize * xx;
                                measureY = Areamargin * 3 + gridSize * yy;
                                MapRects[(xx * mapSize) + yy] = new Rect(measureX, measureY, gridSize, gridSize);
                                GUI.DrawTexture(MapRects[(xx * mapSize) + yy], _texture, ScaleMode.StretchToFill, true, 0, Color.white, 3, 0);
                            }
                        }

                    //�N���b�N���ꂽ�ʒu��T���Ă��̏ꏊ�ɉ摜�f�[�^������
                    Event e = Event.current;
                    if (e.type == EventType.MouseDown)
                    {
                        Vector2 pos = Event.current.mousePosition;
                        int xx;
                        bool xmax = false;

                        //x�ʒu��T��
                        for (xx = 0; xx < (mapSize - 1); xx++)
                        {
                            if (MapRects[(xx * mapSize)].x <= pos.x && pos.x <= MapRects[((xx + 1) * mapSize)].x)
                            {

                                Debug.Log(xx);
                                break;
                            }

                            if (xx == mapSize - 2) xmax = true;
                        }

                        if (xmax && xx == (mapSize - 2)) xx = mapSize - 1;

                        //y�ʒu��T��
                        for (int yy = 0; yy < (mapSize - 1); yy++)
                        {
                            if (MapRects[yy].y <= pos.y && pos.y <= MapRects[(yy + 1)].y)
                            {
                                //�����S���̂Ƃ��̓f�[�^������
                                if (parent.SelectedImagePath.IndexOf("000") > -1)
                                {
                                    map[xx, yy] = "";
                                }
                                else
                                {
                                    map[xx, yy] = parent.SelectedImagePath;
                                }
                                Repaint();
                                break;
                            }

                            if (yy == mapSize - 2)
                            {
                                yy = mapSize - 1;
                                if (parent.SelectedImagePath.IndexOf("000") > -1)
                                {
                                    map[xx, yy] = "";
                                }
                                else
                                {
                                    map[xx, yy] = parent.SelectedImagePath;
                                }
                                Repaint();
                                break;
                            }
                        }
                    }

                    //�I�������摜��`�悷��
                    for (int yy = 0; yy < mapSize; yy++)
                    {
                        for (int xx = 0; xx < mapSize; xx++)
                        {
                            if (map[xx, yy] != null && map[xx, yy].Length > 0)
                            {
                                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(map[xx, yy], typeof(Texture2D));
                                GUI.DrawTexture(MapRects[(xx * mapSize) + yy], tex);
                            }
                        }
                    }

                    //Ctrl + �X�N���[���Ń}�X�̃T�C�Y���g��A�k��
                    if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftControl)
                    {
                        if (scrollPos.y > scrollmonitor.y)
                        {
                            gridSize -= 5;
                            Repaint();
                            scrollmonitor = scrollPos;
                        }

                        if (scrollPos.y < scrollmonitor.y)
                        {
                            gridSize += 5;
                            Repaint();
                            scrollmonitor = scrollPos;
                        }
                    }


                    GUILayout.Space(measureY);
                        
                    //�o�̓{�^��
                    Rect rect = new Rect(0, position.size.y - 50, 300, 50);
                    GUILayout.BeginArea(rect);
                    if (GUILayout.Button("output file", GUILayout.MinWidth(300), GUILayout.MinHeight(50)))
                    {
                        OutputFile();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();

                }

                GUILayout.Space(measureX);
            } 
        } 
    }

    //�t�@�C���ŏo��
    private void OutputFile()
    {

        string folderpath = parent.OutputFilePath();

        FileInfo MDfileInfo = new FileInfo(folderpath + "/" + "Mapdata.json");
        StreamWriter mdsw = MDfileInfo.AppendText();
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                GetMapStrFormat(i, j);
            }
        }
        mdsw.WriteLine(WriteJsonMapData());
        mdsw.Flush();
        mdsw.Close();

        FileInfo MIfileInfo = new FileInfo(folderpath + "/" + "Mapinfo.json");
        StreamWriter misw = MIfileInfo.AppendText();
        GetMapInfoFormat();
        misw.WriteLine(WriteJsonMapInfo());
        misw.Flush();
        misw.Close();

        //�����|�b�v�A�b�v
        EditorUtility.DisplayDialog("MapEditor", "output file success\n" + folderpath, "OK");
    }

    //�o�͂���}�b�v�f�[�^���`
    private void GetMapStrFormat(int x, int y)
    {
        json.mapdata[x * mapSize + y].xcoor = x;
        json.mapdata[x * mapSize + y].ycoor = y;
        json.mapdata[x * mapSize + y].objectname = OutputDataFormat(map[x, y]);
    }

    private void GetMapInfoFormat()
    {
        info.mapsize = mapSize;
        info.date = System.DateTime.Now.Date.ToString();
    }

    private string WriteJsonMapData()
    {
        jsonstr = JsonUtility.ToJson(json, true);
        return jsonstr;
    }

    private string WriteJsonMapInfo()
    {
        mapinfostr = JsonUtility.ToJson(info, true);
        return mapinfostr;
    }

    private string OutputDataFormat(string data)
    {
        if (data != null && data.Length > 0)
        {
            string[] tmps = data.Split('/');
            string fileName = tmps[tmps.Length - 1];
            return fileName.Split('/')[0];
        }
        else
        {
            return "";
        }
    }
}

//MapEditor SubWindow2
public class MapEditorSubWindow2 : EditorWindow
{
    //�}�b�v�E�B���h�E�̃T�C�Y
    const float WINDOW_W = 750.0f;
    const float WINDOW_H = 750.0f;
    //�}�b�v�̃O���b�h��
    private int mapSize = 0;
    //�O���b�h�T�C�Y
    private int gridSize = 0;
    //�}�b�v�f�[�^
    private string[,] map;
    //�}�b�v�̃}�X��Texture
    private Texture _texture;
    //�}�b�v�\���G���A�̗]��
    private int Areamargin;
    //�}�X�̑傫��
    private Rect[] MapRects;
    //�}�b�v�������̃}�X��x���W��y���W
    private int measureX, measureY;
    //�e�E�B���h�E�̎Q��
    private MapEditor parent;
    //�X�N���[���ʒu���L�^
    private Vector2 scrollPos = Vector2.zero;
    //�X�N���[���̒��x�����m���邽�߂̕ϐ�
    private Vector2 scrollmonitor = Vector2.zero;

    Jsondata json = new Jsondata();
    MapInfo info = new MapInfo();


    //��������json�f�[�^�̕�����̒�`
    public string jsonstr;
    public string mapinfostr;

    //�T�u�E�B���h�E���J��
    public static MapEditorSubWindow WillAppear(MapEditor _parent)
    {
        MapEditorSubWindow window = (MapEditorSubWindow)EditorWindow.GetWindow(typeof(MapEditorSubWindow), false);
        window.Show();
        window.minSize = new Vector2(WINDOW_W, WINDOW_H);
        //window.SetParent(_parent);
        window.init();
        return window;
    }

    private void SetParent(MapEditor _parent)
    {
        parent = _parent;
    }

    //�T�u�E�B���h�E�̏�����
    public void init()
    {
        mapSize = parent.MapSize;
        Debug.Log(mapSize);
        gridSize = parent.GridSize;
        Areamargin = 10;

        json.mapdata = new Mapdata[mapSize * mapSize];

        //�}�b�v�f�[�^�A��������Json�f�[�^�̔z���������
        map = new string[mapSize, mapSize];
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                json.mapdata[i * mapSize + j] = new Mapdata();
                map[i, j] = "";
            }
        }

        //Map�̃}�X��`�悷��Rects��Texture�̏�����
        MapRects = new Rect[mapSize * mapSize];
        var measureTexture = new Texture2D(1, 1);
        measureTexture.SetPixel(0, 0, Color.white);
        measureTexture.Apply();
        _texture = measureTexture;



    }

    void OnGUI()
    {
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPos))
        {
            scrollPos = scrollView.scrollPosition;

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                    //�O���b�h����`�悷��
                    for (int yy = 0; yy < mapSize; yy++)
                    {
                        for (int xx = 0; xx < mapSize; xx++)
                        {
                            measureX = Areamargin * 3 + gridSize * xx;
                            measureY = Areamargin * 3 + gridSize * yy;
                            MapRects[(xx * mapSize) + yy] = new Rect(measureX, measureY, gridSize, gridSize);
                            GUI.DrawTexture(MapRects[(xx * mapSize) + yy], _texture, ScaleMode.StretchToFill, true, 0, Color.white, 3, 0);
                        }
                    }

                    //�N���b�N���ꂽ�ʒu��T���Ă��̏ꏊ�ɉ摜�f�[�^������
                    Event e = Event.current;
                    if (e.type == EventType.MouseDown)
                    {
                        Vector2 pos = Event.current.mousePosition;
                        int xx;
                        bool xmax = false;

                        //x�ʒu��T��
                        for (xx = 0; xx < (mapSize - 1); xx++)
                        {
                            if (MapRects[(xx * mapSize)].x <= pos.x && pos.x <= MapRects[((xx + 1) * mapSize)].x)
                            {

                                Debug.Log(xx);
                                break;
                            }

                            if (xx == mapSize - 2) xmax = true;
                        }

                        if (xmax && xx == (mapSize - 2)) xx = mapSize - 1;

                        //y�ʒu��T��
                        for (int yy = 0; yy < (mapSize - 1); yy++)
                        {
                            if (MapRects[yy].y <= pos.y && pos.y <= MapRects[(yy + 1)].y)
                            {
                                //�����S���̂Ƃ��̓f�[�^������
                                if (parent.SelectedImagePath.IndexOf("000") > -1)
                                {
                                    map[xx, yy] = "";
                                }
                                else
                                {
                                    map[xx, yy] = parent.SelectedImagePath;
                                }
                                Repaint();
                                break;
                            }

                            if (yy == mapSize - 2)
                            {
                                yy = mapSize - 1;
                                if (parent.SelectedImagePath.IndexOf("000") > -1)
                                {
                                    map[xx, yy] = "";
                                }
                                else
                                {
                                    map[xx, yy] = parent.SelectedImagePath;
                                }
                                Repaint();
                                break;
                            }
                        }
                    }

                    //�I�������摜��`�悷��
                    for (int yy = 0; yy < mapSize; yy++)
                    {
                        for (int xx = 0; xx < mapSize; xx++)
                        {
                            if (map[xx, yy] != null && map[xx, yy].Length > 0)
                            {
                                Texture2D tex = (Texture2D)AssetDatabase.LoadAssetAtPath(map[xx, yy], typeof(Texture2D));
                                GUI.DrawTexture(MapRects[(xx * mapSize) + yy], tex);
                            }
                        }
                    }

                    //Ctrl + �X�N���[���Ń}�X�̃T�C�Y���g��A�k��
                    if (e.type == EventType.KeyDown && e.keyCode == KeyCode.LeftControl)
                    {
                        if (scrollPos.y > scrollmonitor.y)
                        {
                            gridSize -= 5;
                            Repaint();
                            scrollmonitor = scrollPos;
                        }

                        if (scrollPos.y < scrollmonitor.y)
                        {
                            gridSize += 5;
                            Repaint();
                            scrollmonitor = scrollPos;
                        }
                    }


                    GUILayout.Space(measureY);

                    //�o�̓{�^��
                    Rect rect = new Rect(0, position.size.y - 50, 300, 50);
                    GUILayout.BeginArea(rect);
                    if (GUILayout.Button("output file", GUILayout.MinWidth(300), GUILayout.MinHeight(50)))
                    {
                        OutputFile();
                    }
                    GUILayout.FlexibleSpace();
                    GUILayout.EndArea();

                }

                GUILayout.Space(measureX);
            }
        }
    }

    //�t�@�C���ŏo��
    private void OutputFile()
    {

        string folderpath = parent.OutputFilePath();

        FileInfo MDfileInfo = new FileInfo(folderpath + "/" + "Mapdata.json");
        StreamWriter mdsw = MDfileInfo.AppendText();
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                GetMapStrFormat(i, j);
            }
        }
        mdsw.WriteLine(WriteJsonMapData());
        mdsw.Flush();
        mdsw.Close();

        FileInfo MIfileInfo = new FileInfo(folderpath + "/" + "Mapinfo.json");
        StreamWriter misw = MIfileInfo.AppendText();
        GetMapInfoFormat();
        misw.WriteLine(WriteJsonMapInfo());
        misw.Flush();
        misw.Close();

        //�����|�b�v�A�b�v
        EditorUtility.DisplayDialog("MapEditor", "output file success\n" + folderpath, "OK");
    }

    //�o�͂���}�b�v�f�[�^���`
    private void GetMapStrFormat(int x, int y)
    {
        json.mapdata[x * mapSize + y].xcoor = x;
        json.mapdata[x * mapSize + y].ycoor = y;
        json.mapdata[x * mapSize + y].objectname = OutputDataFormat(map[x, y]);
    }

    private void GetMapInfoFormat()
    {
        info.mapsize = mapSize;
        info.date = System.DateTime.Now.Date.ToString();
    }

    private string WriteJsonMapData()
    {
        jsonstr = JsonUtility.ToJson(json, true);
        return jsonstr;
    }

    private string WriteJsonMapInfo()
    {
        mapinfostr = JsonUtility.ToJson(info, true);
        return mapinfostr;
    }

    private string OutputDataFormat(string data)
    {
        if (data != null && data.Length > 0)
        {
            string[] tmps = data.Split('/');
            string fileName = tmps[tmps.Length - 1];
            return fileName.Split('/')[0];
        }
        else
        {
            return "";
        }
    }
}


