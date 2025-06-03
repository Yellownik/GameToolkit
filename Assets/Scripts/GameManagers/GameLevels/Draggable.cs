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

    public void SetDraggingActive(bool isActive) => 
        _collider2D.enabled = isActive;
    
    public void OnPointerDown(PointerEventData eventData) => 
        transform.position = eventData.position;

    public void OnPointerMove(PointerEventData eventData) => 
        transform.position = eventData.position;

    public void OnPointerUp(PointerEventData eventData)
    {
        OnDragEnded.Invoke(_isCollidedWithChair);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TargetTag)) 
            _isCollidedWithChair = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(TargetTag)) 
            _isCollidedWithChair = false;
    }
}
