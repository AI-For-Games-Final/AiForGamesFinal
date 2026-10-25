using UnityEngine;

public class UIGroup : MonoBehaviour
{
    CanvasGroup _canvasGroup;

	public virtual void Start()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}
	public void SetCanvasGroup(bool enabled)
    {
        _canvasGroup.alpha = enabled ? 1 : 0;
        _canvasGroup.enabled = enabled;
        _canvasGroup.blocksRaycasts = enabled;
    }

    public virtual void Display(Component sender, object data) { }
}
