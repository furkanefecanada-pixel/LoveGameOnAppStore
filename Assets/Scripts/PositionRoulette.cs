using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[System.Serializable]
public class PositionData
{
    public string positionName;
   // public string positionDescription;
    public Sprite positionSprite;
}

public class PositionRoulette : MonoBehaviour
{
    public RectTransform wheelTransform;
    public Button spinButton;
    public GameObject resultPanel;
    public TMP_Text resultText;
   // public TMP_Text resultDescriptionText;
    public Image resultImage;
    public Button nextButton;

    public PositionData[] positions; // tüm pozisyonlar
    public TMP_Text[] rouletteTexts; // 9 TMP_Text

    private List<PositionData> currentRouletteOptions = new List<PositionData>();
    private bool isSpinning = false;

    public Button TimerStartBtn;
    public GameObject TimerPanel;
    public TMP_Text TimerTMP;
     private Coroutine countdownCoroutine;

       public void BackMainMenu()
    {
            SceneManager.LoadScene("MainScene");
    }


    void Start()
    {
        TimerStartBtn.onClick.AddListener(TimerStart);
        spinButton.onClick.AddListener(Spin);
        nextButton.onClick.AddListener(HideResultPanel);
        resultPanel.SetActive(false);
        AssignRandomPositionsToWheel();
    }

    public void TimerStart()
    {
        TimerPanel.SetActive(true);

         if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine); // Daha önce çalışıyorsa durdur
        }

        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }
    private IEnumerator CountdownRoutine()
    {
        // 3dk = 180sn, 10dk = 600sn
        float remainingTime = Random.Range(180f, 600f);

        while (remainingTime > 0)
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            TimerTMP.text = $"{minutes:00}:{seconds:00}";

            yield return new WaitForSeconds(1f);
            remainingTime -= 1f;
        }

        TimerTMP.text = "00:00";
        
    }

    void AssignRandomPositionsToWheel()
    {
        currentRouletteOptions.Clear();
        List<int> usedIndices = new List<int>();

        for (int i = 0; i < rouletteTexts.Length; i++)
        {
            int randIndex;

            // Duplicate önleme
            do
            {
                randIndex = Random.Range(0, positions.Length);
            } while (usedIndices.Contains(randIndex));

            usedIndices.Add(randIndex);
            PositionData selectedData = positions[randIndex];
            currentRouletteOptions.Add(selectedData);
            rouletteTexts[i].text = selectedData.positionName;
        }
    }

    public void Spin()
    {
        if (!isSpinning)
            StartCoroutine(SpinWheel());
    }

    IEnumerator SpinWheel()
    {
        isSpinning = true;
        spinButton.interactable = false;
        resultPanel.SetActive(false);

        int selectedIndex = Random.Range(0, currentRouletteOptions.Count);
        float anglePerSlice = 360f / currentRouletteOptions.Count;
        float targetAngle = anglePerSlice * selectedIndex + 360f * 5; // 5 tur
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

        // Sonucu göster
        PositionData selected = currentRouletteOptions[selectedIndex];
        resultText.text = selected.positionName;
        //resultDescriptionText.text = selected.positionDescription;
        resultImage.sprite = selected.positionSprite;
        resultPanel.SetActive(true);

        isSpinning = false;
    }

    public void HideResultPanel()
    {
        resultPanel.SetActive(false);
        spinButton.interactable = true;
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine); // Daha önce çalışıyorsa durdur
        }
        TimerPanel.SetActive(false);

        // Opsiyonel: her spin sonrası tekrar random dizilim istersen bunu aç:
        // AssignRandomPositionsToWheel();
    }
}
