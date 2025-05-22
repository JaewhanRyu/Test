using UnityEngine;
using TMPro;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class LoginManager : MonoBehaviour
{
   public GameObject RigisterPanel;
   public RectTransform EmailInputScale;
   public TMP_InputField EmailInput;
   public RectTransform PasswordInputScale;
   public TMP_InputField PasswordInput;
   public GameObject forgotPasswordPanel;
   public TextMeshProUGUI errorText;

   private Vector3 originalEmailInputScale;
   private Vector3 originalPasswordInputScale;

   public void Start()
   {
      originalEmailInputScale = EmailInputScale.localScale;
      originalPasswordInputScale = PasswordInputScale.localScale;

      EmailInput.onSelect.AddListener(OnEmailInputSlected);
      PasswordInput.onSelect.AddListener(OnPasswordInputSlected);

      EmailInput.onDeselect.AddListener(OnEmailInputDeslected);
      PasswordInput.onDeselect.AddListener(OnPasswordInputDeslected);
   }

   void OnEmailInputSlected(string text)
   {
      EmailInputScale.localScale = new Vector3(1.5f, 1.5f, 1);
   }
   void OnPasswordInputSlected(string text)
   {
      PasswordInputScale.localScale = new Vector3(1.5f, 1.5f, 1);
   }

   void OnEmailInputDeslected(string text)
   {
      EmailInputScale.localScale = originalEmailInputScale;
   }
   void OnPasswordInputDeslected(string text)
   {
      PasswordInputScale.localScale = originalPasswordInputScale;
   }

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
               errorText.color = Color.red;
               errorText.text = "Login Failed\nCheck your email or password";
               return;
            }

            FirebaseUser user = task.Result.User;
            Debug.Log("로그인 성공: " + user.Email);
            SceneManager.LoadScene("Main");
         });
   }
}
