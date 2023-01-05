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
        Debug.Log("�T�[�o�N��");
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
        //�T�[�o�ɐڑ����̃N���C�A���g���Ǘ����郊�X�g
        public static List<WebSocketBehavior> clientList = new List<WebSocketBehavior>();
        //�N���C�A���g�̔ԍ��U��̂��߂̕ϐ�
        static int globalSeq = 0;
        //�N���C�A���g���g�̔ԍ�
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
