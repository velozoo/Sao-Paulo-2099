using UnityEngine;

public class ButtonsTabs : MonoBehaviour
{
    [Header("TabsController References")]
    [SerializeField] private GameObject tab;
    [SerializeField] private RectTransform initialPos;

    private Vector2 _finalPos;
    private RectTransform _rtTab;

    bool _isOpen = false;

    private void Awake()
    {
        _rtTab = tab.GetComponent<RectTransform>();
        _finalPos = _rtTab.anchoredPosition;

        tab.transform.localScale = Vector3.zero;

        tab.SetActive(false);
    }

    public void OpenTab()
    {
        if (!_isOpen)
        {
            TabsController.Instance.OpenTab(tab, initialPos, _finalPos, () => { _isOpen = true; });
        }

    }

    public void CloseTab()
    {
        if (_isOpen)
        {
            _finalPos = _rtTab.anchoredPosition;
            TabsController.Instance.CloseTab(tab, initialPos, () => { _isOpen = false; });
        }
    }
}
