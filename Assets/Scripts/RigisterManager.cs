using UnityEngine;
using Firebase.Auth;
using TMPro;
using Firebase.Extensions;

public class RigisterManager : MonoBehaviour
{
    public TextMeshProUGUI RigisterStatusText;

    public void RigisterAccount(string email, string password)
    {
        FirebaseAuthManager.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("회원가입 실패");
                RigisterStatusText.color = Color.red;
                RigisterStatusText.text = "Register Failed";
                return;
            }
            
            FirebaseUser user = task.Result.User;
            Debug.Log("회원가입 성공");
            RigisterStatusText.color = Color.green;
            RigisterStatusText.text = "Register Success";
        });
    }
}
