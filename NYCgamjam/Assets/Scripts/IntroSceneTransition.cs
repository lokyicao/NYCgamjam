using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneTransition : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Register()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        yield return new WaitForSecondsRealtime(5f);
        if (!Application.CanStreamedLevelBeLoaded("emma"))
        {
            Debug.LogError("Add emma to the active Build Profile scene list.", this);
            yield break;
        }
        Time.timeScale = 1f;
        SceneManager.LoadScene("emma");
    }
}
