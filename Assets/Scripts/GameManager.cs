using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{


    [SerializeField]
    private bool _isGameOver;
    private SpawnManager _spawnManager;
    private AudioManager _audioManager;

    void Start()
    {
        _spawnManager = FindObjectOfType<SpawnManager>();
        _audioManager = FindObjectOfType<AudioManager>();
    }
    public void GameWon()
    {
        StartCoroutine(GameWonRoutine());
    }
    IEnumerator GameWonRoutine()
    {
        _spawnManager.KillBossMusic();
        _audioManager.TurnOnVictoryMusic();
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
