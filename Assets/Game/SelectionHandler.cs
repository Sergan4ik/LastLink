using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionHandler : ConnectableMonoBehaviour
{
    public Image selectionBox;
    public Camera camera;
    
    private Vector2 startPos;
    
    public void SelectionProcess(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            startPos = Mouse.current.position.ReadValue();
            selectionBox.rectTransform.anchoredPosition = startPos;
            Debug.Log(Mouse.current.position.ReadValue());
        }
        else if (ctx.canceled)
        {
            selectionBox.rectTransform.anchoredPosition = Vector2.negativeInfinity;
        }
    }

    public void SelectionTick(InputAction selectionAction)
    {
        if (selectionAction.IsPressed())
        {
            var currentMousePosition = Mouse.current.position.ReadValue();
            var size = currentMousePosition - startPos;
            selectionBox.rectTransform.anchoredPosition = Vector2.Min(startPos, currentMousePosition);
            selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
        }
    }
}