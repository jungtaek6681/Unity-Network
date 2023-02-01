using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceShip : MonoBehaviourPun, IPunObservable
{
	private Rigidbody rigid;

	[SerializeField]
	private float movePower;
	[SerializeField]
	private float rotateSpeed;
	[SerializeField]
	private float maxSpeed;

	[SerializeField]
	private Bullet bulletPrefab;

	[SerializeField]
	private int bulletCount;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		CheckExitScreen();
	}

	public void Accelate(float power)
	{
		rigid.AddForce(power * transform.forward * movePower * Time.deltaTime, ForceMode.Force);
		if (rigid.velocity.magnitude > maxSpeed)
		{
			rigid.velocity = rigid.velocity.normalized * maxSpeed;
		}
	}

	public void Rotate(float speed)
	{
		transform.Rotate(Vector3.up, speed * rotateSpeed * Time.deltaTime);
	}

	private void CheckExitScreen()
	{
		if (Camera.main == null)
			return;

		if (Mathf.Abs(rigid.position.x) > (Camera.main.orthographicSize * Camera.main.aspect))
		{
			rigid.position = new Vector3(-Mathf.Sign(rigid.position.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, rigid.position.z);
			rigid.position -= rigid.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
		}

		if (Mathf.Abs(rigid.position.z) > Camera.main.orthographicSize)
		{
			rigid.position = new Vector3(rigid.position.x, rigid.position.y, -Mathf.Sign(rigid.position.z) * Camera.main.orthographicSize);
			rigid.position -= rigid.position.normalized * 0.1f; // offset a little bit to avoid looping back & forth between the 2 edges 
		}
	}

	public void Fire()
	{
		photonView.RPC("CreateBullet", RpcTarget.All, transform.position, transform.rotation);
		bulletCount++;
	}

	[PunRPC]
	public void CreateBullet(Vector3 position, Quaternion rotation)
	{
		Instantiate(bulletPrefab, position, rotation);
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		// 순서주의 : 스트림의 주는 순서와 받는 순서를 동일하게
		if (stream.IsWriting)
		{
			stream.SendNext(bulletCount);
		}
		else  // stream.IsReading
		{
			bulletCount = (int)stream.ReceiveNext();
		}
	}
}
