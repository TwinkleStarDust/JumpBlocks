using UnityEngine;
using DG.Tweening;

public class JumpitPlayerController : MonoBehaviour
{
    [Header("跳跃设置")]
    public float minJumpForce = 5f;    // 最小跳跃力度
    public float maxJumpForce = 15f;   // 最大跳跃力度
    public float chargeSpeed = 5f;     // 蓄力速度
    public float jumpAngle = 45f;      // 跳跃角度
    public float fallDeathTime = 2f;   // 掉落多久判定死亡

    [Header("视觉效果")]
    public GameObject powerIndicator;   // 力度指示器
    public float rotateSpeed = 720f;    // 跳跃时旋转速度
    public Color chargeStartColor = Color.green;  // 蓄力开始颜色
    public Color chargeEndColor = Color.red;      // 蓄力结束颜色

    private bool isCharging = false;
    private bool isGrounded = false;
    private float currentForce = 0f;
    private bool isDead = false;
    private Vector3 startPosition;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float fallTimer = 0f;      // 掉落计时器

    public void SetStartPosition(Vector3 position)
    {
        startPosition = position;
        transform.position = position;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 刚体设置
        rb.gravityScale = 1f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.linearVelocity = Vector2.zero;

        // 初始状态
        isGrounded = true;
        isJumping = false;
        isCharging = false;
        isDead = false;

        if (powerIndicator != null)
        {
            powerIndicator.SetActive(false);
        }
    }

    private void Update()
    {
        if (isDead) return;

        // 检查是否可以跳跃（在地面上且没有在跳跃中）
        bool canJump = isGrounded && !isJumping && !isCharging;

        // 按下空格开始蓄力
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            StartCharging();
        }

        // 持续蓄力
        if (isCharging)
        {
            ChargeJump();
        }

        // 释放空格跳跃
        if (Input.GetKeyUp(KeyCode.Space) && isCharging)
        {
            Jump();
        }

        // 更新力度指示器
        UpdatePowerIndicator();

        // 更新掉落计时器
        if (!isGrounded && rb.linearVelocity.y < 0)  // 只在下落时计时
        {
            fallTimer += Time.deltaTime;
            if (fallTimer >= fallDeathTime)  // 如果掉落时间超过阈值
            {
                Die();
            }
        }
    }

    private void StartCharging()
    {
        isCharging = true;
        currentForce = minJumpForce;
        if (powerIndicator != null)
        {
            powerIndicator.SetActive(true);
            powerIndicator.transform.localScale = Vector3.one * 0.5f;
            SpriteRenderer indicatorSprite = powerIndicator.GetComponent<SpriteRenderer>();
            if (indicatorSprite != null)
            {
                indicatorSprite.color = chargeStartColor;
            }
        }
    }

    private void ChargeJump()
    {
        currentForce = Mathf.Min(currentForce + chargeSpeed * Time.deltaTime, maxJumpForce);
    }

    private void Jump()
    {
        isCharging = false;
        isJumping = true;
        isGrounded = false;
        if (powerIndicator != null)
        {
            powerIndicator.SetActive(false);
        }

        // 计算跳跃方向和速度
        float angleInRadians = jumpAngle * Mathf.Deg2Rad;
        Vector2 jumpDirection = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
        Vector2 jumpVelocity = jumpDirection * currentForce;

        // 设置刚体速度
        rb.linearVelocity = jumpVelocity;

        // 旋转动画
        transform.DORotate(new Vector3(0, 0, -360), 0.8f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            // 确保在落地时重置所有状态
            isJumping = false;
            isCharging = false;
            isGrounded = true;
            rb.linearVelocity = Vector2.zero;
            transform.rotation = Quaternion.identity;
            fallTimer = 0f;  // 重置掉落计时器
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Respawn"))
        {
            isGrounded = false;
        }
    }

    private void UpdatePowerIndicator()
    {
        if (powerIndicator != null && isCharging)
        {
            float chargeProgress = (currentForce - minJumpForce) / (maxJumpForce - minJumpForce);
            float scale = Mathf.Lerp(0.5f, 2f, chargeProgress);
            powerIndicator.transform.localScale = new Vector3(scale, scale, 1);

            SpriteRenderer indicatorSprite = powerIndicator.GetComponent<SpriteRenderer>();
            if (indicatorSprite != null)
            {
                indicatorSprite.color = Color.Lerp(chargeStartColor, chargeEndColor, chargeProgress);
            }
        }
    }

    public void Die()
    {
        if (!isDead)
        {
            isDead = true;
            isJumping = false;
            isCharging = false;
            isGrounded = false;
            rb.linearVelocity = Vector2.zero;
            spriteRenderer.DOFade(0, 0.5f).OnComplete(() =>
            {
                GameManager gameManager = FindObjectOfType<GameManager>();
                if (gameManager != null)
                {
                    gameManager.ResetPlayerPosition();
                }
            });
        }
    }

    public void ResetGame()
    {
        // 立即停止所有动画
        DOTween.Kill(transform);

        // 重置位置和旋转
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector2.zero;

        // 禁用所有力
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.Sleep(); // 暂时休眠刚体，防止不必要的物理计算

        // 重置状态
        isDead = false;
        isJumping = false;
        isCharging = false;
        isGrounded = true;
        currentForce = 0f;
        fallTimer = 0f;  // 重置掉落计时器

        // 重置玩家透明度
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 1f;
            spriteRenderer.color = color;
            // 立即应用颜色，不使用动画
            DOTween.Kill(spriteRenderer);
        }

        // 重置力度指示器
        if (powerIndicator != null)
        {
            powerIndicator.SetActive(false);
            powerIndicator.transform.localScale = Vector3.one;
            SpriteRenderer indicatorSprite = powerIndicator.GetComponent<SpriteRenderer>();
            if (indicatorSprite != null)
            {
                indicatorSprite.color = chargeStartColor;
            }
        }

        // 确保玩家位置正确
        transform.position = startPosition;

        // 延迟一帧后唤醒刚体
        Invoke("WakeUpRigidbody", 0.02f);
    }

    private void WakeUpRigidbody()
    {
        if (rb != null)
        {
            rb.WakeUp();
            rb.linearVelocity = Vector2.zero;
        }
    }
}