using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.UI;
using ZergRush;
using ZergRush.ReactiveCore;

public class StateButton
{
    Connections connections = new Connections();
    public Button button;
    public TextMeshProUGUI label;
    public int state;

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
        this.state = state;
        onChangeState?.Invoke(button, label, state);
    }

    public async Task HoldStateUntilTaskDone(Task task, int stateWhileWaiting, int stateAfterWaiting)
    {
        SetState(stateWhileWaiting);
        await task;
        SetState(stateAfterWaiting);
    }

    public async Task<T> HoldStateUntilTaskDone<T>(Task<T> task, int stateWhileWaiting, Func<T, int> stateAfterWaiting)
    {
        SetState(stateWhileWaiting);
        var result = await task;
        SetState(stateAfterWaiting(result));
        return result;
    }
}