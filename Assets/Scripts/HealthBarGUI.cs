using UnityEngine;
using System.Collections;

public class HealthBarGUI : MonoBehaviour {
    private Vector3 pos;

    public float xOffset;
    public float yOffset;
	public float healthPercent;
    public Texture backgroundTexture;
	public Texture overlayTexture;

    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        pos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x + xOffset, transform.position.y + yOffset,transform.position.z));
        pos.y = Screen.height - pos.y;
    }

    void OnGUI(){
		GUI.DrawTexture(new Rect(pos.x - 30, pos.y, 60, 10), backgroundTexture);
		GUI.DrawTexture(new Rect(pos.x - 30, pos.y, 60*healthPercent, 10), overlayTexture);
    }
}
