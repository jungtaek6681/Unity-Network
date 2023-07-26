using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviourPun
{
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        if (photonView.InstantiationData != null)
        {
            rigid.AddForce((Vector3)photonView.InstantiationData[0], ForceMode.Impulse);
            rigid.AddTorque((Vector3)photonView.InstantiationData[1], ForceMode.Impulse);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (transform.position.magnitude > 200f)
            PhotonNetwork.Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
            return;

        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            PhotonNetwork.Destroy(photonView);
    }
}
