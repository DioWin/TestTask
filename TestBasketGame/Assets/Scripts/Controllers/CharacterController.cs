using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public event Action<FruitType> OnCollected;

    [SerializeField] private IKController iKController;
    [SerializeField] private ScoreDisplayController scoreDisplayController;
    [SerializeField] private BasketConteller basketConteller;

    [SerializeField] private Transform handTransform;

    private InteractionManager interactionController;
    private Animator animator;
    private FruitController tempFruit;

    private TaskConfig currentTask;

    private string victoryTriggerName = "Victory";
    public float magniteSpeed;
    private bool isBlockBehavior;

    private void Awake()
    {
        interactionController = InteractionManager.Instance;
        animator = GetComponent<Animator>();

        iKController.ChangeEnableStatus(true);

        SubscibeOnEvents();
    }

    private void OnDestroy()
    {
        UnsubscibeOnEvents();
    }

    private void SubscibeOnEvents()
    {
        interactionController.OnSelected += SetTarget;
        iKController.OnSelectObject += OnSelectObject;
        iKController.OnHideObject += OnHideObject;
    }

    private void UnsubscibeOnEvents()
    {
        interactionController.OnSelected += SetTarget;
        iKController.OnSelectObject += OnSelectObject;
        iKController.OnHideObject += OnHideObject;
    }

    private void SetTarget(FruitController fruitController)
    {
        if (fruitController.fruitType != currentTask.fruitType || isBlockBehavior)
            return;

        tempFruit = fruitController;

        tempFruit.BlockSelect();

        iKController.SetTarget(fruitController.transform);

        isBlockBehavior = true;
    }

    public void SetTask(TaskConfig taskConfig)
    {
        currentTask = taskConfig;
    }

    public void ActiveteDance()
    {
        basketConteller.Hide();
        iKController.Reset();

        animator.SetTrigger(victoryTriggerName.ToString());
    }

    #region Actions

    private void OnSelectObject()
    {
        // to do smooth to idle
        tempFruit.ActivateMagniteToTarget(handTransform, magniteSpeed);
        tempFruit.OnMagnetised += OnMagnetised;
    }

    private void OnMagnetised()
    {
        tempFruit.OnMagnetised -= OnMagnetised;

        iKController.Reset();

        iKController.ChangeEnableStatus(false);
        iKController.ActiveteHideAnimation();
    }


    private void OnHideObject()
    {
        basketConteller.AddFruit(tempFruit);

        OnCollected?.Invoke(tempFruit.fruitType);
        scoreDisplayController.ShowAddirionalScore(1);

        isBlockBehavior = false;

        iKController.ChangeEnableStatus(true);

        tempFruit = null;
    }
    #endregion
}
