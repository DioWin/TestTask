using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayController : MonoBehaviour
{
    [SerializeField] private List<ScoreUIElement> scoreUIElements = new List<ScoreUIElement>();

    private void Awake()
    {
        scoreUIElements.Clear();
        scoreUIElements.AddRange(GetComponentsInChildren<ScoreUIElement>(true));
    }

    public void ShowAddirionalScore(int value)
    {
        for (int i = 0; i < scoreUIElements.Count; i++)
        {
            if (!scoreUIElements[i].isEnable)
            {
                scoreUIElements[i].ActivateVisual(value);
                return;
            }
        }
    }    
}
