using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	private Rigidbody rigid;

	[SerializeField]
	private float speed;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		rigid.velocity = transform.forward * speed;
		Destroy(gameObject, 3.0f);
	}
}
