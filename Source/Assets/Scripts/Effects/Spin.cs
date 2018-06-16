using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spin : MonoBehaviour 
{
	public float speed;
	[SerializeField]
	Transform _rotatedPivot;
	[SerializeField]
	Transform[] _dirts;
	public float interruptTime;

	void Awake(){
		for(var i =  0; i < _dirts.Length; i++){
			_dirts[i].gameObject.SetActive(false);
		}
	}

	void Start(){
		StartCoroutine(ShowDirts());
	}

	IEnumerator ShowDirts(){
		for(var i =  0; i < _dirts.Length; i++){
			_dirts[i].gameObject.SetActive(true);
			yield return new WaitForSeconds(interruptTime);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		transform.RotateAround(_rotatedPivot.transform.position, Vector3.forward, speed);
	}
}
