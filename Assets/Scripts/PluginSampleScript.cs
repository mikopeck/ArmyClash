using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PluginSampleScript : MonoBehaviour
{
    Text enemyUnits;
    Text ourUnits;
    Text ourRangers;
    Text ourFootmen;
    Text ourHorsemen;
    Text fightInfo;
    Text round;
    Text turnComplete;
    Text lastRound;
    public static string _ourChoice;
    GameObject endScreen;

    public int rangerNo
	{
		get {return int.Parse(ourRangers.text.Replace("Available: ", string.Empty));}
        set { ourRangers.text = "Available: " + value; }
	}
    public int footmenNo
    {
        get { return int.Parse(ourFootmen.text.Replace("Available: ", string.Empty)); }
        set { ourFootmen.text = "Available: " + value; }
    }
    public int horsemenNo
    {
        get { return int.Parse(ourHorsemen.text.Replace("Available: ", string.Empty)); }
        set { ourHorsemen.text = "Available: " + value; }
    }

    public void Start()
    {
        enemyUnits = GameObject.Find("enemyUnitsText").GetComponent<Text>();
        ourUnits = GameObject.Find("ourUnitsText").GetComponent<Text>();
        fightInfo = GameObject.Find("fightText").GetComponent<Text>();
        round = GameObject.Find("roundNo").GetComponent<Text>();
        turnComplete = GameObject.Find("turnComplete").GetComponent<Text>();
        lastRound = GameObject.Find("lastRoundText").GetComponent<Text>();
        endScreen = GameObject.Find("EndMessage");
        endScreen?.SetActive(false);
        ourRangers = GameObject.Find("rangerNo").GetComponent<Text>();
        ourFootmen = GameObject.Find("footmenNo").GetComponent<Text>();
        ourHorsemen = GameObject.Find("horsemenNo").GetComponent<Text>();
    }

    public void CallFunction(string callerText) //call the PassText function in the .jslib file from inside of unity
    {
        if (turnComplete.text != "true")
            return;

        if (!UnitAvailable(callerText))
		{
            fightInfo.text = "You have no more "+callerText+".";
            return;
        }

        fightInfo.text = "You attacked with " + callerText + ".";
        _ourChoice = callerText;
        turnComplete.text = "false";
        round.text = (int.Parse(round.text) + 1).ToString();
        Plugin.SetFastData(round.text+callerText);
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
            Debug.LogError("Unexpected data recieved: "+enemyChoice);
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
            enemyUnits.text = (int.Parse(enemyUnits.text) - 1).ToString();
            ourUnits.text = (int.Parse(ourUnits.text) - 1).ToString();
            Kill(ourChoice);
            fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. Both died.";
		}
		else
		{
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
                enemyUnits.text = (int.Parse(enemyUnits.text) - 1).ToString();
                fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. The enemy lost.";
            }
            else
            {
                ourUnits.text = (int.Parse(ourUnits.text) - 1).ToString();
                fightInfo.text = $"Your {ourChoice} attacked their {enemyChoice}. You lost.";
                Kill(ourChoice);
            }
        }

        lastRound.text = "Last round: " + fightInfo.text;

        if(int.Parse(ourUnits.text) == 0 || int.Parse(enemyUnits.text) == 0)
		{
            endScreen.SetActive(true);
            Text endText = GameObject.Find("endText").GetComponent<Text>();

            if (int.Parse(enemyUnits.text) == 0 && int.Parse(ourUnits.text) == 0)
                endText.text = "Stalemate...";
            else if (int.Parse(enemyUnits.text) == 0)
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

    private void Kill(string unitType)
	{
        if (unitType == "horsemen")
            horsemenNo--;
        else if (unitType == "rangers")
            rangerNo--;
        else if (unitType == "footmen")
            footmenNo--;
        else
            Debug.LogError("Unexpected unit to kill: " + unitType);
	}

    private bool UnitAvailable(string unitType)
	{
        if (unitType == "horsemen")
            return horsemenNo > 0;
        else if (unitType == "rangers")
            return rangerNo > 0;
        else if (unitType == "footmen")
            return footmenNo > 0;
		else
		{
            Debug.LogError("Unexpected unit to kill: " + unitType);
            return false;
		}
    }
}