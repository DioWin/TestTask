using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    public event Action OnMagnetised;

    public FruitType fruitType;
    private float speed;

    [SerializeField] private Transform model;
    private Rigidbody rigidbody;
    private Transform endCorner;
    private Transform target;

    private float minMagnititeValue = 0.1f;

    public bool isBlockToSelect { get; private set; }
    private bool isEnable;
    private bool toTarget;


    private void Awake()
    {
        rigidbody = GetComponentInChildren<Rigidbody>();
    }

    public void Initalize(float speed, Quaternion rotation, Transform endCorner)
    {
        model.rotation = rotation;

        this.speed = speed;
        this.endCorner = endCorner;

        isEnable = true;
    }

    private void Update()
    {
        if (!isEnable)
            return;

        if (toTarget)
        {
            Vector3 direction = (transform.position - target.position).normalized;

            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);

            if ((transform.position - target.position).magnitude <= minMagnititeValue)
            {
                OnMagnetised?.Invoke();


                BlockMovment();
            }

            return;
        }

        if (transform.position.x >= endCorner.position.x)
            Destroy(gameObject);

        transform.Translate(Vector3.right * (speed * Time.deltaTime));
    }

    private void BlockMovment()
    {
        isEnable = false;
        transform.SetParent(target);
        rigidbody.isKinematic = true;
    }

    public void ActivateMagniteToTarget(Transform target, float speed)
    {
        this.speed = speed;
        this.target = target;

        rigidbody.isKinematic = true;
        toTarget = true;
        isEnable = true;
    }

    public void BlockSelect()
    {
        isBlockToSelect = true;
    }
}
