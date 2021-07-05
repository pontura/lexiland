using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class CDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject _itemBeingDrag;
    Vector3 _startPosition;
    Transform _startParent;
    public static bool _enabled;

    void Start()
    {
      //  _enabled = false;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_enabled)
        {
            _itemBeingDrag = gameObject;
            _startPosition = transform.position;
            _startParent = transform.parent;
            GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_enabled)
        {
            transform.position = Input.mousePosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_enabled)
        {
            _itemBeingDrag = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (transform.parent == _startParent)
            {
                transform.position = _startPosition;
            }
        }
    }
}

