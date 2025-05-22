using UnityEngine;
using Firebase.Auth;

public class RigisterManager : MonoBehaviour
{
    public void RigisterAccount(string email, string password)
    {
        FirebaseAuthManager.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("회원가입 실패");
                return;
            }
            
            FirebaseUser user = task.Result.User;
            Debug.Log("회원가입 성공");
        });
    }
}
