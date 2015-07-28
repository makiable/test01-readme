using UnityEngine;
using System.Collections;

public class BackgroundControl : MonoBehaviour {
	//Background와 Foreground의 Animator를 등록시킬 변수.
	// Public 으로 선언된 변수는 인스펙터 뷰에서 접근과 수정이 가능합니다. (외부에서 조정이 가능하다는 이야기)

	public Animator[] mBackgrounds;

	// Use this for initialization
	void Start () {
		//객체가 활성화 되고 1번 호추 ㄹ됩니다.
		FlowControl (0); //처음 시작할때는 0으로 백그라운드로 움직이지 않게.

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void FlowControl(float speed)
	{
		//등록된 모든 애니메이터들의 Speed를 조정
		foreach (Animator bg in mBackgrounds)
			bg.speed = speed;
	}
}
