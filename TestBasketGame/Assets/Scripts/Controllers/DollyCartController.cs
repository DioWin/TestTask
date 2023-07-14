using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollyCartController : MonoBehaviour
{
    [SerializeField] CinemachineDollyCart cinemachineDollyCart;
    private CinemachineVirtualCamera camera;
    private Transform cameraTransform;

    public Quaternion victoryRotation; 
    public Quaternion defaultRoration;

    [SerializeField] private float smoothSpeed;
    [SerializeField] private bool isEnable;

    private void Awake()
    {
        camera = GetComponentInChildren<CinemachineVirtualCamera>();

        camera.transform.rotation = defaultRoration;
        cameraTransform = camera.transform;
    }

    private void LateUpdate()
    {
        if (isEnable)
        {
            cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, victoryRotation, smoothSpeed * Time.deltaTime);
        }
    }
    
    public void Activete()
    {
        isEnable = true;
        cinemachineDollyCart.enabled = true;
    }
}
