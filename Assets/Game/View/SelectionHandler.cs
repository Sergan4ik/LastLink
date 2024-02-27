using System.Collections.Generic;
using Game.GameCore;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using ZergRush.ReactiveCore;

public class SelectionHandler : ConnectableMonoBehaviour
{
    public Image selectionBox;
    public Camera camera;
    public Canvas selectionCanvas;
    
    private Vector2 startPos;
    private float startTime;

    public EventStream<(SelectionRectClipSpace, float)> onSelection = new EventStream<(SelectionRectClipSpace, float)>();
    
    public void SelectionProcess(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            startPos = Mouse.current.position.ReadValue();
            startTime = Time.time;
            selectionBox.rectTransform.anchoredPosition = startPos;
            Debug.Log(Mouse.current.position.ReadValue());
        }
        else if (ctx.canceled)
        {
            if (ctx.duration < 0.1f)
            {
                
            }
            SelectBox();
            selectionBox.rectTransform.anchoredPosition = Vector2.negativeInfinity;
        }
    }

    private void SelectBox()
    {
        var leftBottom = selectionBox.rectTransform.anchoredPosition;
        var rightTop = selectionBox.rectTransform.anchoredPosition + selectionBox.rectTransform.sizeDelta;
            
        onSelection.Send((new SelectionRectClipSpace()
        {
            leftBottom = new Vector2(leftBottom.x / Screen.width, leftBottom.y / Screen.height),
            rightTop = new Vector2(rightTop.x / Screen.width, rightTop.y / Screen.height),
            unitToViewportMatrix = Tools.GetUnitToViewportMatrix(camera),
            cameraSize = new Vector2(Screen.width , Screen.height),
            
        }, Time.time - startTime));
    }

    public void SelectionTick(InputAction selectionAction)
    {
        if (selectionAction.IsPressed())
        {
            var currentMousePosition = Mouse.current.position.ReadValue();
            var size = currentMousePosition - startPos;
            selectionBox.rectTransform.anchoredPosition = Vector2.Min(startPos, currentMousePosition);
            selectionBox.rectTransform.sizeDelta = new Vector2(Mathf.Abs(size.x), Mathf.Abs(size.y));
            SelectBox();
        }
    }
}