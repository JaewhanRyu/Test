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
            CreateCharacterFunction();
        });
    }

    public void CreateCharacterFunction()
    {
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
            character.GetComponent<CharacterStat>().InitStat(jobDt);
        }
    }
}
