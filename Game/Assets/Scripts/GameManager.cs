using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using Cinemachine;

using AssemblyCSharp.Assets.Scripts;

public class GameManager : MonoBehaviour {

	public AudioMixer audioMixer;
    public CinemachineVirtualCamera vcam;

    [Header("Planes")]
    public GameObject miniPlane;
    public GameObject jet;
    public GameObject commercialPlane;
    public GameObject spacecraft;
    public GameObject paperPlane;

    [Header("UI")]
	private Text speedTxt;
	private static bool isPaused = false;
	private int correctAnswers = 0;
	private GameObject pauseUI;
    private GameObject quizUI;
    private GameObject hudUI;
	private GameObject loseUI;
	private GameObject winUI;
	private Text statsTxt;
	private Text wStatsTxt;
	private Text coinsTxt;
	private Image soundImage;
	public Sprite musicImage;
	public Sprite muteImage;

	[Header("Quiz")]
    private ParticleSystem wrongAnim;
    private Text questionTxt;
    private Text answerTxt1;
    private Text answerTxt2;
    private Text answerTxt3;
    private Text answerTxt4;
    private Button b1;
	private Button b2;
	private Button b3;
	private Button b4;
    private Color correctColor;
    private Color wrongColor;
    private Color correctAnswerWasColor;
    private Color normalColor;
    private GameObject correctObj;
    private GameObject wrongObj;
    private Text correctTxt;
	private Text wrongTxt;
	private int corrNum;
	private Transform pos;
	private int coinsPerQuestion = 1;
    private string coinString = "coins";    
	private bool inQuiz = false;
    private QA currentQuestion;
	private int fuelAdded = 25;

	public static bool isEndless = false;

