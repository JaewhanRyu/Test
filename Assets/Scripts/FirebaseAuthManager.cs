using UnityEngine;
using Firebase; //파이어 베이스 기본기능
using Firebase.Auth; //파이어베이스 로그인/회원가입 관련 기능

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuth auth;

    void Awake() //파이어베이스 사용하기 위한 초기화 과정
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // 파이어베이스 초기화
                auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase 초기화 완료");
            }
            else
            {
                Debug.LogError("Firebase 초기화 실패: " + dependencyStatus);
            }
        });
    }
}
