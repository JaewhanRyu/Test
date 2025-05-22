using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

//캐릭터 직업별 기본 능력치 파이어베이스에서 로드
public class LoadCharacterJobdata : MonoBehaviour
{
    CharacterJobData currentJobData;

  public void Start()
  {
    LoadStatFromFirebase("Knight", (jobData) =>
    {
        if(jobData != null)
        {
            currentJobData = jobData;
            Debug.Log("데이터 로드 성공");

            Debug.Log(currentJobData.jobName);
            Debug.Log(currentJobData.level);
            Debug.Log(currentJobData.maxHp);
            Debug.Log(currentJobData.attackPower);
            Debug.Log(currentJobData.defense);
            Debug.Log(currentJobData.dodgeRate);
            Debug.Log(currentJobData.attackDelay);
            Debug.Log(currentJobData.skillCoolTimeDecreaseRate);
            
        }
        else
        {
            Debug.Log("데이터 로드 실패");
        }    
    });
  }


    public void LoadStatFromFirebase(string jobName, System.Action<CharacterJobData> callback)
    {
        Debug.Log("LoadStatFromFirebase 실행");
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance; //파이어베이스 데이터베이스 초기화(사용준비)
        db.Collection("JobStats").Document(jobName).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted && !task.IsFaulted && !task.IsCanceled)
            {
               var snapshot = task.Result;

               Debug.Log("데이터 로드 시도");
               if(snapshot.Exists)
               {
               
                    Dictionary<string, object> statDic = snapshot.ToDictionary();
                    CharacterJobData jobData = ScriptableObject.CreateInstance<CharacterJobData>(); //ScriptableObject 인스턴스는 반드시 이런식으로 생성해야함.

                    jobData.jobName = (string)statDic["JobName"];
                    jobData.level = (int)(long)statDic["Level"];
                    jobData.maxHp = (int)(long)statDic["MaxHp"];
                    jobData.attackPower = (int)(long)statDic["AttackPower"];
                    jobData.defense = (int)(long)statDic["Defense"];
                    jobData.dodgeRate = statDic.ContainsKey("DodgeRate") ? System.Convert.ToSingle(statDic["DodgeRate"]) : 0f; //C#에서 float으로 바로 캐스팅하면 예외가 발생할 수 있기 때문에 System.Convert.ToSingle()로 변환해야 안전하게 float으로 변환됨
                    jobData.attackDelay = statDic.ContainsKey("AttackDelay") ? System.Convert.ToSingle(statDic["AttackDelay"]) : 0f;
                    jobData.skillCoolTimeDecreaseRate = statDic.ContainsKey("SkillCoolTimeDecreaseRate") ? System.Convert.ToSingle(statDic["SkillCoolTimeDecreaseRate"]) : 0f;
                    jobData.moveSpeed = statDic.ContainsKey("MoveSpeed") ? System.Convert.ToSingle(statDic["MoveSpeed"]) : 0f;
                    jobData.viewRange = statDic.ContainsKey("ViewRange") ? System.Convert.ToSingle(statDic["ViewRange"]) : 0f;
                    jobData.attackRange = statDic.ContainsKey("AttackRange") ? System.Convert.ToSingle(statDic["AttackRange"]) : 0f;
                    jobData.currentGold = (int)(long)statDic["CurrentGold"];
                    jobData.currentExp = (int)(long)statDic["CurrentExp"];
                    jobData.maxExp = (int)(long)statDic["MaxExp"];
                    jobData.maxExpRateIncreaseRate = statDic.ContainsKey("MaxExpRateIncreaseRate") ? System.Convert.ToSingle(statDic["MaxExpRateIncreaseRate"]) : 0f;             

                    callback?.Invoke(jobData);
                    Debug.Log("데이터 로드 성공");
               }
               else
               {
                Debug.Log("문서 없음");
               }
            }
            else
            {
                Debug.Log("데이터 로드 실패");
            }
        });
    }
}
