//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Action Base
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using LitJson;
using System.Collections;

public class BattleFieldController : AIControllerBase
{
	private Vector3 mSavedShipPosition;
	private Quaternion mSavedShipRotation;
	
	public GameObject PlayerShip;
	private GameObject mPlayerShip;
	
	void OnDisable()
	{
		GameObject.Destroy(mPlayerShip);
		GameObject Steer = GameObject.Find("Steer");
		if( Steer != null ) {
			GlobalMethods.SendMessage(Steer, "SetPowerTarget", PlayerShip);
		}
	}
	
	public override void EnableController ()
	{
		base.EnableController ();
		PreparePlayerShips();
		GameObject Steer = GameObject.Find("Steer");
		if( Steer != null ) {
			GlobalMethods.SendMessage(Steer, "SetPowerTarget", mPlayerShip);
		}
	}
	
	public override void DisableController ()
	{
		base.DisableController ();
		GameObject.Destroy(mPlayerShip);
		GameObject Steer = GameObject.Find("Steer");
		if( Steer != null ) {
			GlobalMethods.SendMessage(Steer, "SetPowerTarget", PlayerShip);
		}
	}
	private void PreparePlayerShips()
	{
		ShipCache cache = transform.GetComponent<ShipCache>();
		// Load ship information
		
		
		if( PlayerShip == null ) return;
		mPlayerShip = GameObject.Instantiate(PlayerShip) as GameObject;
		if( mPlayerShip != null ) {
			mPlayerShip.transform.parent = transform;
			mPlayerShip.transform.localPosition = Vector3.zero;
			mPlayerShip.transform.rotation = transform.rotation;
			mPlayerShip.SetActiveRecursively(true);
		}
		GameObject Steer = GameObject.Find("Steer");
		if( Steer != null ) {
			GlobalMethods.SendMessage(Steer, "SetPowerTarget", mPlayerShip);
		}
	}
}