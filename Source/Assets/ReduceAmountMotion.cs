using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReduceAmountMotion : MonoBehaviour 
{
	Text _text;

	void Awake()
	{
		_text = GetComponent<Text>();
	}

	public IEnumerator Reduce(Transform target, int amount)
	{
		_text.text = "-" + amount.ToString();	
		var percent = 0f;
		var srcPos = Vector2.zero;
		var destPos = new Vector2(0, 5f);
		while(percent <= 1f){
			percent += Time.deltaTime * 2.5f;
			transform.localPosition = Vector2.Lerp(srcPos, destPos, percent);
			yield return null;
		}
		Destroy(gameObject);
	}
}
