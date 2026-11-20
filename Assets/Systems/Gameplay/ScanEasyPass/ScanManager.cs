using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScanManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer passengerSprite;
    [SerializeField] private SpriteRenderer silhoutteSprite;
    [SerializeField] private PolygonCollider2D passengerCollider;
    [SerializeField] private Collider2D monitorCollider;
    [SerializeField] private GameObject npcSilhoutteGO;

    [Header("Text")]
    [SerializeField] private TMP_Text hasPassText;
    [SerializeField] private TMP_Text expirationText;

    [Header("Show/Hide Anim")]
    [SerializeField] private GameObject[] hideObjects;
    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform scanPanel;
    [SerializeField] private Transform scanObject;

    [SerializeField] private float animationDuration = 0.5f;
    [SerializeField] private float hiddenPositionX = -1000f;
    [SerializeField] private float visiblePositionX = 0f;

    [SerializeField] private float hiddenObjectX = -10f;
    [SerializeField] private float visibleObjectX = 0f;

    [SerializeField] private Ease animationEase = Ease.OutCubic;

    [Header("Chip")]
    [SerializeField] private GameObject chipPreFab;
    [SerializeField] private int maxAttempts;
    [SerializeField] private Transform defaultPos;

    private GenericPassengerData _passengerData;
    public bool hasScanned;
    private GameObject _currentChip;

    private Sequence _scanTween;
    public bool _isScanVisible;
    public bool _onAnim;

    public static ScanManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UpdatePassenger(GenericPassengerData passenger)
    {
        if (_currentChip != null)
            Destroy(_currentChip);

        passengerSprite.sprite = passenger.sprite;
        silhoutteSprite.sprite = passenger.sprite;

        if (passenger.easyPassData == null || !passenger.easyPassData.hasPass)
            return;

        for (int i = 0; i < hideObjects.Length; i++)
        {
            hideObjects[i].SetActive(false);
        }

        _passengerData = passenger;


        GenerateCollider();
    }

    private void GenerateCollider()
    {
        if (silhoutteSprite.sprite == null)
            return;

        Sprite sprite = silhoutteSprite.sprite;

        int shapeCount = sprite.GetPhysicsShapeCount();
        
        passengerCollider.pathCount = shapeCount;

        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < shapeCount; i++)
        {
            points.Clear();

            sprite.GetPhysicsShape(i, points);

            passengerCollider.SetPath(i, points);
        }

        SpawnChip();
    }
    [ContextMenu ("SpawnChip")]
    public void SpawnChip()
    {
        hasScanned = false;
        Vector2 spawnPos = GetRandomSpawnPosition();

        _currentChip =  Instantiate(chipPreFab, spawnPos, Quaternion.identity, npcSilhoutteGO.transform);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Bounds bounds = passengerCollider.bounds;

        for (int i = 0; i < maxAttempts; i++)
        {
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomY = Random.Range(bounds.min.y, bounds.max.y);

            Vector2 randomPosition = new Vector2(randomX, randomY);

            bool insidePassenger = passengerCollider.OverlapPoint(randomPosition);
            bool insideTV = monitorCollider.OverlapPoint(randomPosition);

            if (insidePassenger && insideTV)
                return randomPosition;
        }

        return defaultPos.position + new Vector3(2f, defaultPos.position.y);
    }

    public void AnimText(bool isScanning, float t)
    {
        if(hasScanned)
            return;

        if (isScanning)
        {
            print("Scann");
            hasPassText.text = "<+char d=" + t + "> ?????";
            expirationText.text = "<+char d=" + t + " > ?????";
        }
        else
        {
            print("NotScann");
            hasPassText.text = "?????";
            expirationText.text = "?????";
        }
    }

    public void ResetText()
    {
        hasPassText.text = "?????";
        expirationText.text = "?????";
    }

    public void CompleteChip()
    {
        print("Collect");
        hasPassText.text = "<+char d=0.3> True";
        expirationText.text = "<+char d=0.3> " + _passengerData.easyPassData.expirationDate;
    }

    public void ShowScan()
    {
        if (_isScanVisible)
            return;

        _onAnim = true;

        for (int i = 0; i < hideObjects.Length; i++)
        {
            print("revela");
            hideObjects[i].SetActive(true);
        }
        _isScanVisible = true;

        panel.SetActive(true);

        _scanTween?.Kill();

        _scanTween = DOTween.Sequence();

        _scanTween.Join(
            scanPanel.DOAnchorPosX(visiblePositionX, animationDuration)
        );

        _scanTween.Join(
            scanObject.DOLocalMoveX(visibleObjectX, animationDuration)
        );

        _scanTween.SetEase(animationEase);

        _scanTween.OnComplete(() =>
        {
            _onAnim = false;
        });
    }

    public void HideScan()
    {
        if (!_isScanVisible)
            return;

        panel.SetActive(false);

        _onAnim = true;

        _isScanVisible = false;

        _scanTween?.Kill();

        _scanTween = DOTween.Sequence();

        _scanTween.Join(
            scanPanel.DOAnchorPosX(hiddenPositionX, animationDuration)
        );

        _scanTween.Join(
            scanObject.DOLocalMoveX(hiddenObjectX, animationDuration)
        );

        _scanTween.SetEase(animationEase);

        _scanTween.OnComplete(() =>
        {
            _onAnim = false;

            for (int i = 0; i < hideObjects.Length; i++)
            {
                hideObjects[i].SetActive(false);
            }
        });
    }
}