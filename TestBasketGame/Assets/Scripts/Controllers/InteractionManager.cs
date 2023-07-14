using System;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    public event Action<FruitController> OnSelected;

    [SerializeField] private LayerMask layerMask;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
            {
                FruitController fruitController = hit.transform.parent.GetComponent<FruitController>();

                if (!fruitController.isBlockToSelect)
                    OnSelected?.Invoke(fruitController);
            }
        }
    }
}
