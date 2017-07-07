using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
public class MyMsgType {  
	public static short NetMessage = 1234;  
};  
/// <summary>
/// 定点vectors
/// </summary>
public struct DVector3
{
	public static DVector3 zero = new DVector3(0m,0m,0m);
	public static DVector3 one = new DVector3(1m,1m,1m);
	public decimal x;
	public decimal y;
	public decimal z;
	public DVector3(decimal x,decimal y,decimal z)
	{
		this.x = x;
		this.y = y;
		this.z = z;
	}
	public static DVector3 operator +(DVector3 lhs,DVector3 rhs)
	{
		return new DVector3 (lhs.x+rhs.x,lhs.y+rhs.y,lhs.z+rhs.z);
	}
}
public class NetMessage : MessageBase {
	public int command;
	public int clientID;
	public DVector3 v;//浮点数存在不同平台精度问题，自定义向量类型，使用定点数
}
