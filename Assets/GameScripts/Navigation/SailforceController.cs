//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Action Base
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using System;

public class SailforceController : AIControllerBase
{
	public float ForceChangeStep = 0.33f;
	public float StepCount = 4f;
	public GameObject ControlTarget = null;
	
	private float mCurrentForceTarget;
	private float mMaxForce;
	private float mCurrentForce;
	private SailforceUIAction mUIAction;
	
	void Start()
	{
		mMaxForce = ForceChangeStep * StepCount;
		mCurrentForceTarget = 0f;
		mUIAction = gameObject.AddComponent<SailforceUIAction>();
		mUIAction.mForeChangeCB = ForceChangeCallback;
		if( ControlTarget == null ) {
			ControlTarget = gameObject;
		}
	}
	
	protected override void OnFrameUpdate()
    {
		mCurrentForce = Mathf.Lerp(mCurrentForce, mCurrentForceTarget, 0.005f);
		ControlTarget.SendMessage("SetForce", mCurrentForce);
    }
	
	private void ForceChangeCallback()
	{
		mCurrentForceTarget += ForceChangeStep;
		mCurrentForceTarget = mCurrentForceTarget % mMaxForce;
	}
			
}

