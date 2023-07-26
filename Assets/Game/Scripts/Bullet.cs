using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Init(Vector3 position, Quaternion rotation, float lag)
    {
        transform.position = position;
        transform.rotation = rotation;

        rigid.velocity = transform.forward * moveSpeed;
        transform.position += rigid.velocity * lag;
    }
}
