using Game;
using Game.GameCore;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using ZergRush.ReactiveUI;

public class UnitView : RTSView, ISimpleUpdatableFrom<Unit>
{
    public Unit currentUnit;
    public GameObject selectionVFX;
    public ProgressBar hpBar;
    public TextMeshProUGUI unitName;
    public RTSWorldCanvas worldCanvas;

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
        var e = 0.01f;
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
        unitName.text = unit.cfg.name;
        unitName.color = gameView.localPlayerFaction.slot == unit.factionSlot ? Color.green : Color.red;
    }
    
    public void OnSelectionToggle(bool select)
    {
        selectionVFX.SetActive(select);
        isSelected = select;
    }

    public void UpdateFrom(Unit unit)
    {
        ShowUnit(unit);

        var sqrMagnitude = (transform.position - unit.transform.position).sqrMagnitude;
        if (sqrMagnitude > unit.moveSpeed * Time.deltaTime * unit.moveSpeed * Time.deltaTime && sqrMagnitude < unit.moveSpeed * unit.moveSpeed * 0.25f)
        {
            transform.position = Vector3.MoveTowards(transform.position, unit.transform.position, unit.moveSpeed * (2 * Time.deltaTime));
            transform.rotation = unit.transform.rotation;
        }
        else
        {
            transform.position = unit.transform.position;
            transform.rotation = unit.transform.rotation;
        }

        hpBar.SetProgress(unit.hp, unit.maxHp, "0");

        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName(unit.currentAnimation.animationName) == false ||
            IsAnimationTimeCorrect(CurrentAnimationTimeView, unit.currentAnimation.timer.elapsedTime, unit.currentAnimation.duration) == false)
        {
            animator.Play(unit.currentAnimation.animationName, 0, unit.currentAnimation.normilizedTime);
        }

        unit.view = this;
        worldCanvas.SetActiveSafe(isSelected || gameView.cameraController.pointedUnitView == this);
    }

    public float OnUnload()
    {
        return 0;
    }
}