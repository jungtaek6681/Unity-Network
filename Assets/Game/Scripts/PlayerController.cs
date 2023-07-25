using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun
{
    [SerializeField] List<Color> playerColor;
    [SerializeField] float movePower;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] Bullet bulletPrefab;

    private PlayerInput playerInput;
    private Rigidbody rigid;
    private CinemachineVirtualCamera cm;
    private Vector2 inputDir;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
        cm = FindObjectOfType<CinemachineVirtualCamera>();

        SetPlayerColor();

        if (photonView.IsMine)
        {
            cm.Follow = transform;
            cm.LookAt = transform;
        }
        else
        {
            Destroy(playerInput);
        }
    }

    private void Update()
    {
        Accelate(inputDir.y);
        Rotate(inputDir.x);
    }

    private void OnMove(InputValue value)
    {
        inputDir = value.Get<Vector2>();
    }

    private void OnFire(InputValue value)
    {
        if (value.isPressed)
            Fire();
    }

    private void Accelate(float input)
    {
        rigid.AddForce(input * movePower * transform.forward, ForceMode.Force);
        if (rigid.velocity.magnitude > maxSpeed)
        {
            rigid.velocity = rigid.velocity.normalized * maxSpeed;
        }
    }

    private void Rotate(float input)
    {
        transform.Rotate(Vector3.up, input * rotateSpeed * Time.deltaTime);
    }

    private void Fire()
    {
        photonView.RPC("CreateBullet", RpcTarget.All);
    }

    [PunRPC]
    public void CreateBullet()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (playerColor == null || playerColor.Count <= playerNumber)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material.color = playerColor[playerNumber];
    }
}
