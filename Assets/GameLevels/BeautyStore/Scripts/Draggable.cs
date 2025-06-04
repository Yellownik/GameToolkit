using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerMoveHandler
{
    [SerializeField] private Collider2D _collider2D;
    
    private const string TargetTag = "Chair";
    
    public event Action<bool> OnDragEnded;
    
    private bool _isDragging;
    private bool _isCollidedWithChair;
    private Vector3 _pressOffset;

    public void SetDraggingActive(bool isActive) => 
        _collider2D.enabled = isActive;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        _isDragging = true;
        _pressOffset = transform.position - eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (_isDragging)
            SetPosition(eventData);
    }

    private void SetPosition(PointerEventData eventData) => 
        transform.position = eventData.pointerCurrentRaycast.worldPosition + _pressOffset;
    
    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
        OnDragEnded.Invoke(_isCollidedWithChair);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TargetTag)) 
            _isCollidedWithChair = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(TargetTag)) 
            _isCollidedWithChair = false;
    }
}
