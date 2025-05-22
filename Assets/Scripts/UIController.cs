using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using TMPro;

public class UIController : MonoBehaviour
{
  public TMP_InputField emailInput;
  public TMP_InputField passwordInput;
  public RigisterManager registerManager;

  public TMP_InputField resetEmailInput;


  public void OnRigisterButton()
  {
    string email = emailInput.text;
    string password = passwordInput.text;

    registerManager.RigisterAccount(email, password);
  }

  public void SendResetPasswordEmail()
  {
    FirebaseAuthManager.auth.SendPasswordResetEmailAsync(resetEmailInput.text).ContinueWithOnMainThread(task => {
      if (task.IsFaulted || task.IsCanceled)
      {
        Debug.LogError("비밀번호 초기화 이메일 전송 실패: " + task.Exception);
        return;
      }

      Debug.Log("비밀번호 초기화 이메일 전송 성공");
    });
  }
}