    private void Awake() {
		Time.timeScale = 1f;

		Scene scene = SceneManager.GetActiveScene(); 
		if(PlayerPrefs.GetString("theme", "light") == "dark" && scene.name == "Menu"){
			SceneManager.LoadScene("Dark_Menu");
			PlayerPrefs.SetString("theme", "dark");
		}

        // Checks for any previously selected plane, then stores that plane into selectedIndex
        int numSelected = 0;
        for (int i = 0; i < 5; i++) {
            if (PlayerPrefs.GetString(ShopSystem.planes[i], "Select") == "Selected") {
                numSelected++;
                ShopSystem.selectedIndex = i;
                break;
            }
        }

        // In case nothing is selected, set the mini-plane as selected
        if (numSelected == 0) {
            PlayerPrefs.SetString(ShopSystem.planes[0], "Selected");
            ShopSystem.selectedIndex = 0;
        }

        PlayerPrefs.Save();

        audioMixer.SetFloat("volume", PlayerPrefs.GetFloat("volume", 0));
		if(!scene.name.Contains("Menu") && scene.name != "Levels" && scene.name != "Dark_LvlSelect" && !scene.name.Contains("Instructions") && !scene.name.Contains("Options") && !scene.name.Contains("Credits") && !scene.name.Contains("GameMode")){

            // show number of jewels the player has
			coinsTxt = GameObject.Find("/canvas/hud/coinsPanel/coinsTxt").GetComponent<Text>();
			coinsTxt.text = PlayerPrefs.GetInt(coinString, 0).ToString();

			if(!scene.name.Contains("Shop")){

                // The player is in a map

				speedTxt = GameObject.Find("/canvas/hud/speedPanel/speedTxt").GetComponent<Text>();
				pauseUI = GameObject.Find("/canvas/pause");
				loseUI = GameObject.Find("/canvas/lose");
				quizUI = GameObject.Find("/canvas/quiz");
				hudUI = GameObject.Find("/canvas/hud");
				winUI = GameObject.Find("/canvas/win");

				questionTxt = GameObject.Find("/canvas/quiz/questionText").GetComponent<Text>();
				answerTxt1 = GameObject.Find("/canvas/quiz/choice1/text").GetComponent<Text>();
				answerTxt2 = GameObject.Find("/canvas/quiz/choice2/text").GetComponent<Text>();
				answerTxt3 = GameObject.Find("/canvas/quiz/choice3/text").GetComponent<Text>();
				answerTxt4 = GameObject.Find("/canvas/quiz/choice4/text").GetComponent<Text>();

				b1 = GameObject.Find("/canvas/quiz/choice1").GetComponent<Button>();
				b2 = GameObject.Find("/canvas/quiz/choice2").GetComponent<Button>();
				b3 = GameObject.Find("/canvas/quiz/choice3").GetComponent<Button>();
				b4 = GameObject.Find("/canvas/quiz/choice4").GetComponent<Button>();

				correctColor = convertColor(46.0f, 204, 113, 255);
				wrongColor = convertColor(255, 75, 71, 255);
				correctAnswerWasColor = convertColor(160, 233, 182, 255);
				normalColor = convertColor(200, 200, 200, 128);

				correctObj = GameObject.Find("/canvas/correct");
				wrongObj = GameObject.Find("/canvas/wrong");

				correctTxt = GameObject.Find("/canvas/correct/correctText").GetComponent<Text>();
				wrongTxt = GameObject.Find("/canvas/wrong/wrongText").GetComponent<Text>();
				wStatsTxt = GameObject.Find("/canvas/win/wStatsText").GetComponent<Text>();
				statsTxt = GameObject.Find("/canvas/lose/statsTxt").GetComponent<Text>();

				quizUI.SetActive(false);
				correctObj.SetActive(false);
				wrongObj.SetActive(false);
				pauseUI.SetActive(false);
				loseUI.SetActive(false);
				winUI.SetActive(false);

                miniPlane.SetActive(false);
                jet.SetActive(false);
                commercialPlane.SetActive(false);
                spacecraft.SetActive(false);
                paperPlane.SetActive(false);

                GameObject plane = null;

                // find the selected plane
                switch (ShopSystem.selectedIndex) {
                    case 0:
                        plane = miniPlane;
                        break;
                    case 1:
                        plane = jet;
                        break;
                    case 2:
                        plane = commercialPlane;
                        break;
                    case 3:
                        plane = spacecraft;
                        break;
                    case 4:
                        plane = paperPlane;
                        break;
                }

                // show the selected plane
                plane.SetActive(true);
                vcam.m_Follow = plane.GetComponent<Transform>();
                vcam.m_LookAt = plane.GetComponent<Transform>();
            }
		}

		if(scene.name != "Endless Map"){
			isEndless = false;
		} else {
			isEndless = true;
		}

		if(scene.name == "Menu" || scene.name == "Dark_Menu"){
			soundImage = GameObject.Find("/canvas/menu/music").GetComponent<Image>();
			if(PlayerPrefs.GetString("sound", "on") == "off"){
				soundImage.sprite = muteImage;
			} else {
				soundImage.sprite = musicImage;
			}
		}
    }

    // Converts an RGB color (0-255) to a 0-1 scale
    Color convertColor(float r, float g, float b, float a) {
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
    }

