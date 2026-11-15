
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class EndGameTrigger : MonoBehaviour
{
    private static bool gameEnded;
    private float previousTimeScale;
    private bool ownsPause;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState() => gameEnded = false;

    private void Reset()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (gameEnded || other.GetComponentInParent<PlayerBehavior>() == null) return;
        if (GameTimer.Instance == null) return;

        gameEnded = true;
        GameTimer.Instance.StopTimer();
        double elapsed = GameTimer.Instance.ElapsedSeconds;
        Scene scene = SceneManager.GetActiveScene();
        string key = "BestTime:" + (string.IsNullOrEmpty(scene.path) ? scene.name : scene.path);
        double best = double.PositiveInfinity;
        if (PlayerPrefs.HasKey(key))
        {
            if (!double.TryParse(PlayerPrefs.GetString(key),
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture, out best)
                || best < 0d || double.IsNaN(best))
                best = double.PositiveInfinity;
        }
        bool newBest = elapsed < best;
        if (newBest)
        {
            best = elapsed;
            PlayerPrefs.SetString(key, best.ToString("R", System.Globalization.CultureInfo.InvariantCulture));
            PlayerPrefs.Save();
        }

        previousTimeScale = Time.timeScale;
        ownsPause = true;
        Time.timeScale = 0f;
        ShowOverlay(elapsed, best, newBest);
    }

    private static string FormatTime(double seconds)
    {
        long ticks = (long)(seconds * 100d);
        return $"{ticks / 6000:00}:{ticks / 100 % 60:00}.{ticks % 100:00}";
    }

    private void ShowOverlay(double elapsed, double best, bool newBest)
    {
        if (EventSystem.current == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(InputSystemUIInputModule));

        GameObject ui = new GameObject("EndGameOverlay", typeof(Canvas),
            typeof(CanvasScaler), typeof(GraphicRaycaster));
        ui.transform.SetParent(transform, false);
        Canvas canvas = ui.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 200;
        CanvasScaler scaler = ui.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        GameObject panel = new GameObject("Backdrop", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(ui.transform, false);
        RectTransform rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        panel.GetComponent<Image>().color = new Color(0.02f, 0.03f, 0.06f, 0.92f);

        AddText(panel.transform, "DELIVERY COMPLETE!", 150f, 60);
        AddText(panel.transform, "Your time: " + FormatTime(elapsed), 45f, 40);
        AddText(panel.transform, "Best time: " + FormatTime(best), -20f, 40);
        if (newBest) AddText(panel.transform, "NEW BEST!", -85f, 30);

        GameObject buttonObject = new GameObject("RestartButton", typeof(RectTransform), typeof(Image), typeof(Button));
        buttonObject.transform.SetParent(panel.transform, false);
        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.sizeDelta = new Vector2(320, 80);
        buttonRect.anchoredPosition = new Vector2(0, -190);
        buttonObject.GetComponent<Image>().color = new Color(0.16f, 0.45f, 0.7f);
        Text label = AddText(buttonObject.transform, "Restart", 0f, 36);
        label.rectTransform.sizeDelta = new Vector2(320, 80);
        Button button = buttonObject.GetComponent<Button>();
        button.onClick.AddListener(RestartGame);
        EventSystem.current.SetSelectedGameObject(buttonObject);
    }

    private static Text AddText(Transform parent, string content, float y, int size)
    {
        GameObject obj = new GameObject("Label", typeof(RectTransform), typeof(Text));
        obj.transform.SetParent(parent, false);
        Text label = obj.GetComponent<Text>();
        label.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        label.text = content;
        label.fontSize = size;
        label.color = Color.white;
        label.alignment = TextAnchor.MiddleCenter;
        label.raycastTarget = false;
        label.rectTransform.sizeDelta = new Vector2(1100, 90);
        label.rectTransform.anchoredPosition = new Vector2(0, y);
        return label;
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.buildIndex < 0)
        {
            Debug.LogError("Add this scene to the active Build Profile scene list to enable restarting.", this);
            return;
        }
        Time.timeScale = previousTimeScale;
        ownsPause = false;
        gameEnded = false;
        SceneManager.LoadScene(scene.buildIndex);
    }

    private void OnDestroy()
    {
        if (!ownsPause) return;
        Time.timeScale = previousTimeScale;
        gameEnded = false;
    }
}
