using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public Button[] buttons;
    public string[] options;

	void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void LoadScene(int _iSceneNum)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(_iSceneNum);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void Solve()
    {
        for (int i = 0; i < buttons.Length; ++i) { 
            buttons[i].gameObject.SetActive(true);
            buttons[i].GetComponentInChildren<Text>().text = options[i]; 
        }
    }
}
