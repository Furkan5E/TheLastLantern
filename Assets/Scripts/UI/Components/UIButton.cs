using UnityEngine;
using UnityEngine.EventSystems;

public class UIButton : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    private UI ui;

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    public void OnSelect(BaseEventData eventData) 
    {
        ui?.MoveIndicators(GetComponent<RectTransform>());
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ui?.HideIndicators();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
}