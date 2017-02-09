using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Overlay : MonoBehaviour
{

    private GameObject overlayText;
	private GameObject messageText;
	private GameObject nameText;
    private GameObject startButton;
    private GameObject exitButton;
    private GameObject restartButton;
    private GameObject pauseButton;
    private GameObject panel;
    private GameObject shadowPanel;
	private GameObject titleText;
	private GameObject continueButton;
    private GameObject creditButton;
	private GameObject startPanel;
    private GameObject conversation;
    private GameObject nextConvButton;
    private GameObject exitConvButton;

	private GameObject chestText;

    private string startText;
    private string loseText;
    private string winText;
    private string creditText;
    private List<String> conversationList;
    private int conversationIndex;
    private bool[] state;

	// Use this for initialization
	void Awake () {

        conversationList = new List<string>();
	    conversationIndex = 0;

        overlayText = GameObject.Find("Overlay Text");
        conversation = GameObject.Find("Conversation Text");
		messageText = GameObject.Find("Message Text");
		nameText = GameObject.Find("Name Text");
		startButton = GameObject.Find("Start Button");
        exitButton = GameObject.Find("Exit Button");
        restartButton = GameObject.Find("Restart Button");
	    pauseButton = GameObject.Find("Pause Button");
	    panel = GameObject.Find("Panel");
		shadowPanel = GameObject.Find("Shadow Panel");
		titleText = GameObject.Find("Title Text");
		continueButton = GameObject.Find("Continue Button");
        creditButton = GameObject.Find("Credit Button");
        startPanel = GameObject.Find("Start Panel");
		chestText = GameObject.Find ("ChestText");
	    nextConvButton = GameObject.Find("Continue Dialog Button");
        exitConvButton = GameObject.Find("Exit Dialog Button");
        
        overlayText.SetActive(false);
		nameText.SetActive (false);
        startButton.SetActive(false);
        exitButton.SetActive(false);
        restartButton.SetActive(false);
		continueButton.SetActive(false);
        creditButton.SetActive(false);
		pauseButton.SetActive(false);
		panel.SetActive(false);
        shadowPanel.SetActive(false);
		titleText.SetActive(false);
		chestText.SetActive(false);
        conversation.SetActive(false);
        nextConvButton.SetActive(false);
        exitConvButton.SetActive(false);

        SaveState();
	}

    public void SetStrings(int level)
    {
        switch (level)
        {
            case 0:
                //intro
                startText = "TODO";
                loseText = "TODO";
                winText = "TODO";
                conversationList.Add("Chip::Hello World!");
                conversationList.Add("");//triggers end of dialog
                break;
            case 1:
                //temple 1
                startText = "Chip, you have fallen to your death...\n\n" +
                    "You have been granted five minutes to revive yourself!\n\n" +
                    "You must solve all four trials in order to retrieve the artifacts from the chests!\n\n" +
                    "Use WASD to move, E to push/interact and click on the clock to reset time.\n\n";
                loseText = "You ran out of time!";
                winText = "Congratulations, you have rejoined the world of the living!\n\n" +
                    "The god of time isn't done with you yet...\n\n" +
                    "Your story is yet to unfold...\n\n" +
                    "Will you forever live in the shadow of your father?";
                conversationList.Add("Chip::First Line\nSecond Line");
                conversationList.Add("Chip::After Clicking Continue");
                conversationList.Add("");//triggers end of dialog
                break;
            case 2:
                //temple 2
                startText = "You may be reborn now and regained your strength, \n" +
						"But you still need to save your village. That's why \n" +
						"you are here after all. So go ahead and face this new challenge. \n" +
						"Looks like there's more of you here. Clones, as it's the Temple of Duality. \n\n" + 
						"Hint: you can now use E to interact with clones as well.";
                loseText = "You ran out of time!";
                winText = "Congratualions! You have saved your village!";
                conversationList.Add("Sister::My long lost brother, Xochipilli is your true name. \n" +
						"You're a God. My God. How have I missed you. \n Come, join me, my blood. Let's live with our kin now.");
                conversationList.Add("Chip::Sister?! A God, you say... then I must do what's right \n" +
						"I must return home to save my village. \n I am sorry, I will deeply miss you.");
                conversationList.Add("");//triggers end of dialog
                break;
        }

        creditText = "Team Two Cubed will return...\n\n" +
            "Raluca Gaina\n" +
            "o-s-s\n" +
            "Jonathan Nichols\n" +
            "Olivier Thill\n" +
            "Ovidio Villarreal\n" +
            "Paul Leonard\n" +
            "Harvey Wigton\n";
    }

	public void IntroCutScene()
	{
		nameText.SetActive (true);
		// add names and messages here
	}

    public void Introduction()
    {
        shadowPanel.SetActive(true);
		nameText.SetActive (false);
        overlayText.SetActive(true);
        overlayText.GetComponent<Text>().text = startText;
		titleText.SetActive(true);
		titleText.GetComponentInChildren<Text>().text = "REBIRTH";
		startButton.SetActive(true);
        exitButton.SetActive(false);
        restartButton.SetActive(false);
        pauseButton.SetActive(false);
    }

    public void MainGame()
    {
        overlayText.SetActive(false);
        shadowPanel.SetActive(false);
		startPanel.SetActive (false);
		titleText.SetActive(false);
        startButton.SetActive(false);
        pauseButton.SetActive(true);
        pauseButton.GetComponentInChildren<Text>().text = "Pause";
		nameText.SetActive (true);
    }

    public void EndGame()
    {
        overlayText.SetActive(true);
		nameText.SetActive (false);
        exitButton.SetActive(true);
		titleText.SetActive(true);
        restartButton.SetActive(true);
        pauseButton.SetActive(false);
		startPanel.SetActive (true);
    }

    public void GameOver()
    {
        overlayText.GetComponent<Text>().text = loseText;
		EndGame();
    }

    public void LevelFinished(int level)
    {
        switch (level)
        {
            case 0:
                //intro
                continueButton.SetActive(true);
                break;
            case 1:
                //temple 1
                continueButton.SetActive(true);
                break;
            case 2:
                //temple 2
                creditButton.SetActive(true);
                break;
        }
        Success();
    }

    public void Success()
    {
		nameText.SetActive (false);
		overlayText.SetActive(true);
		pauseButton.SetActive(false);
		startPanel.SetActive(true);
        overlayText.GetComponent<Text>().text = winText;
    }

	public void Credits() {
		creditButton.SetActive(false);
	    overlayText.GetComponent<Text>().text = creditText;
		EndGame();
	}

    public void Pause(bool toggle)
    {
        panel.SetActive(toggle);
        exitButton.SetActive(toggle);
        restartButton.SetActive(toggle);

        pauseButton.GetComponentInChildren<Text>().text = toggle ? "Unpause" : "Pause";
    }

    public void NextConversation()
    {

        if (!conversation.activeInHierarchy)
        {
            Debug.Log("Conversation Activate");
            SaveState();
            overlayText.SetActive(false);
            shadowPanel.SetActive(false);
            startPanel.SetActive(false);
            titleText.SetActive(false);
            startButton.SetActive(false);
            pauseButton.SetActive(false);
            creditButton.SetActive(false);
            nameText.SetActive(true);
            conversation.SetActive(true);
            nextConvButton.SetActive(true);
        }

        if (conversationIndex >= conversationList.Count)
        {
            Debug.Log("Conversation out of bounds!");
            nameText.GetComponent<Text>().text = "Error";
            messageText.GetComponent<Text>().text = "Error";
            nextConvButton.SetActive(false);
            exitConvButton.SetActive(true);
            return;
        }

        if (conversationList[conversationIndex] == "")
        {
            Debug.Log("Conversation Empty");
            conversationIndex++;
            nameText.GetComponent<Text>().text = "";
            messageText.GetComponent<Text>().text = "End of Conversation";
            nextConvButton.SetActive(false);
            exitConvButton.SetActive(true);
        }
        else
        {
            Debug.Log("Conversation Normal");
            string[] conversationStrings = conversationList[conversationIndex].Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries); ;
            nameText.GetComponent<Text>().text = conversationStrings[0];
            messageText.GetComponent<Text>().text = conversationStrings[1];

            conversationIndex++;

            if (conversationIndex == conversationList.Count || conversationList[conversationIndex] == "")
            {
                nextConvButton.SetActive(false);
                exitConvButton.SetActive(true);
                conversationIndex++;
            }

        }
    }

    public void EndConversation()
    {
        RestoreState();
    }

    private void RestoreState()
    {
        overlayText.SetActive(state[0]);
        nameText.SetActive(state[1]);
        startButton.SetActive(state[2]);
        exitButton.SetActive(state[3]);
        restartButton.SetActive(state[4]);
        continueButton.SetActive(state[5]);
        creditButton.SetActive(state[6]);
        pauseButton.SetActive(state[7]);
        panel.SetActive(state[8]);
        shadowPanel.SetActive(state[9]);
        titleText.SetActive(state[10]);
        chestText.SetActive(state[11]);
        conversation.SetActive(state[12]);
        nextConvButton.SetActive(state[13]);
        exitConvButton.SetActive(state[14]);
        startPanel.SetActive(state[15]);
    }

    private void SaveState()
    {
        state = new[]
        {
            overlayText.activeInHierarchy,
            nameText.activeInHierarchy,
            startButton.activeInHierarchy,
            exitButton.activeInHierarchy,
            restartButton.activeInHierarchy,
            continueButton.activeInHierarchy,
            creditButton.activeInHierarchy,
            pauseButton.activeInHierarchy,
            panel.activeInHierarchy,
            shadowPanel.activeInHierarchy,
            titleText.activeInHierarchy,
            chestText.activeInHierarchy,
            conversation.activeInHierarchy,
            nextConvButton.activeInHierarchy,
            exitConvButton.activeInHierarchy,
            startPanel.activeInHierarchy
        };
    }

	public void ChestOpenText()
	{
		Debug.Log("chest unlocked");
		StartCoroutine (ChestOpenTextHelper());
		AudioSource audio = chestText.GetComponent<AudioSource>();
		if (!audio.isPlaying)
		{
			audio.Play();
		}
	}

	public IEnumerator ChestOpenTextHelper()
	{
		chestText.SetActive (true);
		yield return new WaitForSeconds(1);
		chestText.SetActive (false);
	}
}
