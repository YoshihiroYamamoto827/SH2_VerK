using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using System.Threading.Tasks;



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
        public Vector3 BasePos;
    }

    private Quaternion[] BoneQua;
    private GameObject TargetObj;
    private WebSocket ws;
    private QuaAr quaar;

    bool wsopen;
    int i, j;

    //public AvatarTracker ATscript;
    public TestTracker ATscript;

    private void Start()
    {
        //サーバへ送信する用のJSONデータの形式と回転を格納する配列を宣言
        quaar = new QuaAr();
        quaar.qualist = new QuaList[Define.BoneNum];
        for(i = 0; i < Define.BoneNum; i++) quaar.qualist[i] = new QuaList();
        BoneQua = new Quaternion[Define.BoneNum];
        //ATscript = GameObject.Find("SimpleSkeleton").GetComponent<AvatarTracker>();

        wsopen = false;

        var SendServer = Task.Run(() =>
        {
            SyncStart();

            while (true)
            {
                BoneQua = ATscript.SendBoneQua();
                quaar.BasePos = ATscript.SendBasePos();
                for (i = 0; i < Define.BoneNum; i++)
                {
                    quaar.qualist[i].X = double.Parse((BoneQua[i].x).ToString("f3"));
                    quaar.qualist[i].Y = double.Parse((BoneQua[i].y).ToString("f3"));
                    quaar.qualist[i].Z = double.Parse((BoneQua[i].z).ToString("f3"));
                    quaar.qualist[i].W = double.Parse((BoneQua[i].w).ToString("f3"));
                    //Debug.Log(quaar.qualist[i].X);
                    Debug.Log(quaar.BasePos);
                }

                var json = JsonUtility.ToJson(quaar);

                ws.Send(json);
            }
        });
    }

    private void SyncStart()
    {
        ws = new WebSocket("ws://localhost:3000/");

        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("Open");
        };

        ws.OnMessage += (sender, e) =>
        {
            
        };

        ws.Connect();
        wsopen = true;
    }

    private void OnDestroy()
    {
        ws.Close();
    }
}
