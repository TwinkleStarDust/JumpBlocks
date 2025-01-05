using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DisappearingGround : MonoBehaviour
{
    private float baseTimeBeforeDisappear = 3f;
    private float timePerHeight = 0.5f;
    private float fadeOutDuration = 1f;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;
    private float actualTimeBeforeDisappear;
    private bool isInitialized = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (spriteRenderer == null || boxCollider == null)
        {
            Debug.LogError("DisappearingGround: Missing required components!");
            return;
        }

        if (!isInitialized)
        {
            Debug.LogWarning("DisappearingGround was not initialized with parameters!");

            Initialize(baseTimeBeforeDisappear, timePerHeight, fadeOutDuration, 0f);
            isInitialized = true;
        }
    }

    public void Initialize(float baseTime, float timePerHeight, float fadeOutDuration, float animationDelay = 0f)
    {
        baseTimeBeforeDisappear = baseTime;
        this.timePerHeight = timePerHeight;
        this.fadeOutDuration = fadeOutDuration;
        isInitialized = true;

        // 计算实际消失时间（基础时间 + 基于高度的额外时间）
        float heightFactor = transform.position.y / 7.5f; // 使用标准step.y值
        float actualDisappearTime = baseTimeBeforeDisappear + (heightFactor * timePerHeight);

        // 启动消失序列，考虑动画延迟
        StartCoroutine(DisappearSequence(actualDisappearTime, animationDelay));
    }

    private IEnumerator DisappearSequence(float disappearTime, float animationDelay)
    {
        // 等待动画完成
        if (animationDelay > 0)
        {
            yield return new WaitForSeconds(animationDelay);
        }

        // 等待消失时间
        yield return new WaitForSeconds(disappearTime);

        // 开始闪烁警告
        float warningDuration = 1f;
        float flashInterval = 0.1f;
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color warningColor = Color.red;

        for (float t = 0; t < warningDuration; t += flashInterval)
        {
            spriteRenderer.color = spriteRenderer.color == originalColor ? warningColor : originalColor;
            yield return new WaitForSeconds(flashInterval);
        }

        // 恢复原始颜色并开始淡出
        spriteRenderer.color = originalColor;
        spriteRenderer.DOFade(0f, fadeOutDuration).OnComplete(() => Destroy(gameObject));
    }

    private void OnDestroy()
    {
        DOTween.Kill(spriteRenderer);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            float heightFromGround = transform.position.y;
            float predictedTime = baseTimeBeforeDisappear + (heightFromGround * timePerHeight);
            UnityEditor.Handles.Label(transform.position, $"预计消失时间: {predictedTime:F1}秒");
        }
    }
#endif
}