using Cinemachine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    [SerializeField] List<Color> playerColor;
    [SerializeField] float movePower;
    [SerializeField] float rotateSpeed;
    [SerializeField] float maxSpeed;
    [SerializeField] Bullet bulletPrefab;
    [SerializeField] float fireCoolTime;

    private PlayerInput playerInput;
    private Rigidbody rigid;
    private CinemachineVirtualCamera cm;
    private Vector2 inputDir;
    private float lastFireTime = float.MinValue;

    [SerializeField] int fireCount;
    [SerializeField] float moveSpeed;

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

        moveSpeed = rigid.velocity.magnitude;
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
        photonView.RPC("RequestCreateBullet", RpcTarget.MasterClient);
    }

    [PunRPC]
    public void RequestCreateBullet()
    {
        if (Time.time < lastFireTime + fireCoolTime)
            return;

        lastFireTime = Time.time;
        photonView.RPC("ResultCreateBullet", RpcTarget.AllViaServer, transform.position, transform.rotation);
    }

    [PunRPC]
    public void ResultCreateBullet(Vector3 position, Quaternion rotation, PhotonMessageInfo info)
    {
        float lag = (float)(PhotonNetwork.Time - info.SentServerTime);

        Bullet bullet = Instantiate(bulletPrefab);
        bullet.Init(position, rotation, lag);
        fireCount++;
    }

    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (playerColor == null || playerColor.Count <= playerNumber)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material.color = playerColor[playerNumber];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(moveSpeed);
        }
        else // stream.IsReading
        {
            moveSpeed = (float)stream.ReceiveNext();
        }
    }
}
