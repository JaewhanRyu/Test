using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    void Awake()
    {
        SceneManager.LoadScene("Field",LoadSceneMode.Additive);
    }
}
