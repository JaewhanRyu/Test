using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class LoginManager : MonoBehaviour
{
   public GameObject RigisterPanel;
   public TMP_InputField EmailInput;
   public TMP_InputField PasswordInput;
   public GameObject forgotPasswordPanel;

   public void RigisterButtonClick()
   {
    RigisterPanel.SetActive(true);
   }

   public void CloseRigisterPanelClick()
   {
    RigisterPanel.SetActive(false);
   }

   public void ForgotPasswordButtonClick()
   {
      forgotPasswordPanel.SetActive(true);
   }

   public void CloseForgotPasswordPanelClick()
   {
      forgotPasswordPanel.SetActive(false);
   }

   public void LoginButtonClick()
   {
      FirebaseAuthManager.auth.SignInWithEmailAndPasswordAsync(EmailInput.text, PasswordInput.text)
         .ContinueWithOnMainThread(task => {
            if (task.IsFaulted || task.IsCanceled)
            {
               Debug.LogError("로그인 실패: " + task.Exception);
               return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("로그인 성공: " + user.Email);
            SceneManager.LoadScene("Main");
         });
   }
}
