using UnityEngine;
using System.Collections.Generic;
using Firebase.Firestore;
using Firebase.Extensions;

public class CharacterStatSave : MonoBehaviour
{
    public CharacterJobData knightData;
    public CharacterJobData mageData;
    public CharacterJobData archerData;



    void Awake()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // 여기서 바로 저장 호출 가능
                SaveAllStats();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        });
    }

    void Start()
    {
        // Start에서는 아무것도 하지 않거나, firebaseReady가 true일 때만 저장
    }

    void SaveAllStats()
    {
        SaveStatToFirebase(knightData);
        SaveStatToFirebase(mageData);
        SaveStatToFirebase(archerData);
    }

    public Dictionary<string, System.Object> StatConvertToDictionary(CharacterJobData data)
    {
         var statDict = new Dictionary<string, System.Object>
         {
            { "JobName", data.jobName },
            { "Level", data.level },
            { "MaxHp", data.maxHp },
            { "AttackPower", data.attackPower },
            { "Defense", data.defense },
            { "DodgeRate", data.dodgeRate },
            { "AttackDelay", data.attackDelay },
            { "SkillCoolTimeDecreaseRate", data.skillCoolTimeDecreaseRate },
            { "MoveSpeed", data.moveSpeed },
            { "ViewRange", data.viewRange },
            { "AttackRange", data.attackRange },
            { "CurrentExp", data.currentExp },
            { "MaxExp", data.maxExp },
            { "CurrentGold", data.currentGold },
            { "MaxExpRateIncreaseRate", data.maxExpRateIncreaseRate }
         };
         return statDict;
    }

    public void SaveStatToFirebase(CharacterJobData data)
    {
            FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
            var dic = StatConvertToDictionary(data);
            db.Collection("JobStats").Document(data.jobName).SetAsync(dic).ContinueWithOnMainThread(task =>
            {
                    if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
                    {
                        Debug.Log("데이터 저장 완료");
                    }
                    else
                    {
                        Debug.Log("데이터 저장 실패");
                        if (task.Exception != null)
                            Debug.LogError(task.Exception.ToString());
                    }
                
               
            });
        }
    }

