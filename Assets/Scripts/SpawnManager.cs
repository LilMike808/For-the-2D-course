using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemy2Prefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] rarepowerups;
    [SerializeField]
    private GameObject[] _enemyType;
    private bool _stopSpawning = false;
    private int _waveNumber;
    private int _enemiesDead;
    private int _maxEnemies;
    private int _enemiesLeft;
    private UI_Manager _uiManager;
    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.FindObjectOfType<UI_Manager>();
    }

    //public void is a method other scripts can communicate with.
    public void StartSpawning(int wavenumber)
    {
        if (wavenumber <= 5)
        {
            _stopSpawning = false;
            _enemiesDead = 0;
            _waveNumber = wavenumber;
            _uiManager.DisplayWaveNumber(_waveNumber);
            _enemiesLeft = _waveNumber + 10;
            _maxEnemies = _waveNumber + 10;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnRarePowerupRoutine());
        }

    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        
        while(_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyType[Random.Range(0,2)], posToSpawn, Quaternion.identity);
            //because newEnemy equals the Instantiate process and is a GameObject, we can access the parent object through its transform
            //which is assigned to the transform of _enemyContainer.
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesLeft--;
            if(_enemiesLeft == 0)
            {
                _stopSpawning = true;
            }           
            yield return new WaitForSeconds(5.0f);
        }
        StartSpawning(_waveNumber + 1);     
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 6);
            //posToSpawn is only local to this while loop                       
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f),7, 0);
            Instantiate(powerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3f, 8f));

        }
    }
    IEnumerator SpawnRarePowerupRoutine()
    {
        yield return new WaitForSeconds(20.0f);
        while (_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 1);
            //posToSpawn is only local to this while loop                       
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(rarepowerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15f, 30f));

        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    public void EnemyDeath()
    {
        _enemiesDead++;
        if(_enemiesLeft == 0 && _enemiesDead == _maxEnemies)
        {
            _waveNumber++;
            if(_waveNumber == 3)
            {
                Debug.Log("Wave 3!!");
            }
            _uiManager.DisplayWaveNumber(_waveNumber);
        }
    }
}
