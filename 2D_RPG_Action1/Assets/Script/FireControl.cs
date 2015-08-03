using UnityEngine;
using System.Collections;

public class FireControl : MonoBehaviour {

	public MonsterControl mMonster;
	public GameObject mArcher;

	//날아가는 것 구현.
	public void Shoot(MonsterControl monster){
		mMonster = monster;
		//계층 뷰에서 Archer 게임오브젝트틀 Find
		mArcher = GameObject.Find ("Archer").gameObject;

		Vector2 randomPos = Random.insideUnitCircle * .3f;

		iTween.MoveTo (gameObject, iTween.Hash ("Position", 
		                                        mArcher.transform.position + new Vector3 (randomPos.x, 1.5f + randomPos.y, 0), 
		                                        "easytype", 
		                                        iTween.EaseType.easeInCubic, 
		                                        "time", 
		                                        0.5f));
	}

	void OnTriggerEnter (Collider other){

		if (other.tag == "Player") {
			int	damage = mMonster.mAttack;

			//mArcher 게임 오브젝트에 있는 모든 컴포넌트에 있는 함수 중 Hit 함수 호출.
			mArcher.SendMessage("Hit",damage);

			Destroy(gameObject, 0.07f);

		}
	}
}
