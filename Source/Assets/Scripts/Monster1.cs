using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster1 : MonoBehaviour 
{
	Animator _animator;
	bool wasBiten;

	void Start()
	{
		_animator = GetComponent<Animator>();
	}	

	void Update()
	{
		HandleInputs();
	}


	void HandleInputs()
	{
		if(Input.GetKeyDown(KeyCode.B))
		{
			if(!wasBiten){
				_animator.SetInteger("Bite", 1);
			}else{
				_animator.SetInteger("Bite", 0);
			}
			wasBiten = !wasBiten;
		}
	}
}
