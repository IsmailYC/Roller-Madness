using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameManager : MonoBehaviour {

	public static GameManager gm;

	[Tooltip("If not set, the player will default to the gameObject tagged as Player.")]
	public GameObject player;

	public enum gameStates {Menu, Playing, Death, GameOver, BeatLevel, Pause};
	public gameStates gameState = gameStates.Menu;

	public int score=0;
    public int highscore;
	public bool canBeatLevel = false;
	public int beatLevelScore=0;

    public GameObject menuCanvas;
	public GameObject mainCanvas;
	public Text mainScoreDisplay;
    public GameObject pauseCanvas;
	public GameObject gameOverCanvas;
	public Text gameOverScoreDisplay;
    public Text gameOverHighscoreDisplay;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public GameObject beatLevelCanvas;

	public AudioSource backgroundMusic;
	public AudioClip gameOverSFX;

	[Tooltip("Only need to set if canBeatLevel is set to true.")]
	public AudioClip beatLevelSFX;

	private Health playerHealth;

	void Start () {
		if (gm == null) 
			gm = gameObject.GetComponent<GameManager>();

		if (player == null) {
			player = GameObject.FindWithTag("Player");
		}

		playerHealth = player.GetComponent<Health>();

		// setup score display
		Collect (0);

        // make other UI inactive
        mainCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
		gameOverCanvas.SetActive (false);
		if (canBeatLevel)
			beatLevelCanvas.SetActive (false);
        highscore = PlayerPrefManager.GetHighscore();
        Time.timeScale = 0.0f;
	}

	void Update () {
		switch (gameState)
		{
			case gameStates.Playing:
                if(Input.GetButtonDown("Cancel"))
                {
                    gameState = gameStates.Pause;
                    Time.timeScale = 0.0f;
                    mainCanvas.SetActive(false);
                    pauseCanvas.SetActive(true);
                }
				if (playerHealth.isAlive == false)
				{
					// update gameState
					gameState = gameStates.Death;

					// set the end game score
					gameOverScoreDisplay.text = mainScoreDisplay.text;
                    if (score > highscore)
                    {
                        gameOverHighscoreDisplay.text = "New Record";
                        PlayerPrefManager.SetHighscore(score);
                    }
                    else
                        gameOverHighscoreDisplay.text = highscore.ToString();
					// switch which GUI is showing		
					mainCanvas.SetActive (false);
					gameOverCanvas.SetActive (true);
				} else if (canBeatLevel && score>=beatLevelScore) {
					// update gameState
					gameState = gameStates.BeatLevel;

					// hide the player so game doesn't continue playing
					player.SetActive(false);

					// switch which GUI is showing			
					mainCanvas.SetActive (false);
					beatLevelCanvas.SetActive (true);
				}
				break;
			case gameStates.Death:
				backgroundMusic.volume -= 0.01f;
				if (backgroundMusic.volume<=0.0f) {
					AudioSource.PlayClipAtPoint (gameOverSFX,gameObject.transform.position);

					gameState = gameStates.GameOver;
				}
				break;
			case gameStates.BeatLevel:
				backgroundMusic.volume -= 0.01f;
				if (backgroundMusic.volume<=0.0f) {
					AudioSource.PlayClipAtPoint (beatLevelSFX,gameObject.transform.position);
					
					gameState = gameStates.GameOver;
				}
				break;
            case gameStates.Pause:
                if (Input.GetButtonDown("Cancel"))
                {
                    gameState= gameStates.Playing;
                    pauseCanvas.SetActive(false);
                    mainCanvas.SetActive(true);
                    Time.timeScale = 1.0f;
                }
                break;
            case gameStates.GameOver:
				// nothing
				break;
		}

	}


	public void Collect(int amount) {
		score += amount;
		if (canBeatLevel) {
			mainScoreDisplay.text = score.ToString () + " of "+beatLevelScore.ToString ();
		} else {
			mainScoreDisplay.text = score.ToString ();
		}
	}

    public void StartGame()
    {
        gameState= gameStates.Playing;
        menuCanvas.SetActive(false);
        mainCanvas.SetActive(true);
        Time.timeScale = 1.0f;
    }
}
