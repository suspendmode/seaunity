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

public class BattleAIController : AIControllerBase
{
	public float ForwardPower;
	public float SteerPower;
	
	private GameObject mPlayerShip = null;
	private Transform mTargetTransform = null;
	
	public void SetPlayer(GameObject player)
	{
		mPlayerShip = player;
		
	}
	
	void LateUpdate()
	{
		if( mPlayerShip == null ) return;
		if( mTargetTransform == null ) {
			mTargetTransform = GetTargetPosition();
		}
		else {
			if( transform.position == mTargetTransform.position ) {
				GetNextTargetPosition();
			}
			
		}
	}
	
	private Transform GetTargetPosition()
	{
		return null;
	}
	
	private Transform GetNextTargetPosition()
	{
		return null;
	}
	
	
}