using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketConteller : MonoBehaviour
{
    [SerializeField] private List<Transform> positionsToSpawn;
    [SerializeField] float declineSpeed = 1f;

    private int positionIndex;

    private List<FruitController> fruitControllers = new List<FruitController>();


    public void AddFruit(FruitController currentFruit)
    {
        if (positionsToSpawn.Count <= positionIndex)
        {
            Debug.LogError("basket is full");
            return;
        }

        currentFruit.ActivateMagniteToTarget(positionsToSpawn[positionIndex].transform, declineSpeed);

        currentFruit.transform.SetParent(positionsToSpawn[positionIndex].transform);

        fruitControllers.Add(currentFruit);

        positionIndex++;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
