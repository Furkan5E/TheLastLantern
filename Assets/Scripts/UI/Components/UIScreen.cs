using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIScreen : MonoBehaviour
{
    protected UI ui;
    [SerializeField] protected RectTransform firstSelected;

    protected virtual void Awake()
    {
        ui = GetComponentInParent<UI>();
    }

    protected virtual void OnEnable()
    {
        if (firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
            ui?.MoveIndicators(firstSelected);
        }
    }

    public virtual void OnBackPressed()
    {
        ui?.GoBack();
    }
}