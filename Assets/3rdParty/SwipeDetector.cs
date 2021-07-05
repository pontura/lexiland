using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour, IEndDragHandler, IDragHandler
{
    public delegate void Swipe(DraggedDirection draggedDirection);

    public static event Swipe swipe;

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("Press position + " + eventData.pressPosition);
        //Debug.Log("End position + " + eventData.position);
        Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        //Debug.Log("norm + " + dragVectorDirection);
        //GetDragDirection(dragVectorDirection);
        //Debug.Log(dragVectorDirection);
        swipe(GetDragDirection(dragVectorDirection));
    }

    public enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    public DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        //Debug.Log(draggedDir);
        return draggedDir;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }
}
