using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Stat
{
	public float baseValue;

	List<float> modifiers = new List<float>();

	public float GetValue() {
		var finalValue = baseValue;
		finalValue += modifiers.Sum();
		return finalValue;
	}

	public void AddModifier(float modifier){
		if(modifier != 0)
			modifiers.Add(modifier);
	}

	public void RemoveModifier(float modifier){
		if(modifier != 0)
			modifiers.Remove(modifier);
	}
}
