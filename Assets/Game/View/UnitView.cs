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
 
        if (unit.isMoving)
            animator.Play("Run_Forward");
        else
            animator.Play("Idle01");
        unit.view = this;
    }

    public float OnUnload()
    {
        return 0;
    }
}