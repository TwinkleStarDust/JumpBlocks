using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneTransitionManager>();

                if (instance == null)
                {
                    GameObject go = new GameObject("SceneTransitionManager");
                    instance = go.AddComponent<SceneTransitionManager>();
                }
            }
            return instance;
        }
    }

    private CanvasGroup transitionPanel;
    private bool isFadeComplete = false;
    private bool isTransitioning = false;
    private Canvas transitionCanvas;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeTransitionPanel();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void InitializeTransitionPanel()
    {
        GameObject canvasObj = new GameObject("TransitionCanvas");
        transitionCanvas = canvasObj.AddComponent<Canvas>();
        transitionCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        transitionCanvas.sortingOrder = 999;

        canvasObj.AddComponent<GraphicRaycaster>();

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        GameObject panelObj = new GameObject("TransitionPanel");
        panelObj.transform.SetParent(canvasObj.transform, false);

        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = Color.black;
        panelImage.raycastTarget = true;

        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.sizeDelta = Vector2.zero;
        panelRect.localScale = Vector3.one;
        panelRect.localPosition = Vector3.zero;

        transitionPanel = panelObj.AddComponent<CanvasGroup>();
        transitionPanel.alpha = 0;
        transitionPanel.blocksRaycasts = false;
        transitionPanel.interactable = false;

        DontDestroyOnLoad(canvasObj);

        Debug.Log("TransitionPanel initialized. Canvas Sort Order: " + transitionCanvas.sortingOrder);
    }

    public void LoadSceneWithTransition(string sceneName, float duration = 0.5f)
    {
        if (isTransitioning)
        {
            Debug.LogWarning("Scene transition already in progress!");
            return;
        }

        Debug.Log($"Starting transition to scene: {sceneName}");
        StopAllCoroutines();
        StartCoroutine(TransitionCoroutine(sceneName, duration));
    }

    private IEnumerator TransitionCoroutine(string sceneName, float duration)
    {
        isTransitioning = true;
        isFadeComplete = false;

        transitionCanvas.sortingOrder = 999;

        transitionPanel.blocksRaycasts = true;
        transitionPanel.interactable = true;

        Debug.Log("Starting fade in...");

        var fadeInTween = DOTween.To(() => transitionPanel.alpha, x => transitionPanel.alpha = x, 1f, duration / 2)
            .SetEase(Ease.InOutQuad)
            .OnComplete(() => isFadeComplete = true);

        yield return fadeInTween.WaitForCompletion();

        Debug.Log("Fade in complete. Alpha: " + transitionPanel.alpha);

        transitionPanel.alpha = 1;
        yield return new WaitForSeconds(0.1f);

        Debug.Log("Loading scene: " + sceneName);

        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = true;

        while (!operation.isDone)
        {
            Debug.Log($"Loading progress: {operation.progress * 100}%");
            yield return null;
        }

        Debug.Log("Scene loaded. Starting fade out...");

        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.2f);

        var fadeOutTween = DOTween.To(() => transitionPanel.alpha, x => transitionPanel.alpha = x, 0f, duration / 2)
            .SetEase(Ease.InOutQuad);

        yield return fadeOutTween.WaitForCompletion();

        transitionPanel.alpha = 0;
        transitionPanel.blocksRaycasts = false;
        transitionPanel.interactable = false;
        isTransitioning = false;

        Debug.Log("Transition complete!");
    }

    private void OnDestroy()
    {
        if (transitionPanel != null)
        {
            DOTween.Kill(transitionPanel);
        }
    }
}