using UnityEngine;
using System.Collections;

public class RotateSimple : MonoBehaviour {

	public float rotationSpeedX = 0;
	public float rotationSpeedY = 0;
	public float rotationSpeedZ = 0;

	// Update is called once per frame
	void Update () {
	    // 매 프리엠에 원하는 방향으로 회전 시킵니다.
		transform.Rotate (new Vector3 (rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
	}
}
