using UnityEngine;
using System.Collections;

public class HpControl : MonoBehaviour {

	private float mTotalHp;
	private float mNowHp;
	public Transform mBar;
	public TextMesh mHpLabel;

	public void SetHp(int hp){
		//체력을 외부로 입력 받아 두 변수에 담아 둡니다.
		mNowHp = mTotalHp = hp;

		//hp바의 상태를 초기화 합니다.
		mBar.transform.localScale = new Vector3 (1, 1, 1);

		//텍스트로 현재 채력을 표시소 합니다.
		mHpLabel.text = mNowHp.ToString ();
	}

	public void Hit (int damage){
		//현재 채력에서 데미지 만큼씩 뺍니다.
		mNowHp -= damage;

		//체력이 0이하면, invisible함수를 0.1초후에 호출 합니다.
		if (mNowHp <= 0) {
			mNowHp = 0;
			Invoke ("Invisible", 0.1f);

		} 
		// 원래 채력과 현재 데미지 입은 체력 간의 비율로 mBar를 스케일링 합니다.
		mBar.transform.localScale = new Vector3 (mNowHp / mTotalHp, 1, 1);
		mHpLabel.text = mNowHp.ToString ();

	}

	public void Invisible(){
		//이 게임 오프젝트가 사라집니다.
		gameObject.SetActive (false);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
