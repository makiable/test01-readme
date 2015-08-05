using UnityEngine;
using System.Collections;

public class HudText : MonoBehaviour {

	public TextMesh mLabel;
	//public GUIText mLabel;
	public TextOutline mOutline;
	
	private void dead(){
		//이 게임오브젝트는 파괴 합니다.
		Destroy (gameObject);
	}
	public void SetHudText(string text, Color32 color, int size){
		//텍스트와 컬러 크기 값을 받아 텍스트 메시에 설정 합니다.
		mLabel.text = text;
		mLabel.color = color;
		mLabel.fontSize = size;

	}

	private void invisible(){
		//textoutline의 컴포넌트의 dead함수를 호출하여 아웃라인을 제거합니다.
		//mOutline.dead();
		//Destroy (gameObject);

}


	private void destroy(){
		Destroy(gameObject);

	}


}
