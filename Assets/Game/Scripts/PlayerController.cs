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

    private Rigidbody rigid;
    private Vector2 inputDir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        SetPlayerColor();
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

    private void SetPlayerColor()
    {
        int playerNumber = photonView.Owner.GetPlayerNumber();
        if (playerColor == null || playerColor.Count <= playerNumber)
            return;

        Renderer render = GetComponent<Renderer>();
        render.material.color = playerColor[playerNumber];
    }
}
