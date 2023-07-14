using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUIElement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private TextMeshProUGUI score;

    private Vector3 defaultPosition;

    public float upwardForce = 5f;
    public float lifeTime = 1.5f;
    public float randomForceRange = 1f;

    public bool isEnable { get; private set; }

    private void Awake()
    {
        defaultPosition = transform.position;
    }

    public void ActivateVisual(int value)
    {
        score.text = $"+{value}";

        ChangeStatus(true);

        Vector3 randomDirection = new Vector3(Random.Range(-randomForceRange, randomForceRange), 1f, Random.Range(-randomForceRange, randomForceRange)).normalized;

        // Применение подкидывающей силы с добавлением случайного направления
        rigidbody.AddForce((Vector3.up + randomDirection) * upwardForce, ForceMode.Impulse);

        StartCoroutine(WaitForDeactivation());
    }

    private IEnumerator WaitForDeactivation()
    {
        yield return new WaitForSeconds(lifeTime);

        ChangeStatus(false);
    }

    private void ChangeStatus(bool isEnable)
    {
        this.isEnable = isEnable;
        gameObject.SetActive(isEnable);

        rigidbody.isKinematic = !isEnable;

        if (!isEnable)
        {
            transform.position = defaultPosition;
        }
    }
}
