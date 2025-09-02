using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainManager : MonoBehaviour
{
    public GameObject SubsPanel;
    public GameObject SettingsPanel;
    public GameObject SetNamesPanel; // <-- eklendi

    [Header("Auto-Subscribe Prompt")]
    [SerializeField] BuyingSubscription buyingSubscription; // Scene’den sürükle-bırak (opsiyonel)
    [SerializeField] bool enableAutoSubsNag = true;
    [SerializeField] float subsNagIntervalSeconds = 9f;

    Coroutine subsNagRoutine;

    void Start()
    {
        if (!buyingSubscription) buyingSubscription = FindObjectOfType<BuyingSubscription>();

        if (enableAutoSubsNag)
            subsNagRoutine = StartCoroutine(AutoOpenSubsIfNoSubscription());
    }

    IEnumerator AutoOpenSubsIfNoSubscription()
    {
        var wait = new WaitForSeconds(subsNagIntervalSeconds);

        while (true)
        {
            yield return wait;

            if (!HasActiveSubscription() && !IsAnyBlockingPanelOpen())
            {
                if (SubsPanel && !SubsPanel.activeSelf)
                    SubsPanel.SetActive(true);
            }
            else
            {
                // Abone olunduysa istersen döngüyü bitirebilirsin:
                // yield break;
            }
        }
    }

    bool HasActiveSubscription()
    {
        if (!buyingSubscription) return false;

        // Şimdilik UI text üzerinden karar veriyoruz (boş değilse abone varsay).
        var txt = buyingSubscription.subscriptionStatusText;
        return txt && !string.IsNullOrEmpty(txt.text);
    }

    // SetNames veya Settings açıksa “bloklayıcı”
    bool IsAnyBlockingPanelOpen()
    {
        bool settingsOpen = SettingsPanel && SettingsPanel.activeSelf;
        bool setNamesOpen = SetNamesPanel && SetNamesPanel.activeSelf;
        return settingsOpen || setNamesOpen;
    }

    public void LinkOpenBtn(string Link)
    {
        Application.OpenURL(Link);
    }

    public void GamesModesOn(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void OnSubscribePanel()
    {
        if (SubsPanel) SubsPanel.SetActive(true);
    }

    public void OnSettingsPanel()
    {
        if (SettingsPanel) SettingsPanel.SetActive(true);
    }

    public void OffSubsPanel()
    {
        if (SubsPanel) SubsPanel.SetActive(false);
    }

    public void OffSettingsPanel()
    {
        if (SettingsPanel) SettingsPanel.SetActive(false);
    }
}
