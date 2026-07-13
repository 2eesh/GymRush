using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private RectTransform _touchArea;
    [SerializeField] private RectTransform _background;
    [SerializeField] private RectTransform _handle;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _handleRange = 100.0f;
    public Vector2 Direction { get; private set; } = Vector2.zero;
    
    private void OnEnable()
    {
        _canvasGroup.alpha = 0.0f;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_background, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        Vector2 clampedPoint = Vector2.ClampMagnitude(localPoint, _handleRange);
        _handle.anchoredPosition = clampedPoint;

        Direction = clampedPoint / _handleRange;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_touchArea, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
        
        _background.anchoredPosition = localPoint;
        
        _canvasGroup.alpha = 1.0f;
        
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _handle.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
        _canvasGroup.alpha = 0.0f;
    }
}
