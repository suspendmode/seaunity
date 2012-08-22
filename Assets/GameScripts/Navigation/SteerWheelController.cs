//============================================================================================================
// Weili Zhi Copy right reserved.
//============================================================================================================
/// Action Base
//============================================================================================================
// Created on 18/7/2012 9:06:52 AM by Weili Zhi
//============================================================================================================
using UnityEngine;
using System;

public class SteerWheelController : AIControllerBase
{
	public float mPowerPerRound = 1f;
	public float mMaxAngle = 720.0f;
	public float mMinAngle = -720.0f;
	public float mSlowAcc = 500f;
	
	private Vector3 mCenter = Vector3.zero;
	private float mTotalAngle = 0.0f;
    private bool mHolding = false;
	private float mCurrentSpeed = 0.0f;
	private DragMessage mLastMessage = null;
	private bool mLeavingSteerPositive = false;
		
	void Awake()
	{
		MouseDrag mouseDrag = gameObject.AddComponent<MouseDrag>();
		Rect dragArea = new Rect(0f, 0f, 200f, 150f);
		mouseDrag.DragArea = dragArea;
		mouseDrag.MessageTarget = gameObject;
	}
	
	void Start()
	{
		Camera parentCamera = transform.parent.GetComponent<Camera>();
		if( parentCamera == null ) {
			Debug.Log("SteerWheelController needs to work with a game object under a camera");
			EnableController = false;
		}
		mCenter = transform.localPosition;
	}
	
	public void DragStart()
	{
		if( !EnableController ) return;
		mHolding = true;
	}
	
	public void DragMove(DragMessage msg)
    {
		if( !EnableController ) return;
		float origineZ = transform.eulerAngles.z;
		float turnAngle = - msg.DeltaPosition.x;
		if( msg.StartPosition.x < - mCenter.x ) {
			turnAngle -= msg.DeltaPosition.y;
		}
		else turnAngle += msg.DeltaPosition.y;
		
		mTotalAngle += turnAngle;
		if( mTotalAngle > mMaxAngle ) { 
			turnAngle -= (mTotalAngle - mMaxAngle);
			mTotalAngle = mMaxAngle;
			mCurrentSpeed = 0.0f;
		}
		else if( mTotalAngle < mMinAngle ) {
			turnAngle -= (mTotalAngle - mMinAngle);
			mTotalAngle = mMinAngle;
			mCurrentSpeed = 0.0f;
		}
		else {
			mCurrentSpeed = turnAngle / msg.DeltaTime;
		}
		transform.rotation = transform.rotation * Quaternion.Euler( 0, 0, turnAngle );
		mLastMessage = msg;
    }
	
    public void DragEnd()
    {
		if( !EnableController ) return;
        mHolding = false;
		mLeavingSteerPositive = mCurrentSpeed > 0? true : false;
    }
	
	protected override void OnFrameUpdate()
    {
		if( !mHolding ) {
			if((mLeavingSteerPositive && mCurrentSpeed > 0f ) ||
				(!mLeavingSteerPositive && mCurrentSpeed < 0f )) {
				float slowDown = mSlowAcc * Time.deltaTime;
				Debug.Log("------------xxxxxxxxxx----------" + mCurrentSpeed);
				Vector2 endPosition = mLastMessage.StartPosition;
				endPosition.x += mCurrentSpeed * Time.deltaTime;;
				mLastMessage.EndPosition = endPosition;
				mLastMessage.DeltaTime = Time.deltaTime;
				mLastMessage.DeltaPosition = mLastMessage.StartPosition - mLastMessage.EndPosition;
				DragMove(mLastMessage);
				if( mCurrentSpeed > 0 )
					mCurrentSpeed -= slowDown;
				else
					mCurrentSpeed += slowDown;
				if( Math.Abs(mCurrentSpeed) < slowDown) mCurrentSpeed = 0.0f;
			}
			else if( mTotalAngle != 0f ){
				
			}
		}
    }
   
	/*protected override void OnFrameUpdate()
    {
        Rect Boundry = new Rect(0, 0, 250, 250);

        if( Input.GetMouseButtonDown(0) ) {
        }

for (var j=0; j < Input.touchCount; ++j){

        var drags : Touch = Input.touches[j];

        

        if(drags.phase == TouchPhase.Began && Boundry.Contains(drags.position)){

        transform.rotation = Quaternion.identity;

            minAngle = drags.position.x;

            Debug.Log("touched" + minAngle);

        }

        else if(drags.phase == TouchPhase.Moved && Boundry.Contains(drags.position)){

            

            transform.rotation = Quaternion.AngleAxis(rotateAngle * Time.deltaTime * 40, Vector3.up);

            

            maxAngle = drags.position.x;

        }

        

        else if(drags.phase == TouchPhase.Ended){

            

            transform.rotation = Quaternion.identity;

            minAngle=maxAngle;

            print("END");
}*/
	
}