using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKController : MonoBehaviour
{
    public event Action OnSelectObject;
    public event Action OnHideObject;

    [SerializeField] private Transform target;
    [SerializeField] private float armIKWeight = 1f;
    [SerializeField] private float armIKSpeed = 5f;
    [SerializeField] private Transform rightHandTarget;
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float lookIKWeight = 1f;
    [SerializeField] private float lookIKSpeed = 5f;

    [SerializeField] string hideAnimationName = "Hide";

    private Animator animator;
    private bool isTargetSet = false;

    private Vector3 currentLeftHandPosition;
    private Vector3 currentLookPosition;

    public bool isEnable;

    private float transitionTime = 0.5f;
    private bool isTransitioning = false;
    private float transitionTimer = 0f;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorIK()
    {
        if (!isEnable)
        {
            if (isTransitioning)
            {
                //Smooth transition to the current animation
                transitionTimer += Time.deltaTime;
                float transitionProgress = Mathf.Clamp01(transitionTimer / transitionTime);

                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f - transitionProgress);
                animator.SetIKPosition(AvatarIKGoal.LeftHand, currentLeftHandPosition);

                animator.SetLookAtWeight(1f - transitionProgress);
                animator.SetLookAtPosition(currentLookPosition);

                if (transitionProgress >= 1f)
                    isTransitioning = false;
            }

            return;
        }

        if (isTargetSet)
        {
            UpdateArmIK();
            UpdateLookIK();
        }
        else
        {
            currentLeftHandPosition = animator.GetIKPosition(AvatarIKGoal.LeftHand);
            currentLookPosition = lookTarget.position;
        }
    }

    private void UpdateArmIK()
    {
        if (target == null || rightHandTarget == null)
            return;

        currentLeftHandPosition = Vector3.Lerp(currentLeftHandPosition, target.position, armIKSpeed * Time.deltaTime);

        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, armIKWeight);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, currentLeftHandPosition);

        if ((currentLeftHandPosition - target.position).magnitude <= 0.1f)
            OnSelectObject?.Invoke();
    }

    private void UpdateLookIK()
    {
        if (lookTarget == null)
            return;

        currentLookPosition = Vector3.Lerp(currentLookPosition, lookTarget.position, lookIKSpeed * Time.deltaTime);

        animator.SetLookAtWeight(lookIKWeight);
        animator.SetLookAtPosition(currentLookPosition);
    }

    // Set IK Target
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        lookTarget = newTarget;

        currentLookPosition = lookTarget.position;

        isTargetSet = true;
    }

    public void ActiveteHideAnimation()
    {
        animator.SetTrigger(hideAnimationName);
    }

    public void ChangeEnableStatus(bool isEnable)
    {
        if (isEnable && !this.isEnable)
        {
            isTransitioning = true;
            transitionTimer = 0f;
        }

        this.isEnable = isEnable;
    }

    public void Reset()
    {
        isTargetSet = false;
        target = null;
    }

    // recive from Animation Event
    public void OnReciveToHideObject()
    {
        OnHideObject?.Invoke();
    }
}
