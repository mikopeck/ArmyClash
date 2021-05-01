using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PluginSampleScript : MonoBehaviour
{
    Text enemyHp;
    Text ourHp;
    Text fightInfo;
    Text round;
    Text turnComplete;
    Text lastRound;
    public static string _ourChoice;
    GameObject endScreen;

    public void Start()
    {
        enemyHp = GameObject.Find("enemyHpText").GetComponent<Text>();
        ourHp = GameObject.Find("ourHpText").GetComponent<Text>();
        fightInfo = GameObject.Find("fightText").GetComponent<Text>();
        round = GameObject.Find("roundNo").GetComponent<Text>();
        turnComplete = GameObject.Find("turnComplete").GetComponent<Text>();
        lastRound = GameObject.Find("lastRoundText").GetComponent<Text>();
        endScreen = GameObject.Find("EndMessage");
        endScreen?.SetActive(false);
    }

    public void CallFunction(string callerText) //call the PassText function in the .jslib file from inside of unity
    {
        if (turnComplete.text == "true")
		{
            fightInfo.text = "You attacked with " + callerText + ".";
            _ourChoice = callerText;
            turnComplete.text = "false";
            round.text = (int.Parse(round.text) + 1).ToString();
            Plugin.SetFastData(round.text+callerText);
		}
    }
    public void CallExternalFunction(string enemyChoice) // Called when receiving data
    {
        if (enemyChoice == "ready") // Start fight.
        {
            GameObject.Find("WelcomeMessage").SetActive(false);
            return;
        }

        if (!int.TryParse(enemyChoice.Substring(0, 1), out int enemyRound))
		{
            Debug.Log("Unexpected data recieved: "+enemyChoice);
            return;
		}

        int roundNumber = int.Parse(round.text);

        if (turnComplete.text == "true") // No moves made from you.
        {
            if (roundNumber < enemyRound) // They moved - you didn't.
            {
                fightInfo.text = "Choose a fight strategy below. The enemy is waiting...";
                return;
            }
            fightInfo.text = "Choose a fight strategy below."; // Nobody moved.
            return;
        }

        if (roundNumber > enemyRound) // You moved - they didn't.
		{
            fightInfo.text = "You have sent " + _ourChoice+".\nWait for enemy response...";
            return;
        }

        // Fight!!
        enemyChoice = enemyChoice.Substring(1);
        Fight(_ourChoice, enemyChoice);
    }

    // Determine fight outcome and adjust HP.
    public void Fight(string ourChoice, string enemyChoice)
    {
        turnComplete.text = "true";

        if (ourChoice == enemyChoice)
		{
            enemyHp.text = (int.Parse(enemyHp.text) - 10).ToString();
            ourHp.text = (int.Parse(ourHp.text) - 10).ToString();
            fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. You each lost 10 hp.";
            lastRound.text = "Last round: " + fightInfo.text;
            return;
		}

        bool win = false;
        if (ourChoice == "horsemen") 
        {
            win = enemyChoice == "rangers";
        }
        else if (ourChoice == "rangers")
		{
            win = enemyChoice == "footmen";
        }
        else if (ourChoice == "footmen")
		{
            win = enemyChoice == "horsemen";
		}

        if (win)
        {
            enemyHp.text = (int.Parse(enemyHp.text) - 20).ToString();
            fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. The enemy lost 20 hp.";
        }
        else
        {
            ourHp.text = (int.Parse(ourHp.text) - 20).ToString();
            fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. You lost 20 hp.";
        }

        lastRound.text = "Last round: " + fightInfo.text;

        if(int.Parse(ourHp.text) <= 0 || int.Parse(enemyHp.text) <= 0)
		{
            endScreen.SetActive(true);
            Text endText = GameObject.Find("endText").GetComponent<Text>();

            if (int.Parse(enemyHp.text) <= 0)
                endText.text = "Victory!";
            else
                endText.text = "Defeat...";

            Plugin.AddContentRecord(endText.text);
        }
    }

    public void MySkyLogin()
	{
        Plugin.InitMySky();
	}

    public void PlayAgain()
	{
        Plugin.ReloadGame();
	}
}