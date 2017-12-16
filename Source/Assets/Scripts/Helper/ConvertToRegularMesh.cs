using UnityEngine;

public class ConvertToRegularMesh : MonoBehaviour
{
	[ContextMenu("Convert to regular mesh")]
	void Convert(){
		var skinnMeshRenderer = GetComponent<SkinnedMeshRenderer>();
		var meshRenderer = gameObject.AddComponent<MeshRenderer>();
		var meshFilter = gameObject.AddComponent<MeshFilter>();
		
		meshRenderer.sharedMaterials = skinnMeshRenderer.sharedMaterials;
		meshFilter.sharedMesh = skinnMeshRenderer.sharedMesh;

		DestroyImmediate(skinnMeshRenderer);
		DestroyImmediate(this);
	}
}