using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LoveTask
{
    public string title;
    public string description;
}

public class LoveRoulette : MonoBehaviour
{
    [Header("Wheel")]
    public RectTransform wheelTransform;
    public TMP_Text[] wheelTexts; // 8 alan

    [Header("Buttons")]
    public Button spinButton;
    public Button nextButton;

    [Header("Result Panel")]
    public GameObject resultPanel;
    public TMP_Text resultNameTMP;
    public TMP_Text resultTitleText;
    public TMP_Text resultDescriptionText;

    [Header("Name Displays")]
    public TMP_Text currentPlayerNameTMP; // Çark üstündeki oyuncu adı

    [Header("Tasks")]
    public List<LoveTask> allTasks = new List<LoveTask>();

    private List<LoveTask> currentTasks = new List<LoveTask>();

    private string manName;
    private string womanName;
    private bool isManTurn = true;

    private bool isSpinning = false;

    public PlayerNamesData playerNamesData;

    void Start()
    {
        LoadNames();
        UpdateCurrentPlayerName();
        AssignRandomTasksToWheel();

        spinButton.onClick.AddListener(Spin);
        nextButton.onClick.AddListener(Next);
        resultPanel.SetActive(false);
    }

    void LoadNames()
    {
        manName = playerNamesData.manNameMAIN;
        womanName = playerNamesData.womanNameMAIN;
    }

    void UpdateCurrentPlayerName()
    {
        string name = isManTurn ? manName : womanName;
        currentPlayerNameTMP.text = name;
    }

    void AssignRandomTasksToWheel()
    {
        currentTasks.Clear();
        List<int> used = new List<int>();

        for (int i = 0; i < wheelTexts.Length; i++)
        {
            int randIndex;
            do
            {
                randIndex = Random.Range(0, allTasks.Count);
            } while (used.Contains(randIndex));

            used.Add(randIndex);
            LoveTask task = allTasks[randIndex];
            currentTasks.Add(task);
            wheelTexts[i].text = task.title;
        }
    }

    void Spin()
    {
        if (!isSpinning)
            StartCoroutine(SpinWheel());
    }
    public void BackMainMenu()
    {
            SceneManager.LoadScene("MainScene");
    }

    IEnumerator SpinWheel()
    {
        isSpinning = true;
        spinButton.interactable = false;
        resultPanel.SetActive(false);

        int selectedIndex = Random.Range(0, currentTasks.Count);
        float anglePerSlice = 360f / currentTasks.Count;
        float targetAngle = anglePerSlice * selectedIndex + 360f * 5;
        float duration = 2.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float angle = Mathf.Lerp(0, targetAngle, Mathf.SmoothStep(0, 1, t));
            wheelTransform.localEulerAngles = new Vector3(0, 0, -angle);
            elapsed += Time.deltaTime;
            yield return null;
        }

        wheelTransform.localEulerAngles = new Vector3(0, 0, -targetAngle);

        LoveTask selectedTask = currentTasks[selectedIndex];
        string currentName = isManTurn ? manName : womanName;

        resultNameTMP.text = currentName;
        resultTitleText.text = selectedTask.title;
        resultDescriptionText.text = selectedTask.description;

        resultPanel.SetActive(true);
        isSpinning = false;
    }

    void Next()
    {
        resultPanel.SetActive(false);
        isManTurn = !isManTurn;
        UpdateCurrentPlayerName();
        spinButton.interactable = true;
    }
}
