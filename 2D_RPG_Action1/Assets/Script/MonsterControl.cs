using UnityEngine;
using System.Collections;

public class MonsterControl : MonoBehaviour{

	private GameManager mGameManager; // 게임 매니저에서 컨트롤 하도록.
	public Animator mAnimator; // 자신의 애니메이터를 참조할 변수
	
	// 생성될 몬스터의 인덱스, 체력, 공격력, 공격속도
	[HideInInspector]
	public int idx;
	public int mHP;
	public int mAttack;
	public float mAttackSpeed;
	
	// 몬스터가 발사할 파이어볼의 발사 지점
	public Transform mFireShootSpot;
	
	// 몬스터의 피격 설정을 위한 콜라이더
	public Collider mCollider;
	
	// 몬스터가 사용할 파이얼 볼 프리팹
	private Object mFirePrefab;

	//HP바 사용.
	public HpControl mHpControl;
	
	// 몬스터의 상태
	public enum Status
	{
		Alive,
		Dead
	}
	
	[HideInInspector]
	public Status mStatus = Status.Alive;
	
	void Start()
	{
		// 참조해야할 객체나 스크립트들을 여기서 설정하게 될 것입니다.
		mGameManager = GameManager.FindObjectOfType<GameManager>();
		mFirePrefab = Resources.Load ("FireBall") as GameObject;
		mHpControl.SetHp(mHP); // 채력을 가져와서 넣는당.
	}
	
	// 생성될 몬스터들은 현재 체력 +- 10의 랜덤 체력을 가지게 됩니다.
	public void RandomHP()
	{
		mHP += Random.Range(-10, 10);	
	}
	
	// 몬스터들이 오토타켓팅이 될 경우만 콜라이더를 세팅하게 합니다.
	public void SetTarget()
	{
		mCollider.enabled = true;	
	}
	
	// 피격 당할 경우 데미지 처리와 애니메이션 처리
	public void Hit()
	{
		GameObject archer = GameObject.Find ("Archer");
		ArcherControl archercontrol = archer.GetComponent<ArcherControl> ();

		// 테스트 문구..
		//mHP -= archercontrol.GetRandomDamage ();
	
		//HP바를 보여주기 위한 ui 및 크리티컬 작업.
		int damage;
		if (archercontrol.IsCritical) {
			damage = archercontrol.GetRandomDamage() * 2;
			Debug.Log("Critical!!!");
			print(damage);
		}
		else {
			damage = archercontrol.GetRandomDamage();
			print(damage);
		}

		mHP -= damage;
		mHpControl.Hit (damage);

		//허드 추가
		HudText (damage, transform.position + new Vector3 (0, 0.7f, 0), archercontrol.IsCritical);

		mAnimator.SetTrigger ("Damage");

		// 사망처리
		if(mHP <= 0)
		{
			Debug.Log("몬스터 사망 ");
			print(mHP);
			mStatus = Status.Dead;
			mHP = 0;
			mCollider.enabled = false;
			mAnimator.SetTrigger("Die");
			mGameManager.ReAutoTarget();
			Destroy(gameObject, 1f);
		}


	}
	
	// 파이어볼 프리팹을 인스턴스(Instance)화 해서 사용합니다.
	private void ShootFire()
	{
		// 파이어볼 프리팹을 씬에 인스턴스화 하는 과정을 작성하게 됩니다.
		//GameObject fire = Instantiate(mFirePrefab, mFireShootSpot.position, Quaternion.identity) as GameObject;
		//fire.SendMessage("Shoot", this);

		GameObject fire = Instantiate (mFirePrefab, mFireShootSpot.position, Quaternion.identity) as GameObject;
		fire.SendMessage ("Shoot", this);

	}

	private void HudText(int damage, Vector3 pos, bool isCritical){
		GameObject prefab = Resources.Load ("HUDTEXT") as GameObject;
		GameObject hudtext = Instantiate (prefab, pos, Quaternion.identity) as GameObject;

		if (isCritical) {
			hudtext.GetComponent<HudText>().setHudText("Critical!! \n" + damage, new Color(255, 216, 0, 255),35);
		}
		else {
			hudtext.GetComponent<HudText>().setHudText(damage.ToString(),new Color(255,255,255,255), 30);
		}
	}



}


































