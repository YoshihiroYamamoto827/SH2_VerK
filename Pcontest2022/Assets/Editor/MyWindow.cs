using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyWindow : EditorWindow
{
    //�E�B���h�E��\������T���v��
    [MenuItem("Window/MyWindow")]
    static void Open()
    {
        var window = GetWindow<MyWindow>();
        window.titleContent = new GUIContent("�I���W�i���̃E�B���h�E");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("�I���W�i���̃E�B���h�E����낤");
            EditorGUILayout.LabelField("EditorGUILayout.BeginVertical���g���Əc�ɕ��т܂�");
            EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("EditorGUILayout.BeginVertical���g����");
                EditorGUILayout.LabelField("���ɕ��ׂ邱�Ƃ��ł��܂�");
            EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("������̓X�^�C���Ȃ��o�[�W����");
            EditorGUILayout.LabelField("���肪�͂��Ă��܂���");
        EditorGUILayout.EndVertical();
    }
}
