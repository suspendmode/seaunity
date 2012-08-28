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

public class GlobalModuleController : AIControllerBase
{
	
	public GameObject [] SailModule;
	public GameObject [] BattleModule;
	
	private static GameObject mControllerGameObject;
	public static GameObject MessageTarget { get { return mControllerGameObject; } }
	
	void Awake()
	{
		mControllerGameObject = gameObject;
	}
	public void Sail(bool enable)
	{
		if( enable ) {
			foreach( GameObject go in SailModule)
			{
				go.SetActiveRecursively(true);
				//GlobalMethods.SendMessage( go, "EnableController");
			}
		}
		else {
			foreach( GameObject go in SailModule)
			{
				go.SetActiveRecursively(false);
				//GlobalMethods.SendMessage( go, "DisableController");
			}
		}
	}
	
	public void Battle(bool enable)
	{
		if( enable ) {
			foreach( GameObject go in BattleModule)
			{
				go.SetActiveRecursively(true);
				GlobalMethods.SendMessage( go, "EnableController");
			}
		}
		else {
			foreach( GameObject go in BattleModule)
			{
				go.SetActiveRecursively(false);
				GlobalMethods.SendMessage( go, "DisableController");
			}
		}
	}
}

