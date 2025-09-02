using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class PerfectSyncM : MonoBehaviour
{
    public TMP_Text manNameTmp;
    public TMP_Text womanNameTmp;
    public TMP_Text resultTmp;

    public HoldButton leftHoldButton;
    public HoldButton rightHoldButton;

    public List<string> highMatchSentences;
    public List<string> mediumMatchSentences;
    public List<string> lowMatchSentences;

    private bool testStarted = false;
    private float matchValue;

    public PlayerNamesData playerNamesData;

    private void Start()
    {
        string manName = playerNamesData.manNameMAIN;
        string womanName = playerNamesData.womanNameMAIN;  

        manNameTmp.text = manName;
        womanNameTmp.text = womanName;

        resultTmp.text = "Hold both sides to begin";
    }

    private void Update()
    {
        if (!testStarted && leftHoldButton.isHoldingLeft && rightHoldButton.isHoldingRight)
        {
            testStarted = true;
            StartMatchTest();
        }
    }

    void StartMatchTest()
    {
        string manName = manNameTmp.text;
        string womanName = womanNameTmp.text;

        matchValue = Random.Range(0f, 100f);
        string sentence = GetSentenceByMatch(matchValue);

        resultTmp.text = manName + " + " + womanName + "\nMatch: %" + Mathf.RoundToInt(matchValue) + "\n" + sentence;
    }

    string GetSentenceByMatch(float value)
    {
        if (value >= 80)
        {
            return GetRandomSentence(highMatchSentences, "You have a strong physical connection.");
        }
        else if (value >= 50)
        {
            return GetRandomSentence(mediumMatchSentences, "There is potential for a good match.");
        }
        else
        {
            return GetRandomSentence(lowMatchSentences, "You might have different desires.");
        }
    }

    string GetRandomSentence(List<string> list, string fallback)
    {
        if (list != null && list.Count > 0)
        {
            return list[Random.Range(0, list.Count)];
        }
        return fallback;
    }


}
