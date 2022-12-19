using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;



public class SyncTest : MonoBehaviour
{
    public static class Define
    {
        public static readonly int BoneNum = 32;
    }

    [System.Serializable]
    public class QuaList
    {
        public double X;
        public double Y;
        public double Z;
        public double W;
    }

    [System.Serializable]
    public class QuaAr
    {
        public QuaList[] qualist;
    }

    private Quaternion[] BoneQua;
    private GameObject TargetObj;
    private WebSocket ws;
    private QuaAr quaar;

    private Vector3 ObjPosB, ObjPosA;
    bool wsopen;
    int i, j;

    AvatarTracker ATscript;

    private void Start()
    {
        //サーバへ送信する用のJSONデータの形式と回転を格納する配列を宣言
        quaar = new QuaAr();
        quaar.qualist = new QuaList[Define.BoneNum];
        for(i = 0; i < Define.BoneNum; i++) quaar.qualist[i] = new QuaList();
        BoneQua = new Quaternion[Define.BoneNum];
        ATscript = GameObject.Find("SimpleSkeleton").GetComponent<AvatarTracker>();

        wsopen = false;
    }

    private void Update()
    {
        if (wsopen) SendServer();
    }

    public void SyncStart()
    {
        ws = new WebSocket("ws://localhost:8080/ws");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log(e.Data);
        };

        ws.Connect();
        wsopen = true;
    }

    public void SendServer()
    {
        BoneQua = ATscript.SendBoneQua();
        for (i = 0; i < Define.BoneNum; i++)
        {
            quaar.qualist[i].X = double.Parse((BoneQua[i].x).ToString("f3"));
            quaar.qualist[i].Y = double.Parse((BoneQua[i].y).ToString("f3"));
            quaar.qualist[i].Z = double.Parse((BoneQua[i].z).ToString("f3"));
            quaar.qualist[i].W = double.Parse((BoneQua[i].w).ToString("f3"));
        }

        var json = JsonUtility.ToJson(quaar);

        ws.Send(json);
    }

    private void OnDestroy()
    {
        ws.Close();
    }

    private double adjustdigit(double qua1)
    {
        double qua2 = double.Parse(qua1.ToString("f3"));
        return qua2;
    }
}
