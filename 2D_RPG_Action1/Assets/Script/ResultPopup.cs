using UnityEngine;
using System.Collections;

public class ResultPopup : MonoBehaviour {

	public GameManager mGameManager;
	public TextMesh mLevel;
	public TextMesh mNextLevel;
	public TextMesh mDisc;
	public GameObject mProgressMid;
	public GameObject mProgressRight;


	public void SetResult(int xp, int monsterCnt){

		gameObject.SetActive (true);

		int[] info = getLevel (xp);
		int minsum = getSum (info [0] - 1);
		float ratio = 0;
	
		mDisc.text = "사냥한 몬스터 수:" + monsterCnt + "마리";
		mNextLevel.text = "/Lv." + info [0];
		mLevel.text = "Level" + (info[0]-1);

		ratio = ((float)(xp - minsum)/(float)(info[1]-minsum));
		mProgressMid.transform.localScale = new Vector3(ratio,1,1); //X좌표로 늘린다...
	
		//exp_progressbar_side.r 스프라이트의 위치.
		mProgressRight.transform.localPosition = new Vector3((2.44f*ratio)-1.16f,0,0);

		mGameManager.mArcherLevel = info[0]-1;
	}

	private int[] getLevel(int xp){
		int sum = 0;
		int i = 1;
		while (true) {
			sum += i;
			if (sum * mGameManager.mLevelBalance >= xp) {
				return new int[]{i+1, sum * mGameManager.mLevelBalance};
			}
			i++;
		}
	}

	private int getSum (int lv){
		int sum = 0;
		for (int i = 1; i < lv; ++i) {
			sum += 1;
		}
		return sum * mGameManager.mLevelBalance;
	}

	void OnButtonDown(string type){
		if (type == "continue") {
			mGameManager.ContinueGame();
		}
		else if (type == "quit") {
			//Device를 종료 합니다.
			Application.Quit();
		}

		gameObject.SetActive (false);
	}
}
