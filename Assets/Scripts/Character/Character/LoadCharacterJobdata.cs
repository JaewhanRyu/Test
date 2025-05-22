using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections.Generic;

//캐릭터 직업별 기본 능력치 파이어베이스에서 로드
public class LoadCharacterJobdata : MonoBehaviour
{



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
                CharacterJobData jobData = new CharacterJobData();

                jobData.jobName = (string)statDic["JobName"];
                jobData.level = (int)(long)statDic["Level"];
                jobData.maxHp = (int)(long)statDic["MaxHp"];
                jobData.attackPower = (int)(long)statDic["AttackPower"];
                jobData.defense = (int)(long)statDic["Defense"];
                jobData.dodgeRate = (float)statDic["DodgeRate"];
                jobData.attackDelay = (float)statDic["AttackDelay"];
                jobData.skillCoolTimeDecreaseRate = (float)statDic["SkillCoolTimeDecreaseRate"];
                jobData.moveSpeed = (float)statDic["MoveSpeed"];
                jobData.viewRange = (float)statDic["ViewRange"];
                jobData.attackRange = (float)statDic["AttackRange"];
                jobData.currentGold = (int)(long)statDic["CurrentGold"];
                jobData.currentExp = (int)(long)statDic["CurrentExp"];
                jobData.maxExp = (int)(long)statDic["MaxExp"];
                jobData.maxExpRateIncreaseRate = (float)statDic["MaxExpRateIncreaseRate"];

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
