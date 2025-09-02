using UnityEngine;

[CreateAssetMenu(fileName = "PlayerNamesData", menuName = "GameData/Player Names")]
public class PlayerNamesData : ScriptableObject
{
    [Header("Main Player Names")]
    public string manNameMAIN;
    public string womanNameMAIN;
}
