using System.Collections;
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
    //�}�b�v�������ɑΏۂƂȂ�prefab���w���ϐ�
    private GameObject InstanceObject;
    //�}�b�v��������y���W��������ϐ�
    private float y;
    //�}�b�v�������Ɋe�I�u�W�F�N�g�̐e�ƂȂ��I�u�W�F�N�g���w���ϐ�
    private GameObject Parent;
    //�}�b�v�������ɐ������ꂽ�I�u�W�F�N�g���w���ϐ�
    private GameObject Instanced;

    private void Start()
    {
        init();

    }

    private void init()
    {
        y = 0;
        MapInfo info = new MapInfo();
    }

    public void MapGenerate(string JsonFileName)
    {
        //�A�Z�b�g�o���h���̐錾�Ə�����
        AssetBundle JsonAssetBundle=null, ObjectAssetBundle = null, ManagerAssetBundle = null, ParentAssetBundle = null;

        //GameObject�̐錾
        GameObject Wall, Door, Floor, Item, Player, SceneManager, ItemManager, WallParent, DoorParent, FloorParent, ItemParent;
        //Json�t�@�C����AssetBundle��AssetBundle���̑Ή�����Json�t�@�C���̓ǂݍ���
        if (JsonAssetBundle != null)JsonAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + MapFolderName);
        string mapdatainputString = JsonAssetBundle.LoadAsset<TextAsset>("Assets/MapData/" + MapFolderName + "/Mapdata.json").ToString();
        string mapinfoinputString = JsonAssetBundle.LoadAsset<TextAsset>("Assets/MapData/" + MapFolderName + "/MapInfo.json").ToString();

        //�}�b�v�T�C�Y�����N���X�ϐ��ɑ��
        MapInfo inputjson2 = JsonUtility.FromJson<MapInfo>(mapinfoinputString);

        //�}�b�v�f�[�^�̃N���X���������A�}�b�v�T�C�Y�ϐ��Ŕz���錾
        jsondata = new Jsondata();
        mapSize = inputjson2.mapsize;
        jsondata.mapdata = new Mapdata[mapSize];

        //�}�b�v��json�t�@�C���̃f�[�^���N���X�ϐ��ɑ��
        Jsondata inputjson = JsonUtility.FromJson<Jsondata>(mapdatainputString);

        //Debug.Log(inputjson.mapdata[0].xcoor);

        //�I�u�W�F�N�g�AMagager�A�N���[�����܂Ƃ߂�p�̋�̃I�u�W�F�N�g��AssetBundle�̓ǂݍ���
        if (ObjectAssetBundle != null) ObjectAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/objects");
        if (ManagerAssetBundle != null) ManagerAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/manager");
        if (ParentAssetBundle != null) ParentAssetBundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/parents");

        Wall = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Wall.prefab");
        Door = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Door.prefab");
        Floor = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Floor.prefab");
        Item = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Item.prefab");
        Player = ObjectAssetBundle.LoadAsset<GameObject>("Assets/Resources/Objects/Player.prefab");
        SceneManager = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Manager/SceneManager.prefab");
        ItemManager = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Manager/ItemManager.prefab");
        WallParent = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Parents/Wall.prefab");
        DoorParent = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Parents/Door.prefab");
        FloorParent = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Parents/Floor.prefab");
        ItemParent = ManagerAssetBundle.LoadAsset<GameObject>("Assets/Resources/Parents/Item.prefab");

        for (int i = 0; i < mapSize * mapSize; i++)
        {
            Instantiate(Floor, new Vector3(inputjson.mapdata[i].xcoor, y, inputjson.mapdata[i].ycoor), Quaternion.identity);

            if (inputjson.mapdata[i].objectname != null)
            {
                switch (inputjson.mapdata[i].objectname)
                {
                    case "Capture\\001.png":
                        InstanceObject = Wall;
                        y = 2f;
                        Parent = WallParent;
                        break;

                    case "Capture\\002.png":
                        InstanceObject = Door;
                        y = 1.5f;
                        Parent = DoorParent;
                        break;

                    case "Capture\\003.png":
                        InstanceObject = Wall;
                        y = 1f;
                        Parent = WallParent;
                        break;

                    case "Capture\\004.png":
                        InstanceObject = Wall;
                        y = 1f;
                        Parent = WallParent;
                        break;

                    case "Capture\\005.png":
                        InstanceObject = Item;
                        y = 1.5f;
                        Parent = ItemParent;
                        break;

                    case "Capture\\006.png":
                        InstanceObject = Player;
                        y = 1.5f;
                        Parent = null;
                        break;
                }
               Instanced = Instantiate(InstanceObject, new Vector3(inputjson.mapdata[i].xcoor, y, inputjson.mapdata[i].ycoor), Quaternion.identity);
                //if (Parent != null) Instanced.transform.parent = Parent.transform;
            }
        }

        Instantiate(SceneManager, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Instantiate(ItemManager, new Vector3(0f, 0f, 0f), Quaternion.identity);
    }
}