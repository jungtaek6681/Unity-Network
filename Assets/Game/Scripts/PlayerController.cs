using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;

[RequireComponent(typeof(SpaceShip))]
public class PlayerController : MonoBehaviourPun
{
	private SpaceShip spaceShip;

	private void Awake()
	{
		spaceShip = GetComponent<SpaceShip>();
	}

	public void Start()
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.color = GameData.GetColor(photonView.Owner.GetPlayerNumber());
		}

		if (!photonView.IsMine)
			Destroy(this);
	}

	public void Update()
	{
		Accelate();
		Rotate();
		Fire();
	}

	private void Accelate()
	{
		float vInput = Input.GetAxis("Vertical");

		spaceShip.Accelate(vInput);
		
	}

	private void Rotate()
	{
		float hInput = Input.GetAxis("Horizontal");

		spaceShip.Rotate(hInput);
	}

	private void Fire()
	{
		if (Input.GetButtonDown("Fire1"))
			spaceShip.Fire();
	}
}
