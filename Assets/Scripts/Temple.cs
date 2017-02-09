using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Temple : MonoBehaviour
{

    public float InitialTime = 120.0f;
    public int CurrentLevel;

	private int state;
	private float time;
	private bool elapsing;
	private bool paused;

	private Player player;
	private Overlay overlay;	

	void Awake()
	{
		player = FindObjectOfType<Player>();
        player.SetPlayerControl(false);
        overlay = FindObjectOfType<Overlay>();
	}

    void Start()
    {
        Debug.Log("Restart");

        Init();
        Intro();
    }

	void Update ()
	{
		if (Time.realtimeSinceStartup >= time && elapsing)
		{
			StopTime();
			time = 0.0f;
			Failure();
		}

        /*
		if ( win)
	    {
	        Success();
	    }
        */
	}

	public void StartTime()
	{
		time += Time.realtimeSinceStartup;
		this.elapsing = true;
	}

	public void StopTime()
	{
		time -= Time.realtimeSinceStartup;
		this.elapsing = false;
	}

	public float SecondsLeft()
	{
		if ( !elapsing )
		{
			return time;
		}
		else if (state < 3)
		{
			return time - Time.realtimeSinceStartup;
		}
		else
		{
			return 0.0f;
		}
	}

	public void Init()
	{
		state = 0;
        overlay.SetStrings(CurrentLevel);
		time = InitialTime;
		elapsing = false;
		paused = false;
        player.SetPlayerControl(false);
	}

	public void Intro()
	{
		Debug.Log ("Intro");
		overlay.Introduction();
		state = 1;
	}

	public void MainGame()
	{
		Debug.Log("Start");
		state = 2;
		overlay.MainGame();
        player.SetPlayerControl(true);
		StartTime();
	}

	public void Failure()
	{
		Debug.Log ("Game Over");
        player.SetPlayerControl(false);
		overlay.GameOver();
		state = 3;
	}

    public void LevelFinished()
    {
        if (state < 3)
        {
            state = 3;
            player.SetPlayerControl(false);
            Debug.Log("Finished Level " + CurrentLevel);
            overlay.LevelFinished(CurrentLevel);
            if (CurrentLevel == 2)
            {
                StartDialog();
            }
        }
        
    }

    public void LoadLevel(int level)
    {
		//Application.LoadLevel("Rebirth"+level);
        SceneManager.LoadScene("Rebirth"+level);
    }

    public void LoadNextLevel()
    {
        LoadLevel(CurrentLevel+1);
    }

	public void Credits() {
		overlay.Credits();
	}

	public void ExitGame()
	{
		Debug.Log("Exit");
		Application.Quit();
	}

	public void Restart()
	{
	    if (state < 3)
	    {
	        LoadLevel(CurrentLevel);
	    }
	    else
	    {
            LoadLevel(1);
        }
	}

	public void Pause()
	{
		if (paused)
		{
			Debug.Log("Unpause");
			overlay.Pause(false);
            player.SetPlayerControl(true);
			StartTime();
			paused = false;
		}
		else
		{
			Debug.Log("Pause");
			StopTime();
            player.SetPlayerControl(false);
			overlay.Pause(true);
			paused = true;
		}
	}

    public void StartDialog()
    {
        paused = true;
        player.SetPlayerControl(false);
        overlay.NextConversation();
    }

    public void EndDialog()
    {
        paused = false;
        player.SetPlayerControl(true);//might be worth remembering the state
        overlay.EndConversation();
    }
	
	public void Penalise(float p) {
		time -= p;
	}
	
	public void Buff(float p) {
		time += p;
	}

}
