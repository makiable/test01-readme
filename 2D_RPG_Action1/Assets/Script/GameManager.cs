using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	
	public ArcherControl mArcher; //게임 매니저에서 아처 컨트롤을 합니다. 맞을 때 죽을때 체크를 해야되기 때문.
	[HideInInspector]
	public List<MonsterControl> mMonster; //여러마리 몬스터를 관리 합니다.
	
	// 오토 타겟이 된 몬스터를 참조합니다.
	[HideInInspector]
	public MonsterControl TargetMonster;
	
	// 몬스터 프리팹들을 인스턴스화 할 위치정보입니다.
	public Transform[] mSpawnPoint;
	
	//테스트 용도로 사용. 던전을 탐험하는 횟수입니다.
	//private int mLoopCount = 5;
	
	// 화면에 나타난 적의 합
	private int mMonsterCount = 0;
	
	// 얼마만큼 뛰다가 적을 만날 것인지.
	private float mRunTime = 1.8f;

	//초기화 함수.
	private void Init(){
		//로컬에 저장된 경험치 정보를 가지고 옵니다.
		mExp = PlayerPrefs.GetInt ("2DP_EXP");

		//경험치를 레벨로 환산.
		int lv = getLevel (mExp);
		mUserName.text = lv + ".Archer";

		mMonsterCount = 0;
		mMonsterKillCnt = 0;
		mMonster = new List<MonsterControl> ();
		mMonster.Clear ();
	}

	private int getLevel(int xp){
		int sum = 0;
		int i = 1;
		while (true) {
			sum += 1;
			if (sum *mLevelBalance >= xp) return i;
			i++;
		}
	}

	
	void Start () {
		// 적 몬스터 들이 담길 List
		//mMonster = new List<MonsterControl>();
		//mMonster.Clear(); // 초기화..

		//위에 꺼는 연습용.이번엔 진짜 초기화.
		Init ();

		// 던전 탐험 스텝을 만들어서 순서대로 순환시킵니다.
		StartCoroutine("AutoStep");
	}

	//UI표시될 사항 정리.
	public int mArcherLevel = 1;
	public int mExp = 1;
	public int mMonsterKillCnt;
	public int mLevelBalance = 40; //경펌치에 사용될 변수.
	public TextMesh mUserName ;


	public void ContinueGame(){
		//기존에 생성되어 있던 몬스터 파괴.
		foreach (Transform spawn in mSpawnPoint) {
			int childcount = spawn.childCount;
			if (childcount > 0) {
				Destroy(spawn.GetChild(0).gameObject);
			}
		}

		//데이터를 초기화 하고, 현재 레벨 정보 아처에게 전달.
		mStatus = Status.Idle;
		int lv = getLevel (mExp);

		Init();
		//스텝을 다시 시작.
		StartCoroutine("AutoStep");
	}

	//ResultPopup 컴포턴트가 있는 게임 오브젝트 참조.
	public ResultPopup mResultPopup;
	

	// 던전의 현재 스텝
	public enum Status
	{
		Idle,
		Run,
		BattleIdle,
		Battle,
		Clear,
		GameOver
	}
	
	public Status mStatus = Status.Idle;
	
	IEnumerator AutoStep()
	{
		while(true)
		{
			if(mStatus == Status.Idle)
			{
				// 1.2초를 대기한 후 던전 탐험을 시작합니다.
				yield return new WaitForSeconds(1.2f);
				mStatus = Status.Run;
			}
			else if(mStatus == Status.Run)
			{

				// 아처의 애니메이션 상태를 달리기로 설정합니다.
				mArcher.SetStatus(ArcherControl.Status.Run, 1);
		
				// mRunTime 후 배틀대기 상태로 돌입합니다.
				yield return new WaitForSeconds(mRunTime);


				mStatus = Status.BattleIdle;
			

			}
			else if(mStatus == Status.BattleIdle)
			{
				// 아처를 Run에서 Idle 상태로 전환합니다.
				mArcher.SetStatus(ArcherControl.Status.Idle, 0);
				mMonster.Clear();
				for(int i=0;i<3;++i)
				{	
					// 3마리의 몬스터를 Spawn 합니다.
					SpawnMonster(i);
					// 0.12 초 간격으로 for문을 순환합니다.
					yield return new WaitForSeconds(0.12f);
				}
				
				// 몬스터 3마리를 모두 Spawn 하고 2초를 대기 합니다.
				yield return new WaitForSeconds(2);

				// 배틀 상태로 돌입합니다.
				mStatus = Status.Battle;
				
				// 아처와 몬스터의 공격을 명령합니다.
				StartCoroutine("ArcherAttack");
				StartCoroutine("MonsterAttack");
				yield break;
			}
		}
	}
	
	private void SpawnMonster(int idx)
	{
		// Resources 폴더로부터 Monster 프리팹(Prefab)을 로드합니다.
		Object prefab = Resources.Load("Monster");
		
		// 참조한 프리팹을 인스턴스화 합니다. (화면에 나타납니다.)
		GameObject monster = Instantiate(prefab, mSpawnPoint[idx].position, Quaternion.identity) as GameObject;
		monster.transform.parent = mSpawnPoint[idx];

			
		// 생성된 인스턴스에서 MonsterControl 컴포넌트를 불러내어 mMonster 리스트에 Add 시킵니다.
		mMonster.Add(monster.GetComponent<MonsterControl>());

		
		// 생성된 몬스터 만큼 카운팅 됩니다.
		mMonsterCount += 1;
	
		mMonster[idx].idx = idx;
		mMonster[idx].RandomHp();//
		monster.name = "Monster"+idx;
		// 레이어 오더를 단계적으로 주어 몬스터들의 뎁스가 차례대로 겹치도록 한다.
		monster.GetComponent<SpriteRenderer>().sortingOrder = idx+1;
	}
	
	IEnumerator ArcherAttack()
	{
		// 아처의 타겟이 될 몬스터를 선택합니다.
		GetAutoTarget();
		
		while(mStatus == Status.Battle)
		{
			// 아처의 공격 애니메이션
			mArcher.SetStatus(ArcherControl.Status.Attack, 0);
			
			// 아처의 공격속도 만큼 대기 후 순환합니다.
			yield return new WaitForSeconds(mArcher.mAttackSpeed);
		}
	}


	private void GetAutoTarget()
	{
		// Hp가 가장 낮은 몬스터를 타겟팅 합니다.
		TargetMonster = mMonster.Where(m=>m.mHp > 0).OrderBy(m=>m.mHp).First();
		
		// 타겟은 충돌체가 준비됩니다.
		TargetMonster.SetTarget();
	}
	
	public void ReAutoTarget()
	{
		// 타겟을 재 탐색합니다.
		
		mMonsterCount -= 1;
		TargetMonster = null;
		if(mMonsterCount == 0)
		{
			// 몬스터를 모두 클리어 하였습니다.
			Debug.Log ("Clear");
			if (mStatus == Status.GameOver) return;

			// 모든 공격과 스텝을 중지시킵니다.
			StopCoroutine("ArcherAttack");
			StopCoroutine("MonsterAttack");
			StopCoroutine("AutoStep");
			
			// 던전 스텝을 초기화 시키고 다시 순환 시킵니다.
			mStatus = Status.Idle;
			StartCoroutine("AutoStep");
			return;
		}
		
		// 타겟 재 탐색
		GetAutoTarget();
	}
	
	IEnumerator MonsterAttack()
	{
		while(mStatus == Status.Battle)
		{
			foreach(MonsterControl monster in mMonster)
			{
				// 등록된 모든 몬스터는 공격 애니메이션 상태로
				if(monster.mStatus == MonsterControl.Status.Dead) continue;
				monster.mAnimator.SetTrigger("Shoot");
				Debug.Log("monster shoot");

				
				yield return new WaitForSeconds(monster.mAttackSpeed + Random.Range(0, 0.5f));
				// 몬스터의 공격스피드 + 랜덤 값
			}
		}
	}

	public void SetEXP(){
		mMonsterKillCnt++;
		mExp += 5;
		//경험치를 로컬에 저장.
		PlayerPrefs.SetInt ("2DP_EXP", mExp);
	}
	
	public void GameOver()
	{
		// 주인공이 사망했거나 모든 스테이지 클리어
		Debug.Log("GameOver");	
		mStatus = Status.GameOver;
		mResultPopup.SetResult(mExp, mMonsterKillCnt);
		StopCoroutine("ArcherAttack");
		StopCoroutine("MonsterAttack");
		StopCoroutine("AutoStep");
	}
}
