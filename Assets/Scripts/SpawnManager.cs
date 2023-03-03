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
    private GameObject[] frequentpowerups;
    [SerializeField]
    private GameObject[] commonenemies;
    [SerializeField]
    private GameObject[] rareenemies;
    [SerializeField]
    private GameObject[] UFO;
    [SerializeField]
    private GameObject[] enemy4;
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
            StartCoroutine(SpawnPowerupRoutine());
            StartCoroutine(SpawnRarePowerupRoutine());
            StartCoroutine(SpawnFrequentPowerupRoutine());            
            
            if(wavenumber >= 1)
            {
                StartCoroutine(SpawnEnemyRoutine());
            }
            if (wavenumber >= 2)
            {              
                StartCoroutine(SpawnRareEnemyRoutine());
            }
            if(wavenumber >= 3)
            {
                StartCoroutine(SpawnUFOEnemyRoutine());
            }
            if(wavenumber >= 4)
            {
                StartCoroutine(SpawnEnemy4Routine());
            }
            if(wavenumber > 5)
            {
                _stopSpawning = true;
            }
            
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(commonenemies[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
            //because newEnemy equals the Instantiate process and is a GameObject, we can access the parent object through its transform
            //which is assigned to the transform of _enemyContainer.
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesLeft--;
            if (_enemiesLeft <= 0)
            {
                _enemiesLeft = 0;
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(5.0f);
        }
        StartSpawning(_waveNumber + 1);
        StartCoroutine(SpawnRareEnemyRoutine());
        
    }
    IEnumerator SpawnRareEnemyRoutine()
    {
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(-11.75F, Random.Range(2, 6.5f), 0);
            GameObject newEnemy = Instantiate(rareenemies[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesLeft--;
            if (_enemiesLeft <= 0)
            {
                _enemiesLeft = 0;
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnUFOEnemyRoutine()
    {
        yield return new WaitForSeconds(4.0f);
        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(-11.75f, -5, 0);
            GameObject newEnemy = Instantiate(UFO[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesLeft--;
            if (_enemiesLeft <= 0)
            {
                _enemiesLeft = 0;
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnEnemy4Routine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false && _enemiesDead <= _maxEnemies)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(0f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(enemy4[Random.Range(0, 1)], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            _enemiesLeft--;
            if (_enemiesLeft <= 0)
            {
                _enemiesLeft = 0;
                _stopSpawning = true;
            }
            yield return new WaitForSeconds(5.0f);
        }       
    }
    

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 3);
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
            int randomPowerup = Random.Range(0, 3);
            //posToSpawn is only local to this while loop                       
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(rarepowerups[randomPowerup], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(15f, 30f));
        }
    }
    IEnumerator SpawnFrequentPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 1);
            Vector3 PosToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(frequentpowerups[randomPowerup], PosToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(2f, 6f));
        }
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    public void EnemyDeath()
    {
        _enemiesDead++;
        if(_enemiesDead == _maxEnemies)
        {
            new WaitForSeconds(3.0f);
            _waveNumber++;
            _uiManager.DisplayWaveNumber(_waveNumber);
        }
    }
}
