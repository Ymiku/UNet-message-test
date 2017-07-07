using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using Protocal;
/// <summary>
/// 客户端消息处理
/// </summary>
public class ClientHandlerCenter : Singleton<ClientHandlerCenter> {
	public Player player;
	private Dictionary<int,Player> _connToPlayerDic = new Dictionary<int,Player>();

	public void InputSend(Vector3 i)
	{
		Debug.Log ("send input");
		NetworkManager.singleton.client.Send (MyMsgType.NetMessage,new NetMessage()
			{
				command = CommandProtocal.PLAYER_MOVE,
				clientID = player.id,
				v=new DVector3((decimal)i.x,(decimal)i.y,(decimal)i.z)
			});
	}
	//客户端消息处理
	public void MessageReceive(NetworkMessage netMsg)  
	{  
		NetMessage msg = netMsg.ReadMessage<NetMessage>();  
		switch (msg.command) {
		case CommandProtocal.PLAYER_ENTER_BRO:
			Player p = GameObject.Instantiate (Resources.Load ("Player") as GameObject).GetComponent<Player> ();
			p.transform.position = new Vector3 ((float)msg.v.x, (float)msg.v.y, (float)msg.v.z);
			p.pos = msg.v;
			p.isLocal = false;
			p.id = msg.clientID;
			_connToPlayerDic.Add (msg.clientID,p);
			break;

		case CommandProtocal.PLAYER_ENTER_CREQ:
			player = GameObject.Instantiate (Resources.Load ("Player") as GameObject).GetComponent<Player> ();
			player.id = msg.clientID;
			player.isLocal = true;
			_connToPlayerDic.Add (msg.clientID,player);

			break;
		case CommandProtocal.PLAYER_MOVE_BRO:
			if (_connToPlayerDic.ContainsKey (msg.clientID)) {
				_connToPlayerDic [msg.clientID].UpdatePos (msg.v); 
			} 
			break;
		case CommandProtocal.PLAYER_LEAVE:
			GameObject t = _connToPlayerDic[msg.clientID].gameObject;
			_connToPlayerDic.Remove(msg.clientID);
			GameObject.Destroy(t);
			break;
		}
	}  
}
