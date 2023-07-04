using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _backgroundMusic;
    private Player _player;
    private SpawnManager _spawnManager;
    private bool _isBackgroundMusicOff;
    private bool _backgroundMusicStaysOff = false;
    [SerializeField]
    private AudioSource _gameOverMusic;
    [SerializeField]
    private AudioSource _victoryMusic;
    // Start is called before the first frame update
    void Start()
    {
        _backgroundMusic = GetComponentInChildren<AudioSource>();
        _spawnManager = FindObjectOfType<SpawnManager>();
        _player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void KillBackgroundMusic()
    {
        _backgroundMusicStaysOff = true;
        _backgroundMusic.gameObject.SetActive(false);
    }
    public void SimplyTurnOffBackgroundMusic()
    {
        _backgroundMusic.Pause();
    }
    public void TurnOnVictoryMusic()
    {
        _victoryMusic.Play();
    }
    public void RestartBackgroundMusic()
    {
        if (_backgroundMusicStaysOff == false)
        {
            _backgroundMusic.Play();
        }
    }
    public void GameOverMusic()
    {
        _gameOverMusic.Play();
        _backgroundMusic.Stop();
        _spawnManager.KillBossMusic();
    }
    public void TurnBackgroundMusicOff(bool turnitoff = false)
    {
        _isBackgroundMusicOff = turnitoff;
        if (_isBackgroundMusicOff == true)
        {
            _backgroundMusic.Stop();
        }
        else
        {
            turnitoff = false;
            _backgroundMusic.Play();
        }
    }
}
