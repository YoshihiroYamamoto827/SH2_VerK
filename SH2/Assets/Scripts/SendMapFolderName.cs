using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System.IO;

public class SendMapFolderName : MonoBehaviour
{
    GameObject DeveloperPanel;
    TMP_Dropdown MCDropdown;
    public static string MapFolderName;

    // Start is called before the first frame update
    void Start()
    {
        //�J���Ґݒ�̃p�l���̎擾
        DeveloperPanel = GameObject.Find("DeveloperPanel");

        List<string> MapList = new List<string>();

        DirectoryInfo dir = new DirectoryInfo(Application.streamingAssetsPath + "/Map");
        FileInfo[] info = dir.GetFiles("*.");
        foreach (FileInfo f in info)
        {
            Debug.Log(f.Name);
            if(f.Name!="Map")MapList.Add(f.Name);
        }

        //�h���b�v�_�E���̃R���|�[�l���g�擾�ƕ\�����郊�X�g�̐錾
        MCDropdown = GameObject.Find("MapChooseDropdown").GetComponent<TMP_Dropdown>();
        Debug.Log(MCDropdown);

        //�h���b�v�_�E����Options���N���A
        MCDropdown.ClearOptions();

        //���X�g���h���b�v�_�E���ɒǉ�
        MCDropdown.AddOptions(MapList);

        MCDropdown.options[MCDropdown.value].text = MapList[0];

        DeveloperPanel.SetActive(false);
    }

    public void OnSelected()
    {
        MapFolderName = MCDropdown.options[MCDropdown.value].text;
        Debug.Log(MapFolderName);
    }

    public static string getMapFolderName()
    {
        return MapFolderName;
    }
}
