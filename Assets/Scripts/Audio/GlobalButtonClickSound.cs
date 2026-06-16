using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalButtonClickSound : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        RegisterAllButtons();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RegisterAllButtons();
    }

    private void RegisterAllButtons()
    {
        // Finds ALL buttons in the scene, including inactive ones
        Button[] buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in buttons)
        {
            // Remove first to avoid stacking duplicates on scene reload
            button.onClick.RemoveListener(OnButtonClicked);
            button.onClick.AddListener(OnButtonClicked);
        }
    }

    private void OnButtonClicked()
    {
        AudioManager.Instance?.PlayButtonClick();
    }
}
