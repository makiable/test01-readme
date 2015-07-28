using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public ArcherControl mArcher; //아처 클래스를 가져온다.
	[HideInInspector]
	public List<MonsterControl> mMonster;

	//오토 타깃이 된 몬스터를 참조합니다.
	[HideInInspector]
	public MonsterControl TargetMonster;

	//몬스터 프리펩들을 인스턴스화 할 위치 정보입니다.
	public Transform[] mSpawnPoint;

	//던전을 탐험하는 횟수입니다.
	private int mLoopCount = 5;

	//화면에 나타난 적의 합
	private int mMonsterCount = 0;

	//얼마만큼 뛰다가 적을 만날 것인지.
	private float mRunTime = 1.8f;

	// Use this for initialization
	void Start () {
		//적 몬스터들이 담길 List
		mMonster = new List<MonsterControl> ();
		mMonster.Clear ();

		//던전 탐험 스텝을 만들어서 순서대로 순환 시킵니다.
		StartCoroutine ("AutoStep");
	}
	
	//던전의 현재 스텝
	public enum Status{
		Idle,
		Run,
		BattleIdle,
		Battle,
		Clear
	}

	public Status mStatus = Status.Idle;

	//오토스탭을 자동으로 실행.
	IEnumerator AutoStep()
	{
		while (true) { //코루틴이 계속 실행 될때..
			if (mStatus == Status.Idle) { //아이들 상태라면.
				yield return new WaitForSeconds (1.2f); //1.2초후 던전탐험을 시작..
				mStatus = Status.Run; //런상태로..
			} else if (mStatus == Status.Run) {
				//아처의 애니메이션 상태를 달리기로 설정합니다.
				mArcher.SetStatus (ArcherControl.Status.Run, 1);

				//mRunTime 후 배틀 대기 상태로 돌입합니다.
				yield return new WaitForSeconds (mRunTime);
				mStatus = Status.BattleIdle;
			}


		}
	}


	
	// Update is called once per frame
	void Update () {
	
	}
}























