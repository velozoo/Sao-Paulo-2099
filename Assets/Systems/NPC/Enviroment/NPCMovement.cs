
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    private RectTransform rectTransform;

    private float targetX;
    private float speed;
    private float jumpHeight;
    private float jumpSpeed;

    private float baseY;
    private float jumpTimer;

    private bool movingRight;

    public void Initialize(
        float target,
        float movementSpeed,
        float height,
        float frequency
    )
    {
        rectTransform = GetComponent<RectTransform>();

        targetX = target;
        speed = movementSpeed;
        jumpHeight = height;
        jumpSpeed = frequency;

        baseY = rectTransform.anchoredPosition.y;

        movingRight = targetX > rectTransform.anchoredPosition.x;

        UpdateDirection();
    }

    private void Update()
    {
        Move();
        Jump();
    }

    private void Move()
    {
        Vector2 position = rectTransform.anchoredPosition;

        float direction = movingRight ? 1f : -1f;

        position.x += direction * speed * Time.deltaTime;

        if (movingRight && position.x >= targetX)
        {
            position.x = targetX;
            Destroy(gameObject);
        }
        else if (!movingRight && position.x <= targetX)
        {
            position.x = targetX;
            Destroy(gameObject);
        }

        rectTransform.anchoredPosition = position;
    }

    private void Jump()
    {
        jumpTimer += Time.deltaTime * jumpSpeed;

        float jump = Mathf.Abs(Mathf.Sin(jumpTimer)) * jumpHeight;

        Vector2 position = rectTransform.anchoredPosition;
        position.y = baseY + jump;

        rectTransform.anchoredPosition = position;
    }

    private void UpdateDirection()
    {
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * (movingRight ? -1 : 1);

        transform.localScale = scale;
    }
}
