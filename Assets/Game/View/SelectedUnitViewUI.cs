using Game.GameCore;
using UnityEngine.UI;
using ZergRush.ReactiveUI;

namespace Game
{
    public class SelectedUnitViewUI : ReusableView
    {
        public Image icon;

        public void Show(Unit unit)
        {
            icon.sprite = unit.cfg.name.GetUnitIcon();
        }
    }
}