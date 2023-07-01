using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonTypeScript : MonoBehaviour
{
    public ButtonType clickType;
    public void OnBtnClick(){
        switch(clickType){
            case ButtonType.New:
                Debug.Log("새 게임 시작하기");
                SceneManager.LoadScene("EpisodeListScene");
                break;
            case ButtonType.Continue:
                Debug.Log("게임 불러오기");
                break;
            case ButtonType.Quit:
                Debug.Log("게임 나가기");
                Application.Quit();
                break;
            case ButtonType.Liking:
                Debug.Log("호감도");
                SceneManager.LoadScene("EpisodeListScene");
                break;
            case ButtonType.Setting:
                Debug.Log("설정");
                SceneManager.LoadScene("SettingScene");
                break;
        }
    }
}
