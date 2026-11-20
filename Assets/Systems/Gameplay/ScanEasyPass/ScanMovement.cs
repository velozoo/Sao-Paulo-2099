using DG.Tweening;
using Unity.Hierarchy;
using UnityEngine;

public class ScanMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform scan;
    [SerializeField] private Camera mainCam;

    [Header("Movement Area")]
    [SerializeField] private Vector2 maxArea;
    [SerializeField] private Vector2 minArea;

    [Header("Return")]
    [SerializeField] private Transform returnPoint;
    [SerializeField] private float returnDuration = 1f;
    [SerializeField] private Ease returnEase = Ease.OutCubic;

    private bool _isDragging;
    private Vector3 _clickOffset;
    private Tween _returnTween;

    private Transform npcStartPos;

    private Vector3 _lastScanPosition;


    private void Update()
    {
        if (ScanManager.Instance._onAnim)
            return;

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            ReturnScan();
        }

        if (_isDragging)
            MoveScan();
    }

    private void OnMouseDown()
    {
        _returnTween?.Kill();

        _lastScanPosition = scan.position;

        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = scan.position.z;

        _clickOffset = scan.position - mousePosition;

        _isDragging = true;
    }

    private void MoveScan()
    {
        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);

        mousePosition += _clickOffset;

        mousePosition.x = Mathf.Clamp(mousePosition.x, minArea.x, maxArea.x);
        mousePosition.y = Mathf.Clamp(mousePosition.y, minArea.y, maxArea.y);

        mousePosition.z = scan.position.z;

        scan.position = mousePosition;

        Vector3 scanMovement = scan.position - _lastScanPosition;

        _lastScanPosition = scan.position;
    }
    private void ReturnScan()
    {
        _returnTween?.Kill();

        _returnTween = scan.DOMove(returnPoint.position, returnDuration)
            .SetEase(returnEase);
    }
}
