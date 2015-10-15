using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Chunk : MonoBehaviour {

	public bool relativePosition = true;
	public Vector2[] vertices;
	private Mesh mesh;
	private Mesh meshBkg;
	private MeshFilter meshBkgFilter;
	private EdgeCollider2D edgeCollider;

	public void updateMesh (){
		mesh = GetComponent<MeshFilter> ().mesh;
		edgeCollider = GetComponent<EdgeCollider2D> ();

		meshBkgFilter = gameObject.transform.Find ("Background").GetComponent<MeshFilter> ();
		meshBkg = meshBkgFilter.mesh;

		float x = 0;
		float y = 0;
		float z = (float) gameObject.transform.position.z;

		if (relativePosition) {
			x = (float) gameObject.transform.position.x;
			y = (float) gameObject.transform.position.y;
		}

		// Use the triangulator to get indices for creating triangles
		Triangulator tr = new Triangulator(vertices);
		int[] indices = tr.Triangulate();
		
		// Create the Vector3 and collider vertices
		Vector3[] vertices3 = new Vector3[vertices.Length];
		Vector2[] points = new Vector2[vertices.Length];
		for (int i=0; i<vertices.Length; i++) {
			vertices3[i] = new Vector3(x + vertices[i].x, y + vertices[i].y, z);
			points[i] = new Vector2(x + vertices[i].x, y + vertices[i].y);
		}

		Vector2[] uvs = new Vector2[vertices3.Length];
		int i2 = 0;
		while (i2 < uvs.Length) {
			uvs[i2] = new Vector2(vertices3[i2].x, vertices3[i2].z);
			i2++;
		}

		edgeCollider.points = points;

		mesh.vertices = vertices3;
		mesh.triangles = indices;
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();

		//Background (reverse terrain)
		meshBkg.vertices = vertices3;
		meshBkg.triangles = indices;
		meshBkg.uv = uvs;
		meshBkg.RecalculateNormals();
		meshBkg.RecalculateBounds();
		ReverseNormals (meshBkgFilter);
	}

	public Mesh getMesh(){
		return mesh;
	}

	public void ReverseNormals (MeshFilter filter) {
		if (filter != null) {
			Mesh mesh = filter.mesh;
			
			Vector3[] normals = mesh.normals;
			for (int i=0; i<normals.Length; i++)
				normals [i] = -normals [i];
			mesh.normals = normals;
			
			for (int m=0; m<mesh.subMeshCount; m++) {
				int[] triangles = mesh.GetTriangles (m);
				for (int i=0; i<triangles.Length; i+=3) {
					int temp = triangles [i + 0];
					triangles [i + 0] = triangles [i + 1];
					triangles [i + 1] = temp;
				}
				mesh.SetTriangles (triangles, m);
			}
		}	
	}
}
