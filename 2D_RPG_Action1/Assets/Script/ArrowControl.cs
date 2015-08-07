using UnityEngine;
using System.Collections;

public class ArrowControl : MonoBehaviour {


	//날아가는 속도? 
	//콜리더가 겹쳐지는지 Tigger 체크해서 데미지 던지주는 것? -> 몬스터 
	private MonsterControl mMonster;
	public BoxCollider mCollider;

	// Use this for initialization
	void Start () {
		//Arrow 오브젝트의 Box Collider를 가져옵니다. 왜냐 메모리에 올려놔야지 체크할 수 있으니..
		mCollider = gameObject.GetComponent<BoxCollider> ();
	}

	public void Shoot (MonsterControl monster){
		mMonster = monster;
		Vector2 randomPos = Random.insideUnitCircle * 0.2f;
		iTween.MoveTo (gameObject, iTween.Hash ("position", monster.transform.position + new Vector3 (randomPos.x, randomPos.y, 0), "easetype", 
		                                      iTween.EaseType.easeOutCubic, "time", 0.3f));
	}
	
	void OnTriggerEnter(Collider other){
		//몬스터 충돌체와 충돌시 충돌정보가 전달 됩니다.

		Debug.Log ("Collider check");

		if (other.tag == "Monster") {
			mCollider.enabled = false;
			mMonster.Hit(transform.position); //힛함수 실행.


			//화살 오브젝트를 0.07초 후 파괴합니다.
			Destroy(gameObject, 0.07f);
		}
	}

	void Update(){

	}

}
