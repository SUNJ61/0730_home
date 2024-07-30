using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //�� ���� ����� ����ϰڴ�. ScenManagement ������ ���

public class UIManager : MonoBehaviour // MonoBehaviour�� ����Ƽ�� ����� �ҷ����ڴ�. (���۳�Ʈ �ҷ����� ��)
{
    public Text FinKill;

    private void Start()
    {
        FinKill = GameObject.Find("Text_FinalKill").GetComponent<Text>();
        FinKill.text = $"KILL Score : {GameManager.Instance.gameData.KillCount.ToString()}";

        Cursor.visible = true; //Ŀ���� �ٽ� ��Ÿ�����Ѵ�.
        Cursor.lockState = CursorLockMode.None; //���콺 ��������
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("PlayScene");
    }

    public void QuitGame()
    {
#if UNITY_EDITOR //��ó���� (�������� �̸� ����� ������ ����.)
        UnityEditor.EditorApplication.isPlaying = false; //����Ƽ���� �������� ���¿��� ����
#else //���忡�� ���� (exe���� �����ε�)
        Application.Quit();
#endif
    }
}
