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
    private Text _victoryText;
    [SerializeField]
    private Text _restartText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Slider _thrusterSlider;
    [SerializeField]
    private Slider _bossHealth;
    [SerializeField]
    private GameObject _magicRedButton;
    [SerializeField]
    private GameObject _magicGreenButton;
    [SerializeField]
    private Text _fuelPercentageText;
    [SerializeField]
    private Text _healthPercentageText;
    [SerializeField]
    private Text _waveDisplay;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    private GameManager _gameManager;
    private AudioManager _audioManager;

    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: ";
        _ammoText.text = "Ammo: 15/15";
        _gameOverText.gameObject.SetActive(false);
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        if(_gameManager == null)
        {
            Debug.LogError("Game Manager is NULL");
        }
    }
    //update is called once per frame
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
    public void MPulseUI()
    {
        StartCoroutine(MPulseUIRoutine());
    }
    IEnumerator MPulseUIRoutine()
    {
        _magicRedButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(6f);
        _magicRedButton.gameObject.SetActive(true);
    }
    public void XRodUI()
    {
        StartCoroutine(XRodUIRoutine());
    }
    IEnumerator XRodUIRoutine()
    {
        _magicGreenButton.gameObject.SetActive(false);
        yield return new WaitForSeconds(12f);
        _magicGreenButton.gameObject.SetActive(true);
    }
    public void UpdateBossHealth(int healthpercentage)
    {
        _bossHealth.value = healthpercentage;
        _healthPercentageText.text = Mathf.RoundToInt(healthpercentage) + "%";
        if(healthpercentage <= 0)
        {
            _bossHealth.gameObject.SetActive(false);
        }
    }
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _audioManager.GameOverMusic();
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }
    public void VictorySequence()
    {
        _victoryText.gameObject.SetActive(true);
        StartCoroutine(VictoryFlickerRoutine());
    }
    public void TurnBossHealthOn()
    {
        _bossHealth.gameObject.SetActive(true);
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
    IEnumerator VictoryFlickerRoutine()
    {
        while (true)
        {
            _victoryText.text = "YOU WON!!!";
            yield return new WaitForSeconds(0.5f);
            _victoryText.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }
    public void DisplayWaveNumber(int wavenumber)
    {   //the coroutine was instead called here, no bueno.
        _waveDisplay.text = "Wave: " + wavenumber;
        _waveDisplay.gameObject.SetActive(true);
         StartCoroutine(WaveDisplayRoutine());
    }
    IEnumerator WaveDisplayRoutine()
    {
        //this below was in a while loop.
        yield return new WaitForSeconds(2.5f);
        _waveDisplay.gameObject.SetActive(false);
    }
    
}
