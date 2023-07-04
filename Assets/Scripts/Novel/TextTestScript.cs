using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;
using UnityEngine.UI;
using TMPro;

public class TextTestScript : MonoBehaviour
{
    public Flowchart flowchart;
    public TextMeshProUGUI texttext;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(flowchart.GetExecutingBlocks().Count != 0&& flowchart.GetExecutingBlocks()[0].BlockName == "exit")
        {
            texttext.text = flowchart.GetIntegerVariable("TestVar").ToString();
            PlayerPrefs.SetInt("testData", flowchart.GetIntegerVariable("TestVar"));
        }
        else if(flowchart.GetExecutingBlocks().Count != 0 && flowchart.GetExecutingBlocks()[0].BlockName == "not exit")
        {
            texttext.text = flowchart.GetIntegerVariable("TestVar").ToString();
        }
    }
}
