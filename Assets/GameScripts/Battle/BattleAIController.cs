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
	
	public float MiniPassRange = 0.01f;
	
	private GameObject mPlayerShip = null;
	private Transform mTargetTransform = null;
	
	//Sail
	private float mCurrentForceTarget;
	private float mMaxForce;
	private float mCurrentForce;
	
	private AttackShipData mAttackShipData = new AttackShipData();
	public void SetPlayer(GameObject player)
	{
		mPlayerShip = player;
		mAttackShipData.FillTargetPositions(mPlayerShip);
		mTargetTransform = GetTargetPosition(mAttackShipData);
		if( mTargetTransform == null ) {
			Debug.Log("Cannot find valid attack position.");
		}
	}
	
	protected override void OnFrameUpdate()
    {
		mCurrentForce = Mathf.Lerp(mCurrentForce, mCurrentForceTarget, 0.005f);
		//GlobalMethods.SendMessage(gameObject, "SetForce", mCurrentForce);
    }
	
	protected override void OnLateFrameUpdate()
	{
		if( mPlayerShip == null ) return;
		if( mTargetTransform == null )  return;
		Vector3 distanceV3 = mTargetTransform.position - transform.position;
		Vector3 forwardV3 = transform.forward;
		distanceV3.y = 0f;
		forwardV3.y = 0f;
		float angle = Vector3.Angle(forwardV3, distanceV3);
		angle = angle * AngleDir(forwardV3, distanceV3, Vector3.up);

		//GlobalMethods.SendMessage(gameObject, "SetSteerPower", angle / 360);
		mCurrentForceTarget = 1f;
	}
	
	private float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
  		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);
	    if (dir > 0.0f) {
   		   return 1.0f;
		} else if (dir < 0.0f) {
	        return -1.0f;
	    } else {
	        return 0.0f;
	    }
	}
	
	private Transform GetTargetPosition(AttackShipData data)
	{
		float miniDistance = float.MaxValue;
		Transform attackPosition = null;
		foreach( Transform target in data.GetAllTargetPositions())
		{
			float positionDistance = (transform.position - target.position).magnitude;
			if( positionDistance < miniDistance ) {
				attackPosition = target;
				miniDistance = positionDistance;
			}
		}
		return attackPosition;
	}
	
	private Transform GetNextTargetPosition()
	{
		return null;
	}
	
	
}