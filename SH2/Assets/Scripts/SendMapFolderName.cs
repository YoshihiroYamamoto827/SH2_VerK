using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SendMapFolderName : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Dropdown MCDropdown;
        List<string> MapList = new List<string>();

        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Map");
        FileInfo[] info = dir.GetFiles("*.");
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);

            if(f.Name!="map")MapList.Add(f.Name);
        }

        //�h���b�v�_�E���̃R���|�[�l���g�擾�ƕ\�����郊�X�g�̐錾
        MCDropdown = GameObject.Find("MapChooseDropdown").GetComponent<Dropdown>();

        //�h���b�v�_�E����Options���N���A
        MCDropdown.ClearOptions();

        //���X�g���h���b�v�_�E���ɒǉ�
        MCDropdown.AddOptions(MapList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
