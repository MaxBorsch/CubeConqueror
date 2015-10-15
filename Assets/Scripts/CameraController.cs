using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public Vector3 minPosition;
	public Vector3 maxPosition;

	private float leftBound;
	private float rightBound;
	private float bottomBound;
	private float topBound;
	private int lastWidth;
	private int lastHeight;
	private bool stay = true;

	void Start () {
		calculateBounds ();
		StartCoroutine( check_for_resize() );
	}

	void Update () {
		if (target == null) return;

		Vector3 pos = target.position + offset;
		transform.position = new Vector3(
			Mathf.Clamp (pos.x, leftBound, rightBound),
			Mathf.Clamp (pos.y, bottomBound, topBound),
			Mathf.Clamp (pos.z, minPosition.z, maxPosition.z)
		);
	}

	public void calculateBounds() {
		float vertExtent = Camera.main.GetComponent<Camera>().orthographicSize;
		float horzExtent = vertExtent * Screen.width / Screen.height;

		leftBound = (float)(horzExtent - minPosition.x / 2.0f);
		rightBound = (float)(maxPosition.x / 2.0f - horzExtent);
		bottomBound = (float)(vertExtent - minPosition.y / 2.0f);
		topBound = (float)(maxPosition.y  / 2.0f - vertExtent);
	}

	IEnumerator check_for_resize(){
		lastWidth = Screen.width;
		lastHeight = Screen.height;

		while( stay ){
			if( lastWidth != Screen.width || lastHeight != Screen.height ){
				calculateBounds ();
				lastWidth = Screen.width;
				lastHeight = Screen.height;
			}
			yield return new WaitForSeconds(0.5f);
		}
	}

	void OnDestroy(){
		stay = false;
	}
}
