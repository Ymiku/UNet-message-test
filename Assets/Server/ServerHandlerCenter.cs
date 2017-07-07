using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using Protocal;
/// <summary>
/// 服务端消息处理
/// </summary>
public class ServerHandlerCenter : Singleton<ServerHandlerCenter>{
	private Dictionary<int,PlayerModel> _connToPlayerDic = new Dictionary<int,PlayerModel>();
	public void ClientConnect(int connID)
	{
		if (_connToPlayerDic.ContainsKey (connID)) {
			return;
		}

		//使客户端知道自己的唯一id
		NetworkServer.SendToClient (connID,MyMsgType.NetMessage, new NetMessage {
			command = CommandProtocal.PLAYER_ENTER_CREQ,
			clientID = connID
		});
		Debug.Log ("发送唯一id到客户端");
		//同步新客户端和旧客户端的玩家字典
		foreach (int i in _connToPlayerDic.Keys) {
			NetworkServer.SendToClient (i,MyMsgType.NetMessage, new NetMessage {
				command = CommandProtocal.PLAYER_ENTER_BRO,
				clientID = connID,
				v = DVector3.zero
			});
			NetworkServer.SendToClient (connID,MyMsgType.NetMessage, new NetMessage {
				command = CommandProtocal.PLAYER_ENTER_BRO,
				clientID = i,
				v = _connToPlayerDic[i].pos
			});
		}
		//加入model字典
		_connToPlayerDic.Add(connID,new PlayerModel());
	}
	public void ClientDisconnect(int connID)
	{
		if (_connToPlayerDic.ContainsKey (connID)) {
			_connToPlayerDic.Remove (connID);
		}
		NetworkServer.SendToAll(MyMsgType.NetMessage, new NetMessage {
			command = CommandProtocal.PLAYER_LEAVE,
			clientID = connID
		});
	}
	/// <summary>
	/// 切帧，同步
	/// </summary>
	public void Sync()
	{
		foreach (int i in _connToPlayerDic.Keys) {
			NetworkServer.SendToAll (MyMsgType.NetMessage, new NetMessage {
				command = CommandProtocal.PLAYER_MOVE_BRO,
				clientID = i,
				v = _connToPlayerDic[i].offset
			});
			Debug.Log ("send move"+i);
			_connToPlayerDic [i].pos += _connToPlayerDic [i].offset;
			_connToPlayerDic [i].offset = DVector3.zero;
		}
	}
	/// <summary>
	/// 服务器端位移更新，根据输入更改位移，因为输入相对更新是低频的，所以可以直接使用上一次的速度
	/// </summary>
	public void Execute()
	{
		foreach (int i in _connToPlayerDic.Keys) {
			_connToPlayerDic [i].offset += new DVector3 (_connToPlayerDic [i].speed.x*(decimal)Time.deltaTime,
				_connToPlayerDic [i].speed.y*(decimal)Time.deltaTime,_connToPlayerDic [i].speed.z*(decimal)Time.deltaTime);
		}
	}
	//服务端消息处理
	public void MessageReceive(NetworkMessage netMsg)
	{
		NetMessage msg = netMsg.ReadMessage<NetMessage> ();  
		switch (msg.command) {
		case CommandProtocal.PLAYER_MOVE:
			_connToPlayerDic [msg.clientID].speed = msg.v;
			break;

		}  
	}
}
