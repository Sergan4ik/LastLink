using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SelectionHandler : ConnectableMonoBehaviour
{
    public Image selectionBox;
    public Camera camera;
    
    private Vector2 startPos;
    public HashSet<UnitView> selectedUnits = new HashSet<UnitView>();
    
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
            //get 4 corners of the selection box
            var leftBottom = selectionBox.rectTransform.anchoredPosition;
            var rightTop = selectionBox.rectTransform.anchoredPosition + selectionBox.rectTransform.sizeDelta;
            var leftTop = new Vector2(leftBottom.x, rightTop.y);
            var rightBottom = new Vector2(rightTop.x, leftBottom.y);
            
            //get world space positions by raycast
            Physics.Raycast(camera.ScreenPointToRay(leftBottom), out var hitLeftBottom, 99999999, 1 << LayerMask.NameToLayer("Ground"));
            Physics.Raycast(camera.ScreenPointToRay(rightTop), out var hitRightTop, 99999999, 1 << LayerMask.NameToLayer("Ground"));
            Physics.Raycast(camera.ScreenPointToRay(leftTop), out var hitLeftTop, 99999999, 1 << LayerMask.NameToLayer("Ground"));
            Physics.Raycast(camera.ScreenPointToRay(rightBottom), out var hitRightBottom, 99999999, 1 << LayerMask.NameToLayer("Ground"));
            
            var worldLeftBottom = hitLeftBottom.point;
            var worldRightTop = hitRightTop.point;
            var worldLeftTop = hitLeftTop.point;
            var worldRightBottom = hitRightBottom.point;
            
            //log the world space positions
            Debug.Log($"LeftBottom: {worldLeftBottom}");
            Debug.Log($"RightTop: {worldRightTop}");
            Debug.Log($"LeftTop: {worldLeftTop}");
            Debug.Log($"RightBottom: {worldRightBottom}");
            
            //draw a box in the world space
            Debug.DrawLine(worldLeftBottom, worldLeftTop, Color.red, 5);
            Debug.DrawLine(worldLeftTop, worldRightTop, Color.red, 5);
            Debug.DrawLine(worldRightTop, worldRightBottom, Color.red, 5);
            Debug.DrawLine(worldRightBottom, worldLeftBottom, Color.red, 5);
            
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