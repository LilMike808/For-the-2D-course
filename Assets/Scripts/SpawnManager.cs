using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] _enemyType;
    [SerializeField]
    private GameObject[] _stars;
    [SerializeField]
    private GameObject[] _randomPowerup;
    [SerializeField]
    private GameObject _dreadBoss;
    private bool _stopSpawning = false;
    private bool _stopSpawningEnemies = false;
    [SerializeField]
    private int _waveNumber;
    [SerializeField]
    private int _enemiesDead;
    [SerializeField]
    private int _maxEnemies;
    private UI_Manager _uiManager;
    private AudioManager _audioManager;
    [SerializeField]
    private AudioSource _waveSoundEffect;
    [SerializeField]
    private AudioSource _bossMusic;
    private PlanetLeave _planetLeave;
    private Player _player;
    public int total;
    public int randomNumber;
    public int[] table =
    {
        40, //ammoPowerup
        16, //health pickup
        13, //shield
        12, //Negative
        8,  //TripleShot
        6, //SpeedBoost
        3, //Missile
        2 //Multidirectional
    };
    // Start is called before the first frame update
    void Start()
    {
        _audioManager = GameObject.FindObjectOfType<AudioManager>();
        _uiManager = GameObject.FindObjectOfType<UI_Manager>();
        _player = GameObject.FindObjectOfType<Player>();
        _planetLeave = GameObject.FindObjectOfType<PlanetLeave>();
        foreach (var item in table)
        {
            total += item;
        }
    }
    public void StartSpawning()
    {
        _planetLeave.Down();
        StartCoroutine(SpawnRandomPowerupsRoutine());
        StartCoroutine(SpawnStarsRoutine());
        SpawnWave(1);
    }

    private void SpawnWave(int wavenumber)
    {
        if (wavenumber <= 6)
        {
           
            _stopSpawningEnemies = false;
            _stopSpawning = false;
            _waveNumber = wavenumber;
            _uiManager.DisplayWaveNumber(wavenumber);
            if(_waveNumber < 6)
            {
                if (_waveNumber < 6)
                {
                    _waveSoundEffect.Play();
                }
                _enemiesDead = 0;
                _maxEnemies = 6 + _waveNumber;
                StartCoroutine(SpawnEnemyRoutine());
            }
            if (_waveNumber == 2)
            {
                _planetLeave.DownSlower();
            }
            if(_waveNumber == 4)
            {
                _planetLeave.DownLater();
            }
            if (_waveNumber == 6)
            {
                _audioManager.KillBackgroundMusic();
                _player.KeepSpeedMusicFromPlaying();
                Vector3 posToSpawn = new Vector3(0, 8.86f, 0);
                Instantiate(_dreadBoss, posToSpawn, Quaternion.identity);
                _uiManager.TurnBossHealthOn();
                _bossMusic.Play();
                _audioManager.TurnBackgroundMusicOff(true);
            }
        }
    }
    public void KillBossMusic()
    {
        _bossMusic.Stop();
    }
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2.3f);
        if (_stopSpawningEnemies == false)
        {
            for (int i = 0; i < _maxEnemies; i++)
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-8, 8), 8, 0);
                GameObject newEnemy = Instantiate(_enemyType[Random.Range(0, _waveNumber)], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
                yield return new WaitForSeconds(5.0f);
            }
        }
        while(_enemiesDead < _maxEnemies)
        {
            yield return null;
        }
        _waveNumber++;
        SpawnWave(_waveNumber);
    }
    IEnumerator SpawnStarsRoutine()
    {
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-10, 10), 8.5f, 0);
            Instantiate(_stars[Random.Range(0, 6)], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(0.1f, 3.1f));
        }
    }

    
    private int RandomPowerup()
    {
        randomNumber = Random.Range(0, total);
        for (int i = 0; i < table.Length; i++)
        {
            if (randomNumber <= table[i])
            {
                return i;
            }
            else
            {
                randomNumber -= table[i];
            }
        }
        return 0;
    }
    public void StopSpawningPowerups()
    {
        _stopSpawning = true;
    }
    IEnumerator SpawnRandomPowerupsRoutine()
    {
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(3.0f);
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(_randomPowerup[RandomPowerup()], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2, 5));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        _stopSpawningEnemies = true;
    }
    public void EnemyDeath()
    {
        _enemiesDead++;
    }
}