    private void Update(){
		Scene scene = SceneManager.GetActiveScene(); 
		if(!scene.name.Contains("Menu") && scene.name != "Levels" && scene.name != "Dark_LvlSelect" && !scene.name.Contains("Instructions") && !scene.name.Contains("Options") && !scene.name.Contains("Credits") && !scene.name.Contains("GameMode")) {
			
            // update the amount of jewels a player has
            coinsTxt.text = PlayerPrefs.GetInt(coinString, 0).ToString();
			if(!scene.name.Contains("Shop")){

                // show the speed of the plane
				float roundedSpeed = Mathf.Round(PlaneController.speed);
				speedTxt.text = roundedSpeed.ToString();

				if(PlaneController.questionCol && !PlaneController.lost){

                    // The player has hit a question box, open a quiz
					if (!inQuiz) {
						inQuiz = true;
						correctObj.SetActive(false);
						wrongObj.SetActive(false);
						Quiz();
						hudUI.SetActive(false);
					}
				}

                // Pause/play
				if (Input.GetKeyDown(KeyCode.Escape)){
					if (isPaused){
						Resume();
					} else {
						Pause(); 
					}
				}

                // Game over when the player has collided with an object (not a question box) and there are still questions left to answer
				if(PlaneController.lost && GameObject.FindGameObjectsWithTag("question cube").Length != 0) {
					gameOver();
				}

                // Win when there are no questions left to answer
				if(GameObject.FindGameObjectsWithTag("question cube").Length == 0 && !inQuiz) {
					win();
				}
			}
		}

		if(PlayerPrefs.GetString("sound", "on") == "off"){
			AudioListener.pause = true;
		} else {
			AudioListener.pause = false;
		}
	}

	private void gameOver(){

        // Show the lose screen
		loseUI.SetActive(true);
		if(correctAnswers > 1 || correctAnswers == 0) statsTxt.text = correctAnswers + "\nAnswers Correct";
		if(correctAnswers == 1) statsTxt.text = correctAnswers + "\nAnswer Correct";
		PlaneController.lost = false;
	}

	private void win(){

        // Show the win screen
		PlaneController.won = true;
		winUI.SetActive(true);
		if(correctAnswers > 1 || correctAnswers == 0) wStatsTxt.text = correctAnswers + "\nAnswers Correct";
		if(correctAnswers == 1) wStatsTxt.text = correctAnswers + "\nAnswer Correct";
		Time.timeScale = 0f;
	}

	public void Quit(){
		Application.Quit();
	}

	public void Restart(){
		Scene scene = SceneManager.GetActiveScene(); 
		SceneManager.LoadScene(scene.name);
		PlaneController.speed = 0;
		Time.timeScale = 1f;
	}

	public void LevelSelect(){
		Time.timeScale = 1f;
		Scene scene = SceneManager.GetActiveScene();
        if (PlayerPrefs.GetString("theme", "light") == "light") {
            SceneManager.LoadScene("Levels");
        } else {
            SceneManager.LoadScene("Dark_LvlSelect");
        }
    }

	public void ChangeTheme(){
		if(PlayerPrefs.GetString("theme", "light") == "light"){
			SceneManager.LoadScene("Dark_Menu");
			PlayerPrefs.SetString("theme", "dark");
		} else if(PlayerPrefs.GetString("theme", "dark") == "dark"){
			SceneManager.LoadScene("Menu");
			PlayerPrefs.SetString("theme", "light");
		}
	}

	public void Menu(){

        // Go back to the menu, reset any flight variables
		inQuiz = false;
		PlaneController.lost = false;
		PlaneController.won = false;
		Time.timeScale = 1f;
		Debug.Log(PlayerPrefs.GetString("theme", "light"));
		if(PlayerPrefs.GetString("theme", "light") == "light"){
			SceneManager.LoadScene("Menu");
		} else {
			SceneManager.LoadScene("Dark_Menu");
		}
		Debug.Log(PlayerPrefs.GetString("theme", "light"));
	}

	public void Mute(){
		if(PlayerPrefs.GetString("sound", "on") == "off"){
			PlayerPrefs.SetString("sound", "on");
			soundImage.sprite = musicImage;
		} else {
			PlayerPrefs.SetString("sound", "off");
			soundImage.sprite = muteImage;
		}
	}

	public void Pause(){
		if(!inQuiz){
			pauseUI.SetActive(true);
			Time.timeScale = 0f;
			isPaused = true;
		}
	}

	public void Resume(){
		pauseUI.SetActive(false);
		Time.timeScale = 1f;
		isPaused = false;
	}

