using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorMovmentController : MonoBehaviour
{
    [SerializeField] private Transform endMovePosition;
    [SerializeField] private List<Transform> conveyorUnits;
    private float speed;

    private Transform lastConveyorUnit;
    private float distanceBetweenUnits;

    private void Awake()
    {
        distanceBetweenUnits = conveyorUnits[1].position.x - conveyorUnits[0].position.x;
        lastConveyorUnit = conveyorUnits[0];
    }

    private void Update()
    {
        HandleConveyorMovement();
    }

    private void HandleConveyorMovement()
    {
        foreach (Transform unit in conveyorUnits)
        {
            unit.transform.Translate(Vector3.right * (speed * Time.deltaTime));
        }

        int conveyorUnitCount = conveyorUnits.Count;

        for (int i = 0; i < conveyorUnitCount; i++)
        {
            if (conveyorUnits[i].position.x >= endMovePosition.position.x)
            {
                Vector3 newPosition = lastConveyorUnit.position;
                newPosition.x -= distanceBetweenUnits;

                conveyorUnits[i].position = newPosition;

                lastConveyorUnit = conveyorUnits[i];
            }
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}






