using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance { get; private set; }
    public double ElapsedSeconds { get; private set; }
    public bool IsRunning { get; private set; } = true;

    private Text timerText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        CreateTimerUI();
        RefreshDisplay();
    }

    private void Update()
    {
        if (!IsRunning) return;
        ElapsedSeconds += Time.deltaTime;
        RefreshDisplay();
    }

    private void CreateTimerUI()
    {
        GameObject canvasObject = new GameObject("TimerCanvas", typeof(Canvas),
            typeof(CanvasScaler));
        canvasObject.transform.SetParent(transform, false);
        Canvas canvas = canvasObject.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;
        CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject label = new GameObject("TimerText", typeof(RectTransform),
            typeof(CanvasRenderer), typeof(Text), typeof(Outline));
        label.transform.SetParent(canvasObject.transform, false);
        RectTransform rect = label.GetComponent<RectTransform>();
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.anchoredPosition = new Vector2(0f, -24f);
        rect.sizeDelta = new Vector2(360f, 80f);

        timerText = label.GetComponent<Text>();
        timerText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        timerText.fontSize = 48;
        timerText.alignment = TextAnchor.MiddleCenter;
        timerText.color = Color.white;
        timerText.raycastTarget = false;
        Outline outline = label.GetComponent<Outline>();
        outline.effectColor = Color.black;
        outline.effectDistance = new Vector2(2f, -2f);
    }

    private void RefreshDisplay()
    {
        long centiseconds = (long)(ElapsedSeconds * 100d);
        timerText.text = $"{centiseconds / 6000:00}:{centiseconds / 100 % 60:00}.{centiseconds % 100:00}";
    }

    public void StopTimer() => IsRunning = false;
    public void ResumeTimer() => IsRunning = true;

    public void ResetTimer()
    {
        ElapsedSeconds = 0d;
        IsRunning = true;
        RefreshDisplay();
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }
}
