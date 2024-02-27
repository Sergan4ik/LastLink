using Game.GameCore;
using UnityEngine.UI;
using ZergRush.ReactiveUI;

namespace Game
{
    public class SelectedUnitViewUI : ReusableView
    {
        public Image icon;
        public ProgressBar hpBar;
        public Unit shownUnit;

        public void Show(Unit unit)
        {
            shownUnit = unit;
            icon.sprite = unit.cfg.name.GetUnitIcon();
        }
    }
}