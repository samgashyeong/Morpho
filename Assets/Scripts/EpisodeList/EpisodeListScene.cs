using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class EpisodeListScene : MonoBehaviour
{
    public void SettingBtnOnCilck(){
        //
    }
    public void LikingAndEndingListBtnOnCilck(){
        Debug.Log("목록");
        SceneManager.LoadScene("LikingAndEndingListScene");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
