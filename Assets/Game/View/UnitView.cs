using Game.GameCore;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush.ReactiveUI;

public class UnitView : RTSView, ISimpleUpdatableFrom<Unit>
{
    public Unit currentUnit;
    public GameObject selectionVFX;
    public ProgressBar hpBar;

    public Animator animator;
    public bool isSelected;
    
    public float CurrentAnimationTimeView
    {
        get
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateInfo.normalizedTime - Mathf.Floor(stateInfo.normalizedTime);
            return normalizedTime * stateInfo.length;
        }
    }
    
    public bool IsAnimationTimeCorrect(float viewSeconds, float modelSeconds, float maxDuration)
    {
        var e = 0.1f;
        if (Mathf.Abs(viewSeconds - modelSeconds) < e)
            return true;
        if (maxDuration - Mathf.Max(viewSeconds,modelSeconds) + Mathf.Min(viewSeconds,modelSeconds) < e)
            return true;
        return false;
    }

    public void ShowUnit(Unit unit)
    {
        currentUnit = unit;
        animator = GetComponentInChildren<Animator>();
    }
    
    public void OnSelectionToggle(bool select)
    {
        selectionVFX.SetActive(select);
        isSelected = select;
    }

    public void UpdateFrom(Unit unit)
    {
        ShowUnit(unit);

        transform.position = unit.transform.position;
        transform.rotation = unit.transform.rotation;

        hpBar.SetProgress(unit.hp, unit.maxHp);

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(unit.currentAnimation.animationName) == false ||
            IsAnimationTimeCorrect(CurrentAnimationTimeView, unit.currentAnimation.timer.elapsedTime, unit.currentAnimation.duration) == false)
        {
            // Debug.Log($"Playing animation {unit.currentAnimation.animationName} (now {animator.GetCurrentAnimatorClipInfo(0)[0].clip.name}) at {unit.currentAnimation.timer.elapsedTime}, current time is {CurrentAnimationTimeView}, duration is {unit.currentAnimation.duration}");
            animator.Play(unit.currentAnimation.animationName, 0, unit.currentAnimation.normilizedTime);
        }

        unit.view = this;
    }

    public float OnUnload()
    {
        return 0;
    }
}