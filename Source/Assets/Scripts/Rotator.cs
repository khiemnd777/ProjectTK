using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotatorDirection { Left, Right }

public class Rotator : MonoBehaviour
{
    public RotatorDirection direction;
	public float speed = 100;

    void Update()
    {
		var dir = direction == RotatorDirection.Left ? 1 : -1;
		transform.Rotate(Vector3.forward * Time.deltaTime * speed * dir, Space.World);
    }
}
