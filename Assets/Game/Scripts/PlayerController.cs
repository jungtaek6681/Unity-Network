using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun.UtilityScripts;

public class PlayerController : MonoBehaviourPun
{
	public void Start()
	{
		foreach (Renderer r in GetComponentsInChildren<Renderer>())
		{
			r.material.color = GameData.GetColor(photonView.Owner.GetPlayerNumber());
		}
	}
}
