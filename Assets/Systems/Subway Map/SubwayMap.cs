using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SubwayMap : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject fullMap;

    [Header("Show/Hide Animation")]
    [SerializeField] private float endPosY;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Ease moveEase;
    private float _startPosY;

    private RectTransform _fullMapRT;
    private bool _isShowing = false;

    private void Start()
    {
        _fullMapRT = fullMap.GetComponent<RectTransform>();
        _startPosY = _fullMapRT.anchoredPosition.y;
        fullMap.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isShowing) 
            ShowMap();
    }

    private void Update()
    {
        if (_isShowing && Input.GetMouseButtonDown(0))
            HideMap();
    }

    public void ShowMap()
    {
        _fullMapRT.DOKill();
        _isShowing = true;
        fullMap.SetActive(true);
        _fullMapRT.DOAnchorPosY(endPosY, moveSpeed).SetEase(moveEase);
    }

    public void HideMap()
    {
        _fullMapRT.DOKill();
        _isShowing = false;
        _fullMapRT.DOAnchorPosY(_startPosY, moveSpeed).SetEase(moveEase).OnComplete(() => {
            fullMap.SetActive(false);
        });
    }
}
