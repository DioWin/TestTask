using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ConveyorMovmentController), typeof(FruitSpawnerController))]
public class ConveyorController : MonoBehaviour
{
    private ConveyorMovmentController movmentController;
    private FruitSpawnerController fruitSpawnerController;

    [SerializeField] Vector2 conveyotSpeedRange;

    [Header("Hide Settings")]
    [SerializeField] float hideSpeed;
    [SerializeField] float hidingDistanceY;

    private void Awake()
    {
        movmentController = GetComponent<ConveyorMovmentController>();
        fruitSpawnerController = GetComponent<FruitSpawnerController>();

        float randomSpeed = UnityEngine.Random.Range(conveyotSpeedRange.x, conveyotSpeedRange.y);

        SetSpeed(randomSpeed);
    }

    public void Activete()
    {
        fruitSpawnerController.ChangeSpawnerStatus(true);
    }

    public void SetSpeed(float conveyorSpeed)
    {
        movmentController.SetSpeed(conveyorSpeed);
        fruitSpawnerController.SetSpeed(conveyorSpeed);
    }

    public void Hide()
    {
        fruitSpawnerController.Eliminate();

        StartCoroutine(SmoothHiding());
    }

    private IEnumerator SmoothHiding()
    {
        while(transform.position.y > hidingDistanceY)
        {
            yield return new WaitForEndOfFrame();

            transform.Translate(Vector3.down * (hideSpeed * Time.deltaTime));
        }
    }
}
