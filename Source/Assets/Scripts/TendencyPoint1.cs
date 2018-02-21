using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TendencyPoint1 : MonoBehaviour
{
	public float grownDelta = 1f;
	[Range(0, 1)]
	public float damagePoint = 1f;
	[Range(0, 1)]
	public float hpPoint = 1f;
	[Range(0, 1)]
	public float speedPoint = 1f;
	[Space]
	[SerializeField]
	TextMesh damageText;
	[SerializeField]
	TextMesh hpText;
	[SerializeField]
	TextMesh speedText;
	[SerializeField]
	float spaceTextDelta = 1f;
	[SerializeField]
	float textSize = .3f;
	
	LineRenderer _lineRenderer;

	Vector3 damagePos;
	Vector3 hpPos;
	Vector3 speedPos;

	Vector3 originalDamagePos;
	Vector3 originalHpPos;
	Vector3 originalSpeedPos;

    GameObject m_goTriangle;
	Mesh m_meshTriangle;
	Quaternion _120degs;
	Quaternion _240degs;

	void Start()
	{
		// init tendency mesh
		m_goTriangle = gameObject;
		m_goTriangle.AddComponent<MeshFilter>();
        m_goTriangle.AddComponent<MeshRenderer>();
        m_meshTriangle = m_goTriangle.GetComponent<MeshFilter>().mesh;

		// compute degrees of 120 and 240
		_120degs = Quaternion.AngleAxis(120f, Vector3.forward);
		_240degs = Quaternion.AngleAxis(240f, Vector3.forward);

		// init line renderer
		_lineRenderer = GetComponent<LineRenderer>();
		_lineRenderer.useWorldSpace = false;
		_lineRenderer.loop = false;
		_lineRenderer.positionCount = 7;
	}

    void Update()
    {
		DetermineOriginalTendencyPositions();
		DetermineTendencyPositions();
		ConfigureText();
		DrawTendencies();
    }

	void DetermineOriginalTendencyPositions()
	{
		// damage point
		originalDamagePos = Vector3.up * grownDelta;
		// line of damage
		var lineOfDamagePosition = originalDamagePos;
		_lineRenderer.SetPosition(1, lineOfDamagePosition);
		// damage text
		damageText.transform.localPosition = originalDamagePos * spaceTextDelta;
		// hp point
		originalHpPos = _120degs * (Vector3.up * grownDelta);
		// line of hp
		var lineOfHpPosition = originalHpPos;
		_lineRenderer.SetPosition(3, lineOfHpPosition);
		// hp text
		hpText.transform.localPosition = originalHpPos * spaceTextDelta;
		// spped point
		originalSpeedPos = _240degs * (Vector3.up * grownDelta);
		// line of Speed
		var lineOfSpeedPosition = originalSpeedPos;
		_lineRenderer.SetPosition(5, lineOfSpeedPosition);
		// hp text
		speedText.transform.localPosition = originalSpeedPos * spaceTextDelta;
	}

	void DetermineTendencyPositions()
	{
		var originalPosition = Vector3.zero;
		hpPos = Vector3.Lerp(originalPosition, originalHpPos, hpPoint);
		damagePos = Vector3.Lerp(originalPosition, originalDamagePos, damagePoint);
		speedPos = Vector3.Lerp(originalPosition, originalSpeedPos, speedPoint);
	}

	void ConfigureText()
	{
		damageText.characterSize = textSize;
		hpText.characterSize = textSize;
		speedText.characterSize = textSize;
	}

	void DrawTendencies()
	{
		// draw triangle
        m_meshTriangle.Clear();
		m_meshTriangle.vertices = new [] { hpPos, damagePos, speedPos };
		m_meshTriangle.uv = new [] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        m_meshTriangle.triangles = new [] { 0, 1, 2 };
	}

	public void GeneratePoints()
	{
		damagePoint = Random.Range(.2f, 1);
		hpPoint = Random.Range(.2f, 1);
		speedPoint = Random.Range(.2f, 1);
	}
}
