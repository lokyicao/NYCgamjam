using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroSceneTransition : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Register()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "intro") return;
        GameObject controller = new GameObject("IntroSceneTransition");
        SceneManager.MoveGameObjectToScene(controller, scene);
        controller.AddComponent<IntroSceneTransition>();
    }

    private IEnumerator Start()
    {
        CreateIntroText();
        yield return new WaitForSecondsRealtime(5f);
        if (!Application.CanStreamedLevelBeLoaded("emma"))
        {
            Debug.LogError("Add emma to the active Build Profile scene list.", this);
            yield break;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("emma");
    }

    private void CreateIntroText()
    {
        GameObject ui = new GameObject("IntroTextCanvas", typeof(Canvas), typeof(CanvasScaler));
        ui.transform.SetParent(transform, false);
        ui.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        ui.GetComponent<Canvas>().sortingOrder = 100;
        CanvasScaler scaler = ui.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        AddText(ui.transform, "EARLY PIGEON GETS THE CRUMBS", 170f, 54, new Color(1f, 0.85f, 0.4f));
        AddText(ui.transform, "New York never sleeps. Neither does a hungry pigeon.", 45f, 34, Color.white);
        AddText(ui.transform, "One delivery. A city of obstacles. Crumbs on the line.", -25f, 34, Color.white);
        AddText(ui.transform, "Placeholder", -140f, 30, new Color(1f, 0.85f, 0.4f));
    }

    private static void AddText(Transform parent, string message, float y, int fontSize, Color color)
    {
        GameObject obj = new GameObject("IntroLabel", typeof(RectTransform), typeof(Text), typeof(Outline));
        obj.transform.SetParent(parent, false);
        Text text = obj.GetComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.text = message;
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = color;
        text.raycastTarget = false;
        text.rectTransform.sizeDelta = new Vector2(1600, 100);
        text.rectTransform.anchoredPosition = new Vector2(0, y);
        Outline outline = obj.GetComponent<Outline>();
        outline.effectColor = new Color(0f, 0f, 0f, 0.9f);
        outline.effectDistance = new Vector2(2f, -2f);
    }
}
