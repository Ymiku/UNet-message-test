using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
/// <summary>
/// 服务器初始化
/// </summary>
public class Server : NetworkManager {
	public void Start()
	{
		
		//开启服务器监听
		NetworkManager.singleton.StartServer();
		NetworkServer.RegisterHandler(MyMsgType.NetMessage, ServerHandlerCenter.Instance.MessageReceive);
		InvokeRepeating ("Sync",1f,0.04f);//每秒同步30次
	}
	/// <summary>
	/// 同步，群发速度信息.
	/// </summary>
	public void Sync()
	{
		ServerHandlerCenter.Instance.Sync ();
	}
	/// <summary>
	/// calculate new position by speed and old pos.
	/// </summary>
	public void Update()
	{
		ServerHandlerCenter.Instance.Execute ();
	}
	public override void OnServerConnect (NetworkConnection conn)
	{
		Debug.Log ("client connected"+conn.connectionId);
		ServerHandlerCenter.Instance.ClientConnect (conn.connectionId);
	}
	public override void OnServerDisconnect (NetworkConnection conn)
	{
		Debug.Log ("client disconnected");
		ServerHandlerCenter.Instance.ClientDisconnect (conn.connectionId);
	}
}
