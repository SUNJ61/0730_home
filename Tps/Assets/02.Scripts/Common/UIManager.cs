using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    public void OnClickPlayBtn() //버튼이 눌러졌을 때 씬을 변경
    {
        SceneManager.LoadScene("Level_1"); //Level_1씬을 불러온다.
        SceneManager.LoadScene("BattleFieldScene",LoadSceneMode.Additive);//LoadSceneMode.Additive는
        //기존씬(Level_1씬)을 삭제하지 않고 BattleFieldScene씬을 추가해서 새로운 씬을 로드한다. (즉, 씬 병합)
        //LoadSceneMode.Single은 기존 씬을 삭제하고 ()안에 선언된 새로운 씬을 로드한다.
    }
    public void OnClickQuitBtn()
    {
#if UNITY_EDITOR
        //실행중인 어플리케이션 종료
        EditorApplication.isPlaying = false; //유니티에서 종료
#else
    Application.Quit(); // 출시하려고 빌드 한 화면에서 종료, 유저에서 종료
#endif
    }
}
