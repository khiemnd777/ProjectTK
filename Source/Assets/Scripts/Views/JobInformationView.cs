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
	GeneratedBaseCharacter baseCharacter;

	// Use this for initialization
	void Update () {
		jobVal.text = baseCharacter.jobLabel.ToString();
		classVal.text = baseCharacter.classLabel.ToString();
	}
}
