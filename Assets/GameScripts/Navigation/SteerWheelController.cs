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
	public GameObject PowerTarget = null;
	public float MaxAngle = 720.0f;
	public float MinAngle = -720.0f;
	public float MaxRotationSpeed = 1500f;
	public float SlowAcc = 500f;
	public float RewindAcc = 500f;
	public bool AllowRewind = true;
	public bool SupportMouseControl = true;
	public bool SupportKeyboardControl = true;
	
	private Vector3 mCenter = Vector3.zero;
	private bool mHolding = false;
	private float mCurrentSpeed = 0.0f;
	private DragMessage mLastMessage = null;
	private bool mLeavingSteerPositive = false;
	private float mRewindAngle = 0f;
	private float mRewindTime = 0f;
	private float mHalfwayRewindTime = 0f;
	private float mTotalAngle = 0.0f;
    
	void Awake()
	{
		if( SupportMouseControl ) {
			MouseDrag mouseDrag = gameObject.AddComponent<MouseDrag>();
			Rect dragArea = new Rect(0f, 0f, 200f, 150f);
			mouseDrag.DragArea = dragArea;
			mouseDrag.MessageTarget = gameObject;
		}
		if( SupportKeyboardControl ) {
			KeyBoardController keyboard = gameObject.AddComponent<KeyBoardController>();
		}	
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
		StopRewind();
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
		
		SetAngle( mTotalAngle + turnAngle );
		if( mTotalAngle > MaxAngle ) { 
			turnAngle -= (mTotalAngle - MaxAngle);
			SetAngle( MaxAngle );
			mCurrentSpeed = 0.0f;
		}
		else if( mTotalAngle < MinAngle ) {
			turnAngle -= (mTotalAngle - MinAngle);
			SetAngle( MinAngle );
			mCurrentSpeed = 0.0f;
		}
		else {
			mCurrentSpeed = turnAngle / msg.DeltaTime;
			if( mCurrentSpeed > MaxRotationSpeed ) mCurrentSpeed = MaxRotationSpeed;
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
	
	void KeyBoardDrive(float x)
	{
		float slowDown = SlowAcc * Time.deltaTime;
		if( mLastMessage == null ) {
			mLastMessage = new DragMessage();
			mLastMessage.StartPosition = Vector2.zero;
		}
		
		Vector2 endPosition = mLastMessage.StartPosition;
		endPosition.x -= x * MaxAngle;
		mLastMessage.EndPosition = endPosition;
		mLastMessage.DeltaTime = Time.deltaTime;
		mLastMessage.DeltaPosition = mLastMessage.StartPosition - mLastMessage.EndPosition;
		DragMove(mLastMessage);
	}
	
	protected override void OnFrameUpdate()
    {
		if( !mHolding ) {
			if((mLeavingSteerPositive && mCurrentSpeed > 0f ) ||
				(!mLeavingSteerPositive && mCurrentSpeed < 0f )) {
				float slowDown = SlowAcc * Time.deltaTime;
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
			else if( AllowRewind && mTotalAngle != 0f ){ // Speed is 0, so now rewind back to 0.
				float turnAngle = 0f;
				if( mRewindAngle == 0f ) { // Start to rewind.
					mRewindTime = 0f;
					mRewindAngle = mTotalAngle;
					RewindAcc = Mathf.Abs(RewindAcc);
					mHalfwayRewindTime = (float) Mathf.Abs(Mathf.Sqrt( Mathf.Abs(mTotalAngle / RewindAcc)));
					if( mTotalAngle < 0 ) RewindAcc = -RewindAcc;
					
				}
				else { // Keep going.
					mRewindTime += Time.deltaTime;
					if( mRewindTime < mHalfwayRewindTime ) {
						SetAngle((mRewindAngle - RewindAcc * mRewindTime * mRewindTime / 2) % 360f);
						transform.rotation = transform.parent.rotation * Quaternion.Euler( 0f, 0f, mTotalAngle );
					}
					else if( mRewindTime < mHalfwayRewindTime * 2 ){
						float restTime = mHalfwayRewindTime * 2 - mRewindTime;
						SetAngle((RewindAcc * restTime * restTime / 2) % 360f);
						transform.rotation = transform.parent.rotation * Quaternion.Euler( 0f, 0f, mTotalAngle );
					}
					else {
						SetAngle(0);
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
	
	private void SetAngle(float angle) {
		if( mTotalAngle == angle && angle == 0 ) return;
		mTotalAngle = angle;
		if( PowerTarget != null ) {
			PowerTarget.SendMessage("SetSteerPower", mTotalAngle / MaxAngle);
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