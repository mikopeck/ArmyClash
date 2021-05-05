using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyBtn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    public void ReadyBtnClick()
    {
        GameObject.Find("ReadyText").GetComponent<Text>().text = "Waiting for opponent...";
        GameObject.Find("ReadyText").GetComponent<CanvasRenderer>().SetAlpha(0);
        GameObject.Find("ReadyText").GetComponent<Text>().CrossFadeColor(Color.white, 1, false, true);
        InvokeRepeating("TryGetData", 0, 2);
        Plugin.SetFastData("ready");
    }

    public void TryGetData()
	{
        Plugin.GetFastData();
	}
}