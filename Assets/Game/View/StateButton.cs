using System;
using TMPro;
using UnityEngine.UI;
using ZergRush;
using ZergRush.ReactiveCore;

public class StateButton
{
    Connections connections = new Connections();
    public Button button;
    public TextMeshProUGUI label;
    
    public Action<Button, TextMeshProUGUI, int> onChangeState;

    public void Setup(Action<Button, TextMeshProUGUI, int> onChange)
    {
        onChangeState = onChange;
    }
    
    public void Setup(Action<Button, TextMeshProUGUI, int> onChange, Cell<int> stateCell)
    {
        connections.DisconnectAll();
        
        onChangeState = onChange;
        connections += stateCell.Bind(SetState);
    }
    
    public void SetState(int state)
    {
        onChangeState?.Invoke(button, label, state);
    }
}