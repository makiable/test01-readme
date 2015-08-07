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
	
	//public Transform mArcher_damage_Spot;

	//아처의 공격력, 채력, 공격 속도에 사용 될 변수.
	public int mOrinHP;
	[HideInInspector]
	public int mHP;

	public int mOrinAttack;
	[HideInInspector]
	public int mAttack;

	public float mAttackSpeed;

	//화살의 프리펩 참조.
	public GameObject mArrowPrefeb;
	public GameObject Spot01;

	public HpControl mHPContorl;
	[HideInInspector]
	public bool IsCritical = false;


	//아처의 상태 (대기, 달림, 공격, 사망)
	public enum Status
	{
		Idle,
		Run,
		Attack,
		Dead,
		Reborn,
	}

	//public으로 선언 되었지만, 인스팩터 뷰에서 노출되지 않기를 원하는 경우 
	//hideinspector를 선언합니다.
	[HideInInspector]
	public Status mStatus = Status.Idle; //아처의 기본상태를 idle로 설정.

	public ParticleSystem mSmoke;

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
		mAttackSpot = transform.FindChild ("Spot");

		mHPContorl.SetHp (mHP);

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
			mSmoke.Stop();

			break;

		case Status.Run:
			mAnimator.SetFloat("Speed", parameter);
			mHPContorl.Invisible();
			mBackgrounds.FlowControl(1);
			mForegrounds.FlowControl(1);
			mSmoke.gameObject.SetActive(true);
			break;

		case Status.Dead:
			mAnimator.SetTrigger("Die");
			Debug.Log("Die");
			break;

		case Status.Attack:
			mHPContorl.gameObject.SetActive(true);
			mAnimator.SetTrigger("Shoot");
			break;
		
		case Status.Reborn:
			mAnimator.SetTrigger("Reborn");
			break;
		
		}
	}

	private void ShootArrow()
	{
		GameObject arrow = null;
		//프리렙에서 게임오브젝트로 붙일때..
		arrow = Instantiate (mArrowPrefeb, mAttackSpot.position, Quaternion.identity) as GameObject;

		arrow.SendMessage ("Shoot", mGameManager.TargetMonster);

		MakeEffect ("Eff_BowLight", mAttackSpot.position + new Vector3 (0.75f, 0, 0), transform);

		isCritical ();
		if (IsCritical) {
			//Eff_CriticalFire프리펩을 로드해서, Arrow의 자식으로 위치 시킵니다.
			GameObject fire = MakeEffect ("Eff_CriticalFire", mAttackSpot.position + new Vector3(0.75f, 0, 0), arrow.transform);

			//flre 프리펩은 비활성화 상태로 프리팹화 되었으므로 활성화 시켜줍니다.
			fire.SetActive(true);
		}
	}

	public int GetRandomDamage(){
		return mAttack + Random.Range (0, 20);
	}

	public void Hit(ArrayList param){

		int damage = (int) param[0]; //

		Debug.Log (damage);

		//데미지를 누적 시킵니다.
		mHP -= damage;

		//Text_Meter.text = string.Format("{0:N0}m",Meter);
		//Damaged_Archer.text = string.Format ("(0:NO)", damage);

		HudText (damage, transform.position + new Vector3 (0, 3.1f, 0));

		mHPContorl.Hit (damage);

		MakeEffect ("Eff_Hit_Archer", (Vector3)param [1], transform);

		if (mHP > 0) {
			mAnimator.SetTrigger("Damaged");
		}


		if (mHP <= 0) {
			//사망 처리 
			mStatus = Status.Dead;
			mHP = 0;
			mAnimator.SetTrigger("Die");
			mGameManager.GameOver();

		}
	}

	public void isCritical(){
		int random = Random.Range (0, 10);
		if (random < 5) {
			IsCritical = true;
			Debug.Log("Critical");
		}
		else {
			IsCritical = false;
		}
	}

	private void HudText(int damage, Vector3 pos){

		GameObject prefab = Resources.Load ("HudText") as GameObject;
		GameObject hudtext = Instantiate (prefab, pos, Quaternion.identity) as GameObject;

		hudtext.GetComponent<HudText>().SetHudText(damage.ToString(), new Color(255,255,255,255),28);
	

	}

	public void SetLeveling(int lv){
		//레벨이 증가할때 마다 공격력을 증가 시킵니다.
		int attack = 0;
		for (int i = 1; i < lv; ++i) {
			attack += i*5;
		}
		mAttack = mOrinAttack + attack;
	}

	public void Reborn(){
		mStatus = Status.Idle;
		mHP = mOrinHP;
		mHPContorl.SetHp (mHP);
		mHPContorl.Invisible ();
		SetStatus (Status.Reborn, 0);
	}

	private GameObject MakeEffect(string path, Vector3 pos, Transform _parents){
		GameObject prefab = Resources.Load (path) as GameObject;
		GameObject eff = Instantiate (prefab) as GameObject;

		eff.transform.parent = _parents;
		eff.transform.position = pos;
		return eff;
	}

}































