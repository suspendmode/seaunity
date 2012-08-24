//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Base AI Controller
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using System.Collections;

// An AI action accepts events from controller and system, performs AI logic and sends out commands.
// AI module is the only one who controls actions under a game object.
public class AIControllerBase : MonoBehaviour
{
	public bool ControllerEnabled = true;
    
    public virtual void Init()
    {
    }

    void Update()
    {
		if( ControllerEnabled ) OnFrameUpdate();
    }

    protected virtual void OnFrameUpdate()
    {
    }
	
	public virtual void DisableController()
	{
		ControllerEnabled = false;
	}
	
	public virtual void EnableController()
	{
		ControllerEnabled = true;
	}
}

