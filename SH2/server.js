//Server側

//ライブラリ
const { response } = require('express');

//クライアント情報を格納する配列の宣言
const clients ={};

//WebSocketサーバの定義
var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8080
});

//サーバに接続されたときに実行する処理
wss.on('connection', function connection(ws) {

    //クライアントIDを生成
    const clientId = Math.random().toString(36).substring(2);

    //クライアントをクライアントリストに追加
    clients[clientId] = ws;

    //クライアントIDをクライアントに返す
    console.log('ClientID:' + clientId);
    ws.send("Your ClientID:" + clientId);

    //クライアントからサーバにデータが送られた時の処理
    ws.on('message', function incoming(data) {

        //送信されたデータをJSON形式に変換して表示
        var JSONarr = JSON.parse(data);
        console.log(JSONarr.qualist);

        for(const otherClientId in clients){
            if(otherClientId !== clientId){
                
                clients[otherClientId].send(data.toString());
            }
        }
    });

ws.on('error', function error(error) {
    console.error(error);
  });

  ws.on('close', function close() {
    // remove the client from the list of clients
    delete clients[clientId];
  });
});