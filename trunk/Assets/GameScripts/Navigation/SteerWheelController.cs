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
	public float mMaxRotationSpeed = 1500f;
	public float mSlowAcc = 500f;
	public float mRewindAcc = 500f;
	
	private Vector3 mCenter = Vector3.zero;
	private float mTotalAngle = 0.0f;
    private bool mHolding = false;
	private float mCurrentSpeed = 0.0f;
	private DragMessage mLastMessage = null;
	private bool mLeavingSteerPositive = false;
	private float mRewindAngle = 0f;
	private float mRewindTime = 0f;
	private float mHalfwayRewindTime = 0f;
	
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
		mRewindTime = 0;
		mHalfwayRewindTime = 0;
		mRewindAngle = 0;
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
			if( mCurrentSpeed > mMaxRotationSpeed ) mCurrentSpeed = mMaxRotationSpeed;
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
				if( Math.Abs(mCurrentSpeed) <= slowDown) mCurrentSpeed = 0.0f;
			}
			else if( mTotalAngle != 0f ){ // Speed is 0, so now rewind back to 0.
				float turnAngle = 0f;
				if( mRewindAngle == 0f ) { // Start to rewind.
					mRewindTime = 0f;
					mRewindAngle = mTotalAngle;
					mRewindAcc = Mathf.Abs(mRewindAcc);
					mHalfwayRewindTime = (float) Mathf.Abs(Mathf.Sqrt( Mathf.Abs(mTotalAngle / mRewindAcc)));
					if( mTotalAngle < 0 ) mRewindAcc = -mRewindAcc;
					
				}
				else { // Keep going.
					mRewindTime += Time.deltaTime;
					if( mRewindTime < mHalfwayRewindTime ) {
						mTotalAngle = (mRewindAngle - mRewindAcc * mRewindTime * mRewindTime / 2) % 360f;
						transform.rotation = transform.parent.rotation * Quaternion.Euler( 0f, 0f, mTotalAngle );
					}
					else if( mRewindTime < mHalfwayRewindTime * 2 ){
						float restTime = mHalfwayRewindTime * 2 - mRewindTime;
						mTotalAngle = (mRewindAcc * restTime * restTime / 2) % 360f;
						transform.rotation = transform.parent.rotation * Quaternion.Euler( 0f, 0f, mTotalAngle );
					}
					else {
						mTotalAngle = 0;
						transform.rotation = transform.parent.rotation * Quaternion.Euler( 0f, 0f, 0f );
						StopRewind();
					}
				}
			}
		}
    }
	
	private void StopRewind()
	{
		mRewindTime = 0;
		mHalfwayRewindTime = 0;
		mRewindAngle = 0;
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