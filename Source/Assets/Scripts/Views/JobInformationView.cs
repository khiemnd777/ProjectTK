using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JobInformationView : MonoBehaviour 
{
	[SerializeField]
	Text jobVal;
	[SerializeField]
	Text classVal;
	[SerializeField]
	Text levelVal;
	[SerializeField]
	Text pointVal;

	[SerializeField]
	GeneratedBaseCharacter baseCharacter;

	BaseClass baseClass;

	void Start()
	{
		baseClass = baseCharacter.baseClass;
	}

	// Use this for initialization
	void Update () {
		jobVal.text = baseCharacter.baseJob.label.ToString();
		classVal.text = baseClass.label.ToString();
		levelVal.text = baseClass.level.ToString();
		pointVal.text = baseClass.pointPerLevel.ToString();
	}
}
