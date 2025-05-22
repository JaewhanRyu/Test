using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using TMPro;

public class UIController : MonoBehaviour
{
  public TMP_InputField rigisterEmailInput;
  public TMP_InputField rigisterPasswordInput;
  public RectTransform rigisterEmailInputRect;
  public RectTransform rigisterPasswordInputRect;

  public RigisterManager rigisterManager;

  public TMP_InputField resetEmailInput;
  public RectTransform resetEmailInputRect;
  public TextMeshProUGUI resetStatusText;

  private Vector3 rigisterEmailInputRectOriginalScale;
  private Vector3 rigisterPasswordInputRectOriginalScale;
  private Vector3 resetEmailInputRectOriginalScale;

  void Start()
  {
    rigisterEmailInputRectOriginalScale = rigisterEmailInputRect.localScale;
    rigisterPasswordInputRectOriginalScale = rigisterPasswordInputRect.localScale;
    resetEmailInputRectOriginalScale = resetEmailInputRect.localScale;

    rigisterEmailInput.onSelect.AddListener(OnSelectedRigisterEmailInput);
    rigisterEmailInput.onDeselect.AddListener(OnDeselectedRigisterEmailInput);

    rigisterPasswordInput.onSelect.AddListener(OnSelectedRigisterPasswordInput);
    rigisterPasswordInput.onDeselect.AddListener(OnDeselectedRigisterPasswordInput);

    resetEmailInput.onSelect.AddListener(OnSelectedResetEmailInput);
    resetEmailInput.onDeselect.AddListener(OnDeselectedResetEmailInput);
  }


  void OnSelectedRigisterEmailInput(string text)
  {
    rigisterEmailInputRect.localScale = new Vector3(3f, 3f, 1.0f);
  }

  void OnSelectedRigisterPasswordInput(string text)
  {
    rigisterPasswordInputRect.localScale = new Vector3(3f, 3f, 1.0f);
  }

  void OnDeselectedRigisterEmailInput(string text)
  {
    rigisterEmailInputRect.localScale = rigisterEmailInputRectOriginalScale;
  }

  void OnDeselectedRigisterPasswordInput(string text)
  {
    rigisterPasswordInputRect.localScale = rigisterPasswordInputRectOriginalScale;
  }

  void OnSelectedResetEmailInput(string text)
  {
    resetEmailInputRect.localScale = new Vector3(3f, 3f, 1.0f);
  }

  void OnDeselectedResetEmailInput(string text)
  {
    resetEmailInputRect.localScale = resetEmailInputRectOriginalScale;
  }


  public void OnRigisterButton()
  {
    string email = rigisterEmailInput.text;
    string password = rigisterPasswordInput.text;

    rigisterManager.RigisterAccount(email, password);
  }

  public void SendResetPasswordEmail()
  {
    FirebaseAuthManager.auth.SendPasswordResetEmailAsync(resetEmailInput.text).ContinueWithOnMainThread(task => {
      if (task.IsFaulted || task.IsCanceled)
      {
        Debug.LogError("비밀번호 초기화 이메일 전송 실패: " + task.Exception);
        resetStatusText.text = "Failed to send reset email";
        resetStatusText.color = Color.red;
        return;
      }

      Debug.Log("비밀번호 초기화 이메일 전송 성공");
      resetStatusText.text = "Reset email sent successfully";
      resetStatusText.color = Color.green;
    });
  }
}
