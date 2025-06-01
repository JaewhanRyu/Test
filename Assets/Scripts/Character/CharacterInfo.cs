using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    public string characterName;
    public int level;
    public float currentPlayTime;
    public float maxPlayTime;
    public float playTimeRecoveryRate;

    public int currentExp;
    public int maxExp;

    public int gold;

    void Awake()
    {
        currentPlayTime = maxPlayTime;
    }

}
