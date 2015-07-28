using UnityEngine;
using System.Collections;

public class MonsterControl : MonoBehaviour {

	public Animator mAnimator; //애니메이터를 사용할껍니다. mAnimator에 들어감.

	//생성될 몬스터의 인덱스, 체력, 공격력, 공겨속도 선언.
	[HideInInspector]
	public int idx;
	public int mHP;
	public int mAttack;
	public float mAttackSpeed;

	//몬스터가 발사할 파이어볼의 발사지점
	public Transform mFireShootSpot;

	//몬스터 피격 설정을 위한 콜라이더.
	public Collider mCollider;

	//몬스터가 사용할 파이어 볼 프리팹
	private Object mFirePrefab;

	//몬스터의 상태.
	public enum Status
	{
		Alive,
		Dead
	}

	[HideInInspector]
	public Status mStatus = Status.Alive; //기본은 살아 있도록.


	// Use this for initialization
	void Start () {
	
	}

	//생성되는 몬스터는 현재 체력의 +-10의 채력을 가지게 됩니다.
	public void RandomHP(){
		mHP += Random.Range (-10, 10);
	}

	//몬스터들이 오토 타게팅이 될 경우만 콜라이더를 설정하게 합니다.
	public void SetTarget(){
		mCollider.enabled = true;
	}

	//피격 당할 경우 데미지 처리와 애니메이션 처리
	public void Hit(){
		mAnimator.SetTrigger ("Damage");

		//사망처리
		if (mHP <= 0) {
			mStatus = Status.Dead;
			mHP = 0;
			mCollider.enabled = false; //콜라이더 처리는 빼고.
			mAnimator.SetTrigger("Die");
		}
	}

	//파이어볼 프리팹을 인스턴스 화해서 사용합니다.
	private void ShootFire(){
	
			//파이어볼 프리펩을 씬에서 인스턴스화 하는 과정을 작성하게 됩니다.
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}



























