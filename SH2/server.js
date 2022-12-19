//Server側

//ライブラリ
const { response } = require('express');
const crypto = require("crypto");

//WebSocketサーバの定義
var WebSocketServer = require('ws').Server
var wss = new WebSocketServer({
    port: 8080
});

//ユーザIDを格納するためのMapを定義
const map = new Map();

wss.broadcast = function (data) {
    for (var i in this.clients) {
      this.clients[i].send(data)
    }
}

//サーバに接続されたときに実行する処理
wss.on('connection', (ws) => {

    //ユーザIDをMapに追加
    const id = crypto.randomUUID();
    map.set(id,ws);

    //ユーザIDをクライアントに返す
    console.log('UserID:' + id);
    ws.send("Your UserID" + id);

    //クライアントからサーバにデータが送られた時の処理
    ws.on('message', function(receiveddata) {

        //送信されたデータをJSON形式に変換して表示
        var JSONarr = JSON.parse(receiveddata);
        console.log(JSONarr.qualist);

        wss.broadcast(JSON.stringify(receiveddata));
    });
});

function getSocketByid(id){
    return map.get(id);
}