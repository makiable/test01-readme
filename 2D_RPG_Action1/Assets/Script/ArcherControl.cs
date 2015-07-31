using UnityEngine;
using System.Collections;

public class ArcherControl : MonoBehaviour {

	public GameManager mGameManager;

	// 아처의 Animator를 컨트롤할 변수.
	private Animator mAnimator; //mAnimator를 선언.

	//배경들을 컨트롤할 BackgroundControl 스크립트를 등록할 변수.
	private BackgroundControl mBackgrounds;
	private BackgroundControl mForegrounds;

	//화살이 발사 되는 지점(mAttackSpot)과 그 지점을 인스팩터에서 가린다.
	[HideInInspector]
	private Transform mAttackSpot;

	//아처의 공격력, 채력, 공격 속도에 사용 될 변수.
	public int mOrinHP;
	[HideInInspector]
	public int mHP;

	public int mOrinAttack;
	[HideInInspector]
	public int mAttack;

	public float mAttackSpeed;

	//화살의 프리펩 참조.
	public Object mArrowPrefeb;

	//아처의 상태 (대기, 달림, 공격, 사망)
	public enum Status
	{
		Idle,
		Run,
		Attack,
		Dead
	}

	//public으로 선언 되었지만, 인스팩터 뷰에서 노출되지 않기를 원하는 경우 
	//hideinspector를 선언합니다.
	[HideInInspector]
	public Status mStatus = Status.Idle; //아처의 기본상태를 idle로 설정.

	// Use this for initialization
	void Start () { 

		//1.HP 넣고, 2. 백그라운드 컴퍼넌트 넣고, 3. 활이 발사될 장소를 넣고. 스타트

		mHP = mOrinHP;
		mAttack = mOrinAttack;

		//Archer의 Animator 컴포넌트 레퍼런스를 가져옵니다.
		//이 script가 붙은 gameObject에 Animator를 가져옴.
		mAnimator = gameObject.GetComponent<Animator> ();

		//계층 부에 있는 GameObject의 component 중
		//BackgroundControl 타입의 컴포넌트를 모두 가져옵니다.
		BackgroundControl[] component = GameObject.FindObjectsOfType<BackgroundControl> ();

		mBackgrounds = component [0];
		mForegrounds = component [1];

		//child Gameobject 중 spot이라는 이름의 object를 찾아서 
		//좌표값을 transform 컴퍼넌트의 형태로 하여 집어 넣습니다.

		mAttackSpot = transform.FindChild ("spot");
	}
	
	// Update is called once per frame
	void Update () {
		//키보드의 left, right 좌표를 값을 가져옵니다.
		//float speed = Mathf.Abs(Input.GetAxis("Horizontal")); //Horizontal은 화살표 좌우로..

		//SetStatus (Status.Run, speed);
		//mBackgrounds.FlowControl (speed);
		//mForegrounds.FlowControl (speed);

		//if (Input.GetKeyDown (KeyCode.Space)) {
		//	SetStatus(Status.Attack, 0); //멈춰서 쏜다.
		//}
		//else if(Input.GetKeyDown (KeyCode.F)){
		//	SetStatus(Status.Dead, 0);
		//}
		//else if(Input.GetKeyDown (KeyCode.I)){
		//	SetStatus(Status.Idle, 0);
		//}
	}

	//상테와 파라메터를 통해 아처의 상태를 컨트롤 합니다.
	public void SetStatus(Status status, float parameter)
	{
		//animator 에서 만든 상태 간 전이를 상황에 맞게 호출 한다.
		switch (status) {
		case Status.Idle:
			mAnimator.SetFloat("Speed", 0);
			mBackgrounds.FlowControl(0);
			mForegrounds.FlowControl(0);
			break;

		case Status.Run:
			mAnimator.SetFloat("Speed", parameter);
			mBackgrounds.FlowControl(1);
			mForegrounds.FlowControl(1);
			break;

		case Status.Dead:
			mAnimator.SetTrigger("Die");
			Debug.Log("Die");
			break;

		case Status.Attack:
			mAnimator.SetTrigger("Shoot");
			Debug.Log("shoot!");
			break;
		}
	}

	private void ShootArrow()
	{
		Debug.Log ("shoot!");
		GameObject arrow = Instantiate (mArrowPrefeb, mAttackSpot.position, Quaternion.identity) as GameObject; //프리렙에서 게임오브젝트로 붙일때..
		arrow.SendMessage ("Shoot", mGameManager.TargetMonster);
	}
}