    // Load the scene passed in as a parameter
	public void customScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }

    // Retrieve a random question and display it to the user
	private void Quiz() {
        currentQuestion = FileReader.getRandomQuestion();
        quizUI.SetActive(true);
		Time.timeScale = 0f;
		showText();
	}

    // Shows the question and answer choices
	private void showText() { 
    	string q = currentQuestion.question;
        string a1 = currentQuestion.answers[0];
        string a2 = currentQuestion.answers[1];
        string a3 = currentQuestion.answers[2];
        string a4 = currentQuestion.answers[3];

        questionTxt.text = q;
        answerTxt1.text = a1;
        answerTxt2.text = a2;
        answerTxt3.text = a3;
        answerTxt4.text = a4;
    }

    // When the user clicks answer choice 1
	public void Choice1(Button button){
		choiceFunc(1, button);
	}

    // When the user clicks answer choice 2
    public void Choice2(Button button){
		choiceFunc(2, button);
	}

    // When the user clicks answer choice 3
    public void Choice3(Button button){
		choiceFunc(3, button);
	}

    // When the user clicks answer choice 4
    public void Choice4(Button button){
		choiceFunc(4, button);
	}

	private void choiceFunc(int num, Button b){
		bool correct;
		ColorBlock cb = b.colors;
		if(num == currentQuestion.correctAnswer){

            // correct

			cb.disabledColor = correctColor;
			correct = true;
			correctAnswers++;
			Coins.addCoin(coinString, coinsPerQuestion);
		} else {

            // incorrect

			cb.disabledColor = wrongColor;

            switch(currentQuestion.correctAnswer) {
                case 1:
                    setButtonColor(b1, correctAnswerWasColor);
                    break;
                case 2:
                    setButtonColor(b2, correctAnswerWasColor);
                    break;
                case 3:
                    setButtonColor(b3, correctAnswerWasColor);
                    break;
                case 4:
                    setButtonColor(b4, correctAnswerWasColor);
                    break;
            }
            correct = false;
		}

		b.colors = cb;
		finishQuestion(correct, b);
	}

    // Make all buttons disabled
	private void finishQuestion(bool c, Button b){
		b1.interactable = !b1.interactable;
		b2.interactable = !b2.interactable;
		b3.interactable = !b3.interactable;
		b4.interactable = !b4.interactable;

		StartCoroutine(waitQuiz(c, b));
	}

    // Sets the color of an inputted button
    private void setButtonColor(Button b, Color c) {
        ColorBlock cb = b.colors;
        cb.disabledColor = c;
        b.colors = cb;
    }

    // After an answer has been selected
    IEnumerator waitQuiz(bool c, Button b){

        // wait for 4 seconds
        yield return new WaitForSecondsRealtime(4);
		Time.timeScale = 1f;

        // reset button colors
        setButtonColor(b1, normalColor);
        setButtonColor(b2, normalColor);
        setButtonColor(b3, normalColor);
        setButtonColor(b4, normalColor);

        // stop buttons from being interactable
        b1.interactable = !b1.interactable;
		b2.interactable = !b2.interactable;
		b3.interactable = !b3.interactable;
		b4.interactable = !b4.interactable;

        // hide quiz, show HUD
        hudUI.SetActive(true);
		quizUI.SetActive(false);

		if(c){

            // Add fuel
			correctObj.SetActive(true);
			correctTxt.text = "Fuel +" + fuelAdded;
			PlaneController.fuel += fuelAdded;
		} else {

            // Do not add fuel
			wrongObj.SetActive(true);
			wrongTxt.text = "Fuel +0";
		}

		StartCoroutine(removeMessages());
		if(isEndless) StartCoroutine(PlaneController.respawnQuestion());
        inQuiz = false;
        PlaneController.questionCol = false;
    }

	IEnumerator removeMessages(){
		yield return new WaitForSeconds(3);
		correctObj.SetActive(false);
		wrongObj.SetActive(false);
	}
}