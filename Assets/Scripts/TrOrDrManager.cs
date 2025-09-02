using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TrOrDrManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text middleTMP;
    public GameObject truthPanel;
    public GameObject darePanel;
    public TMP_Text truthTitleText;
    public TMP_Text truthDescriptionText;
    public TMP_Text dareTitleText;
    public TMP_Text dareDescriptionText;

    [Header("Truth & Dare Lists")]
    public List<string> truthDescriptions = new List<string>();
    public List<string> dareDescriptions = new List<string>();

    private string manName;
    private string womanName;
    private string currentPlayer;
    private bool isManTurn; // true: man, false: woman
    public PlayerNamesData playerNamesData;
    
     public void BackMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    void Start()
    {
        LoadPlayerNames();
        ChooseRandomStartPlayer();
        UpdatePlayerDisplay();
        HidePanels();
    }

    void LoadPlayerNames()
    {
        manName = playerNamesData.manNameMAIN;
        womanName = playerNamesData.womanNameMAIN;
    }

    void ChooseRandomStartPlayer()
    {
        isManTurn = Random.value > 0.5f;
        currentPlayer = isManTurn ? manName : womanName;
    }

    void UpdatePlayerDisplay()
    {
        currentPlayer = isManTurn ? manName : womanName;
        middleTMP.text = currentPlayer;
    }

    public void OnTruthButtonClicked()
    {
        HidePanels();
        truthPanel.SetActive(true);

        truthTitleText.text = currentPlayer;
        truthDescriptionText.text = GetRandomFromList(truthDescriptions);
    }

    public void OnDareButtonClicked()
    {
        HidePanels();
        darePanel.SetActive(true);

        dareTitleText.text = currentPlayer;
        dareDescriptionText.text = GetRandomFromList(dareDescriptions);
    }

    public void OnNextButtonClicked()
    {
        HidePanels();
        // Sırayı değiştir
        isManTurn = !isManTurn;
        UpdatePlayerDisplay();
    }

    void HidePanels()
    {
        truthPanel.SetActive(false);
        darePanel.SetActive(false);
    }

    string GetRandomFromList(List<string> list)
    {
        if (list.Count == 0)
            return "No content available.";
        return list[Random.Range(0, list.Count)];
    }
}
