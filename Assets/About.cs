using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class About : MonoBehaviour
{
    GameObject aboutPanel;
    Text aboutButtonTxt;
    bool active;

    // Start is called before the first frame update
    void Start()
    {
        aboutButtonTxt = GameObject.Find("AboutText").GetComponent<Text>();
        aboutPanel = GameObject.Find("AboutPanel");
        aboutPanel.SetActive(false);
        active = false;
    }

    public void AboutClick()
	{
		if (active)
		{
            aboutPanel.SetActive(false);
            active = false;
            aboutButtonTxt.text = "About";
		}
		else
		{
            aboutPanel.SetActive(true);
            active = true;
            aboutButtonTxt.text = "Close";
        }
	}
}
