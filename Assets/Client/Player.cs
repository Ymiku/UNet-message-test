using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{
	public int id = -1;
	public bool isLocal;
	public DVector3 pos = new DVector3 (0m,0m,0m);//浮点数平台差异，用定点
	// Use this for initialization
	// Update is called once per frame
	void Update () {
		Lerp ();
		//判断是不是本地玩家（自己只能控制自己的玩家，不能控制别人的玩家）  
		if (!isLocal) {  
			return;  
		}  
		//获取键盘横轴的值  
		float h = Input.GetAxis ("Horizontal");  
		//获取键盘纵轴的值  
		float v = Input.GetAxis ("Vertical");  
		transform.position += (Vector3.right * h * 4 * Time.deltaTime + Vector3.forward * v * 3 * Time.deltaTime);//先实施位移，如果存在延迟会在lerp中拉回
		ClientHandlerCenter.Instance.InputSend (Vector3.right*h*4+Vector3.forward*v*3);

	} 

	public void Lerp()
	{
		transform.position = Vector3.Lerp(transform.position,new Vector3 ((float)pos.x,(float)pos.y,(float)pos.z),8f*Time.deltaTime);
	}
	public void UpdatePos(DVector3 offset)
	{
		pos = new DVector3 (pos.x+offset.x,pos.y+offset.y,pos.z+offset.z);
	}
}
