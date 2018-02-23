using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsView : MonoBehaviour 
{
	[SerializeField]
	Text damageVal;
	[SerializeField]
	Text hpVal;
	[SerializeField]
	Text speedVal;

	[SerializeField]
	TendencyPoint tendencyPoint;

	// Use this for initialization
	void Update () {
		var damagePoint = tendencyPoint.damagePoint * 100f;
		var hpPoint = tendencyPoint.hpPoint * 100f;
		var speedPoint = 100 - (damagePoint + hpPoint);
		damageVal.text = Mathf.Round(damagePoint).ToString() + "%";
		hpVal.text = Mathf.Round(hpPoint).ToString() + "%";
		speedVal.text = Mathf.Round(speedPoint).ToString() + "%";
	}
}
