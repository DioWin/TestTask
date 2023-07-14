using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUIController : MonoBehaviour
{
    public event Action OnChangeLevelPressed;

    [SerializeField] private TextMeshProUGUI taskDescription;
    [Header("ChangeLevel")]
    [SerializeField] private GameObject view;
    [SerializeField] private Button chancleLevelButton;
    [SerializeField] private float waitToShowButton;
    [Header("HideView")]
    [SerializeField] private float changeHideViewSpeed;
    [SerializeField] private CanvasGroup canvasGroup;


    private void Awake()
    {
        chancleLevelButton.onClick.AddListener(OnButtonPressed);
    }

    private void OnDestroy()
    {
        chancleLevelButton.onClick.RemoveAllListeners();
    }

    public void ShowChangeLevelView()
    {
        StartCoroutine(WaitToShowView(waitToShowButton));
    }

    private IEnumerator WaitToShowView(float delay)
    {
        yield return new WaitForSeconds(delay);

        view.SetActive(true);
    }

    public IEnumerator ChangeHideViewStatus(bool isShow)
    {
        float targetAlpha = isShow ? 1 : 0;
        float startAlpha = canvasGroup.alpha;
        float elapsedTime = 0;
        float duration = Mathf.Abs(targetAlpha - startAlpha) / changeHideViewSpeed;

        while (elapsedTime < duration)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
        }

        canvasGroup.alpha = targetAlpha;

        if (isShow)
            OnChangeLevelPressed?.Invoke();
    }

    public void SetText(TaskConfig taskConfig)
    {
        string message = $"Collect ";
        message += $"{taskConfig.fruitAmount} {taskConfig.fruitType.ToString()}";

        taskDescription.text = message;
    }

    private void OnButtonPressed()
    {
        StartCoroutine(ChangeHideViewStatus(true));
    }

    public void SmoothShowScene()
    {
        canvasGroup.alpha = 1;
        StartCoroutine(ChangeHideViewStatus(false));
    }

    public void Clear()
    {
        taskDescription.text = "";
    }
}
