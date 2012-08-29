//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Action Base
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using System;
using System.Collections.Generic;

public class SailCameraController : AIControllerBase
{
	public override void DisableController()
	{
		base.DisableController();
		BroadcastMessage("DisableController",SendMessageOptions.DontRequireReceiver);
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = false;
	}
	
	public override void EnableController()
	{
		base.EnableController();
		Transform tf = transform.FindChild("Steer");
		if( tf != null )
		{
			if( tf.gameObject != null ) {
				GlobalMethods.SendMessage(tf.gameObject, "EnableController");
			}
		}
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = true;
	}
}