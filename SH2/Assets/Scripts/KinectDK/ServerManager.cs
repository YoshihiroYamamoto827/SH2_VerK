using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class ServerManager : MonoBehaviour
{
    private WebSocketServer server;


    // Start is called before the first frame update
    void Start()
    {
        
        server = new WebSocketServer(3000);

        server.AddWebSocketService<Echo>("/");
        server.Start();
        Debug.Log("サーバ起動");
    }

    private void OnDestroy()
    {
        server.Stop();
        server = null;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class Echo : WebSocketBehavior
    {
        //サーバに接続中のクライアントを管理するリスト
        public static List<WebSocketBehavior> clientList = new List<WebSocketBehavior>();
        //クライアントの番号振りのための変数
        static int globalSeq = 0;
        //クライアント自身の番号
        int seq;

        protected override void OnOpen()
        {
            globalSeq++;
            this.seq = globalSeq;
            clientList.Add(this);
            Debug.Log("Seq:" + seq + " Login.");
            this.Send("Login Success!");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            var JSONarr = JsonUtility.ToJson(e);
            //Debug.Log(e.Data);
            Sessions.Broadcast(e.Data);
        }
    }
}
