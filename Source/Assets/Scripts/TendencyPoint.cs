using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TendencyPoint : MonoBehaviour
{
	public Material mat;

    void OnRenderObject()
    {
        Draw();
    }

	void OnDrawGizmos()
	{
		Draw();
	}

	void Draw()
	{
		GL.PushMatrix();
        mat.SetPass(0);
		DrawTriangle();
		DrawLine(transform.position, new Vector3(2,3,0));
		GL.PopMatrix();
	}

	void DrawTriangle()
	{
		if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        // GL.LoadOrtho();
        GL.Begin(GL.TRIANGLES);
        GL.Vertex(transform.position);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(0, 1, 0);
        GL.End();
	}

	void DrawLine(Vector3 start, Vector3 end, Color color = default(Color)){
		var cachedPosition = transform.position;
		GL.Begin(GL.LINE_STRIP);
		GL.Color(color);
        GL.Vertex(cachedPosition);
        GL.Vertex(cachedPosition + end);
        GL.End();
	}

	void Draw3TendencyPivots()
	{

	}
}
