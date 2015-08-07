using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class AutoSound : MonoBehaviour {

	public bool OnlyDeactivate;

	IEnumerator Start(){
		while (true) {
			//0.5초 간격으로 오디오가 플레이 중인지 확인합니다.
			yield return new WaitForSeconds(0.5f);

			if (!GetComponent<AudioSource>().isPlaying) {
				//오디오 소스가 플레이 되는지 체크 중.

				//OnlyDeactivate가 되어 있으면 꺼짐..
				if (OnlyDeactivate) {
					gameObject.SetActive(false);

				}
				else
					Destroy(gameObject);
			}
		}
	}
}
