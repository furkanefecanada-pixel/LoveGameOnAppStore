using UnityEngine;
using TMPro; // TMP inputlar için

public class SetNamesManager : MonoBehaviour
{
    [Header("Input Fields")]
    public TMP_InputField manNameInput;
    public TMP_InputField womanNameInput;

    public TMP_Text ManNameText;
    public TMP_Text WomanNameText; 

    [Header("Set Names Panel")]
    public GameObject setNamesPanel; // Paneli buraya sürükle
    public PlayerNamesData playerNamesData;

    void Start()
    {
        string manName = PlayerPrefs.GetString("ManName", "Man");
        if (manName != "Man")
        {
            setNamesPanel.SetActive(true);
        }
        setNamesPanel.SetActive(true);
    }
    

    // Play fonksiyonu, butona bağlanacak!
    public void Play()
    {
        // Input field'larda yazan isimleri al
        string manName = manNameInput.text;
        string womanName = womanNameInput.text;

        // PlayerPrefs ile kaydet
        PlayerPrefs.SetString("ManName", manName);
        PlayerPrefs.SetString("WomanName", womanName);
        PlayerPrefs.Save();

        playerNamesData.manNameMAIN = manName;
        playerNamesData.womanNameMAIN = womanName;
        // Paneli kapat
        setNamesPanel.SetActive(false);
    }

    public void OnSetNamesPanel()
    {
        setNamesPanel.SetActive(true);
        ManNameText.text = PlayerPrefs.GetString("ManName", "Please Enter");
        WomanNameText.text = PlayerPrefs.GetString("WomanName", "Please Enter");

    }
}
