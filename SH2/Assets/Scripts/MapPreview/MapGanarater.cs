/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

//MapGenerater
public class MapGanarater : MonoBehaviour
{
    //json�t�@�C��������͂����z��
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

    //AssetBundle�t�@�C���̎w�� 
    private string MapFolderName;
    //�}�b�v�̑傫��
    private int mapSize;

    //�}�b�v�f�[�^��������ϐ�
    private Jsondata jsondata;

    private void OnGUI()
    {
        init();
        GUILayout.BeginHorizontal();
        MapFolderName = EditorGUILayout.TextField("MapFolderName", MapFolderName);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Map") && MapFolderName != null)
        {
            OnPreviewButton(MapFolderName);
        }
    }

    private void init()
    {

        MapInfo info = new MapInfo();
    }

    public void OnPreviewButton(string JsonFileName)
    {
        //Json�t�@�C����AssetBundle��AssetBundle���̑Ή�����Json�t�@�C���̓ǂݍ���
        var JsonassetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + MapFolderName);
        string mapdatainputString = JsonassetBundle.LoadAsset<TextAsset>("Assets/MapData/" + MapFolderName + "/Mapdata.json").ToString();
        string mapinfoinputString = JsonassetBundle.LoadAsset<TextAsset>("Assets/MapData/" + MapFolderName + "/MapInfo.json").ToString();

        //�}�b�v�T�C�Y�����N���X�ϐ��ɑ��
        MapInfo inputjson2 = JsonUtility.FromJson<MapInfo>(mapinfoinputString);

        jsondata = new Jsondata();
        jsondata.mapdata = new Mapdata[mapSize];

        mapSize = inputjson2.mapsize;

        //�}�b�v��json�t�@�C���̃f�[�^���N���X�ϐ��ɑ��
        Jsondata inputjson = JsonUtility.FromJson<Jsondata>(mapdatainputString);

        Debug.Log(inputjson.mapdata[0].xcoor);

        //�I�u�W�F�N�g��prefab�������Ă���t�H���_��AssetBundle�̓ǂݍ��݁AAssetBundle������e�I�u�W�F�N�g�̕ϐ��ɑ��
        var ObjectAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/objects");
        var Wall = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Wall.prefab");
        var Door = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Door.prefab");
        var Floor = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Floor.prefab");
        var Item = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Item.prefab");
        var Player = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Player.prefab");

        for (int i = 0; i < mapSize; i++)
        {
            Instantiate(Floor, new Vector3(inputjson.mapdata[i].xcoor, 0f, inputjson.mapdata[i].ycoor), Quaternion.identity);

            if (inputjson.mapdata[i].objectname != null)
            {
                switch (inputjson.mapdata[i].objectname)
                {
                    case "Capture\\001.png":
                        Instantiate(Wall, new Vector3(inputjson.mapdata[i].xcoor, 2f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                        break;
                    case "Capture\\002.png":
                        Instantiate(Door, new Vector3(inputjson.mapdata[i].xcoor, 1.5f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                        break;
                    //case "Capture\\003.png":
                    //Instantiate(Wall, new Vector3(inputjson.mapdata[i].xcoor, 1f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                    //break;
                    //case "Capture\\004.png":
                    //Instantiate(Wall, new Vector3(inputjson.mapdata[i].xcoor, 1f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                    //break;
                    case "Capture\\005.png":
                        Instantiate(Item, new Vector3(inputjson.mapdata[i].xcoor, 1.5f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                        break;
                    case "Capture\\006.png":
                        Instantiate(Player, new Vector3(inputjson.mapdata[i].xcoor, 1.5f, inputjson.mapdata[i].ycoor), Quaternion.identity);
                        break;
                }
            }
        }
    }
}*/