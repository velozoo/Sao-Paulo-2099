using System;
using UnityEngine;

public class ParallaxeEffect : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float intensity = 20f;
    [SerializeField] private float smoothSpeed = 5f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.localPosition;
    }

    private void Update()
    {
        Vector2 mousePosition = Input.mousePosition;

        float x = (mousePosition.x / Screen.width) - 0.5f;
        float y = (mousePosition.y / Screen.height) - 0.5f;

        Vector3 targetPosition = startPosition + new Vector3(
            -x * intensity,
            -y * intensity,
            0f
        );

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }
}
