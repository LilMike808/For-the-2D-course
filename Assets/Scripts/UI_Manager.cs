using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Text _fuelPercentageText;
    [SerializeField]
    private Text _waveDisplay;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: ";
        _ammoText.text = "Ammo: 15/15";
        _gameOverText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void UpdateScore(int PlayerScore)
    {
        _scoreText.text = "Score: " + PlayerScore.ToString();
    }
    public void UpdateLives(int currentLives)
    {
        if(currentLives <= 0)
        {
            currentLives = 0;
        }
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives <= 0)
        {
            GameOverSequence();
        }    
    }
    public void UpdateAmmo(int ammocount)
    {
        _ammoText.text = "Ammo: " + ammocount + "/15";
    }
    public void UpdateThruster(float fuelpercentage)
    {
        _thrusterSlider.value = fuelpercentage;
        _fuelPercentageText.text = Mathf.RoundToInt(fuelpercentage) + "%";
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        //while loops go on forever as long as the conditions for it are met.
        while (true)
        {
            _gameOverText.text = "GAME OVER!!!";
            yield return new WaitForSeconds(0.5f);
            _gameOverText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DisplayWaveNumber(int wavenumber)
    {
        _waveDisplay.text = "Wave: " + wavenumber;
        _waveDisplay.gameObject.SetActive(true);
        StartCoroutine(WaveDisplayRoutine());
    }
    IEnumerator WaveDisplayRoutine()
    {
        while(_waveDisplay == true)
        {
            yield return new WaitForSeconds(2.5f);
            _waveDisplay.gameObject.SetActive(false);
        }
    }

}
