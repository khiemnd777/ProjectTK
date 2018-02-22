using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(MeshFilter), typeof(MeshRenderer))]
public class TendencyPoint : MonoBehaviour
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

    Vector3 damagePos;
    Vector3 hpPos;
    Vector3 speedPos;

    Vector3 originalDamagePos;
    Vector3 originalHpPos;
    Vector3 originalSpeedPos;

	Mesh _meshTriangle;
    Mesh meshTriangle
	{
		get
		{
			return _meshTriangle ?? (_meshTriangle = GetComponent<MeshFilter>().mesh);
		}
	}

	LineRenderer _lineRenderer;
	LineRenderer lineRenderer
	{
		get
		{
			return _lineRenderer ?? (_lineRenderer = GetComponent<LineRenderer>());
		}
	}

    Quaternion _120degs;
    Quaternion _240degs;

    void Start()
    {
        // init tendency mesh
        Init();
        // generate init points
        GeneratePoints();
    }

    void Update()
    {
        DetermineOriginalTendencyPositions();
        DetermineTendencyPositions();
        ConfigureText();
        DrawTendencies();
    }

	void Init()
	{
        // compute degrees of 120 and 240
        _120degs = Quaternion.AngleAxis(120f, Vector3.forward);
        _240degs = Quaternion.AngleAxis(240f, Vector3.forward);

        // init line renderer
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = false;
        lineRenderer.positionCount = 7;

        // generate init points
        GeneratePoints();
	}

    void DetermineOriginalTendencyPositions()
    {
        // damage point
        originalDamagePos = Vector3.up * grownDelta;
        // line of damage
        var lineOfDamagePosition = originalDamagePos;
        lineRenderer.SetPosition(1, lineOfDamagePosition);
        // damage text
        damageText.transform.localPosition = originalDamagePos * spaceTextDelta;
        // hp point
        originalHpPos = _120degs * (Vector3.up * grownDelta);
        // line of hp
        var lineOfHpPosition = originalHpPos;
        lineRenderer.SetPosition(3, lineOfHpPosition);
        // hp text
        hpText.transform.localPosition = originalHpPos * spaceTextDelta;
        // spped point
        originalSpeedPos = _240degs * (Vector3.up * grownDelta);
        // line of Speed
        var lineOfSpeedPosition = originalSpeedPos;
        lineRenderer.SetPosition(5, lineOfSpeedPosition);
        // hp text
        speedText.transform.localPosition = originalSpeedPos * spaceTextDelta;
    }

    void DetermineTendencyPositions()
    {
        var originalPosition = Vector3.zero;
        var maxPoint = Mathf.Max(hpPoint, damagePoint, speedPoint);
        hpPos = Vector3.Lerp(originalPosition, originalHpPos, hpPoint / maxPoint);
        damagePos = Vector3.Lerp(originalPosition, originalDamagePos, damagePoint / maxPoint);
        speedPos = Vector3.Lerp(originalPosition, originalSpeedPos, speedPoint / maxPoint);
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
        meshTriangle.Clear();
        meshTriangle.vertices = new[] { hpPos, damagePos, speedPos };
        meshTriangle.uv = new[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1) };
        meshTriangle.triangles = new[] { 0, 1, 2 };
    }

    public void GeneratePoints()
    {
        var a = Random.Range(.1f, .8f);
        var b = Random.Range(.1f, 1 - a - .1f);
        var c = Mathf.Max(.1f, 1 - (a + b));
        var arr = new List<float> { a, b, c };
		var arrIndex = Random.Range(0, arr.Count);
		damagePoint = arr[arrIndex];
		arr.RemoveAt(arrIndex);
		arrIndex = Random.Range(0, arr.Count);
		hpPoint = arr[arrIndex];
		arr.RemoveAt(arrIndex);
		arrIndex = Random.Range(0, arr.Count);
		speedPoint = arr[arrIndex];
		arr.Clear();
    }
}
