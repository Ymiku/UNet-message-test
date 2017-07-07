using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
/// <summary>
/// 客户端初始化
/// </summary>
public class Client : NetworkManager {
	
	public void Start()
	{
		//尝试连接服务器，但不马上加入游戏，等待连接完成
		NetworkManager.singleton.StartClient();
	}
	public override void OnClientConnect (NetworkConnection conn)
	{
		NetworkManager.singleton.client.RegisterHandler(MyMsgType.NetMessage, ClientHandlerCenter.Instance.MessageReceive);
		Debug.Log ("client connected");

	}
	public override void OnClientDisconnect (NetworkConnection conn)
	{
		Debug.Log ("client disconnected");
	}
}
