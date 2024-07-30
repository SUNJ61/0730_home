using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public void OnClickPlayBtn() //��ư�� �������� �� ���� ����
    {
        SceneManager.LoadScene("Level_1"); //Level_1���� �ҷ��´�.
        SceneManager.LoadScene("BattleFieldScene",LoadSceneMode.Additive);//LoadSceneMode.Additive��
        //������(Level_1��)�� �������� �ʰ� BattleFieldScene���� �߰��ؼ� ���ο� ���� �ε��Ѵ�. (��, �� ����)
        //LoadSceneMode.Single�� ���� ���� �����ϰ� ()�ȿ� ����� ���ο� ���� �ε��Ѵ�.
    }
    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        //�������� ���ø����̼� ����
        EditorApplication.isPlaying = false; //����Ƽ���� ����
#else
    Application.Quit(); // ����Ϸ��� ���� �� ȭ�鿡�� ����, �������� ����
#endif
    }
}
