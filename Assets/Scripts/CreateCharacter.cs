using UnityEngine;
using System.Collections.Generic;

public class CreateCharacter : MonoBehaviour
{
    public static CreateCharacter instance;
    public int maxCharacterCount = 5;
    public GameObject knightPrefab;
    public GameObject archerPrefab;
    public GameObject magePrefab;

    public List<GameObject> characterList = new List<GameObject>();
    public Transform respawnPoint;
    public LoadCharacterJobdata loadCharacterJobdata;
    private CharacterJobData jobDt;
    private string jobName;

    private void Awake()
    {
        instance = this;
    }

    public void CreateButtonClick()
    {
        int randomNumber = Random.Range(0, 3);

        switch (randomNumber)
        {
            case 0:
                jobName = "Knight";
                break;
            case 1:
                jobName = "Archer";
                break;
            case 2:
                jobName = "Mage";
                break;
        }

        LoadCharacterJobdataHere();
    }

    public void LoadCharacterJobdataHere()
    {
        loadCharacterJobdata.LoadStatFromFirebase(jobName, (jobData) =>
        {
            jobDt = jobData;
            if (jobDt == null)
            {
                Debug.Log($"문서 없음: {jobName}");
            }
            else
            {
                CreateCharacterFunction();
            }
        });
    }

    public void CreateCharacterFunction()
    {
        if (jobDt == null)
        {
            Debug.LogError("jobDt가 null입니다! 캐릭터 스탯 데이터가 정상적으로 로드되지 않았습니다.");
            return;
        }

        if (characterList.Count < maxCharacterCount)
        {
            GameObject character = null;
            switch (jobName)
            {
                case "Knight":
                    character = Instantiate(knightPrefab, transform.position, Quaternion.identity);
                    break;
                case "Archer":
                    character = Instantiate(archerPrefab, transform.position, Quaternion.identity);
                    break;
                case "Mage":
                    character = Instantiate(magePrefab, transform.position, Quaternion.identity);
                    break;
            }
            characterList.Add(character);

            var stat = character.GetComponent<CharacterStat>();
            if (stat == null)
            {
                Debug.LogError("CharacterStat 컴포넌트가 프리팹에 없습니다!");
                return;
            }
            stat.InitStat(jobDt);
        }
    }
}
