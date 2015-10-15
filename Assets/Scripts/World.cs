using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class World : MonoBehaviour {

	public static float chunkWidth = 10;
	public static int chunkDetail = 16;
	public static float chunkAmplitude = 8;
	public List<GameObject> chunks = new List<GameObject>();
	public GameObject Chunk;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public GameObject generateChunk(int num){
		GameObject chunk = (GameObject) Instantiate(Chunk);
		chunk.transform.position = new Vector3 (num * (chunkWidth/2f), 0f, 1f);
		generateVertices (chunk);
		chunks.Add (chunk);
		return chunk;
	}

	public void clearChunks() {
		foreach(GameObject item in chunks) {
			Destroy(item);
		}
		chunks = new List<GameObject>();
	}

	public void decorateChunk(int num) {

	}

	public void generateVertices(GameObject chunk) {
		Chunk c = chunk.GetComponent<Chunk> ();
		Vector2[] verts = new Vector2[chunkDetail];

		int i;
		for (i=1; i < verts.Length-3; i++) {
			verts[i] = new Vector2 ((float) i*(chunkWidth / verts.Length), Random.Range(0, chunkAmplitude)/10f);
		}

		verts [0] = new Vector2 (0, 0);
		verts [verts.Length - 3] = new Vector2 (chunkWidth, 0);
		verts [verts.Length - 2] = new Vector2 (chunkWidth, -20);
		verts [verts.Length - 1] = new Vector2 (0, -20);

		c.vertices = verts;
		c.updateMesh ();
	}
}
