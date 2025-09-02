using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Roleplay
{
    public Sprite roleImage;
    public string scenarioDescription;
    public string maleDialogue;
    public string femaleDialogue; 

}

public class RoleplayManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image roleImageDisplay;
    public Button selectRoleButton;
    public GameObject startButton;
    public GameObject readPanel;
    public TMP_Text dialogueText;
    public Button nextButton;

    [Header("Roleplay List")]
    public List<Roleplay> roleplays = new List<Roleplay>();

    private Roleplay selectedRoleplay;
    private int dialogueIndex = 0;
    private string[] currentDialogues;

    public TMP_Text TitleForPanel;

        private string manName;
    private string womanName;

public PlayerNamesData playerNamesData;

    void Start()
    {
        startButton.SetActive(false);
        readPanel.SetActive(false);
        nextButton.onClick.AddListener(NextDialogue);
        manName = playerNamesData.manNameMAIN;
        womanName = playerNamesData.womanNameMAIN;
        
    }

    public void OnSelectRole()
    {
        StartCoroutine(SelectRandomRoleCoroutine());
    }

    IEnumerator SelectRandomRoleCoroutine()
    {
        startButton.SetActive(false);
        readPanel.SetActive(false);

        float timer = 0f;
        float duration = 2.5f;
        while (timer < duration)
        {
            int randomIndex = Random.Range(0, roleplays.Count);
            roleImageDisplay.sprite = roleplays[randomIndex].roleImage;
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        selectedRoleplay = roleplays[Random.Range(0, roleplays.Count)];
        roleImageDisplay.sprite = selectedRoleplay.roleImage;

        yield return new WaitForSeconds(0.5f);
        startButton.SetActive(true);
    }

    public void OnStartRoleplay()
    {
        if (selectedRoleplay == null) return;

        readPanel.SetActive(true);
        dialogueIndex = 0;

        currentDialogues = new string[]
        {
            selectedRoleplay.scenarioDescription,
            selectedRoleplay.maleDialogue,
            selectedRoleplay.femaleDialogue
        };

        dialogueText.text = currentDialogues[dialogueIndex];

 
    }

    void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < currentDialogues.Length)
        {
            dialogueText.text = currentDialogues[dialogueIndex];
        }
        else
        {
            readPanel.SetActive(false);
            startButton.SetActive(false);
        }
               if (dialogueIndex == 0)
        {
            TitleForPanel.text = "Scenario";
        }
        if (dialogueIndex == 1)
        {
            TitleForPanel.text = manName;
        }
             if (dialogueIndex == 2)
        {
            TitleForPanel.text = womanName;
        }
    }
}
