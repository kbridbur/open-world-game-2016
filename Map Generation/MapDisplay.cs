using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
  public MeshCollider meshCollider;

	public void DrawTexture(Texture2D texture) {
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData, Texture2D texture) {
    Mesh mesh = meshData.CreateMesh ();
		meshFilter.sharedMesh = mesh;
		meshRenderer.sharedMaterial.mainTexture = texture;
    meshCollider.sharedMesh = mesh;
	}

}