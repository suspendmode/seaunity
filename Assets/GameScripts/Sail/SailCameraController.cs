//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Action Base
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using System;

public class SailCameraController : AIControllerBase
{
	public override void DisableController()
	{
		base.DisableController();
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = false;
	}
	
	public override void EnableController()
	{
		base.EnableController();
		ControllerEnabled = true;
		Camera camera = gameObject.GetComponent<Camera>();
		camera.enabled = true;
	}
}