using Game.GameModel;
using UnityEngine;
using ZergRush.ReactiveUI;

public class UnitView : ReusableView
{
    public Unit currentUnit;
    public GameObject selectionVFX;
    public void ShowUnit(Unit unit)
    {
        
    }
    
    public void OnSelectionToggle(bool isSelected)
    {
        selectionVFX.SetActive(isSelected);
    }
}