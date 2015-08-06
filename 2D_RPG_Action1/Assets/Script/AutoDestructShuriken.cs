using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]

public class AutoDestructShuriken : MonoBehaviour {

	public bool OnlyDeactivate;

	//게임 오브젝트가 활성화 되면 함수가 호출 됩니다.
	void OnEnable(){
		StartCoroutine ("CheckIfAlive");
	}

	IEnumerator CheckIfAlive(){
		while (true) {
			//0.5초 간격으로 루프되는 코루틴.
			yield return new WaitForSeconds (0.5f);
			//파티클이 살아있는지 체크.
			if (!GetComponent<ParticleSystem> ().IsAlive (true)) {
				if (OnlyDeactivate) {
					this.gameObject.SetActive (false);
				} else
					GameObject.Destroy (this.gameObject);
				break;
			}
		}
	}
}
