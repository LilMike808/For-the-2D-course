using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    [SerializeField]
    private GameObject[] rarepowerups;
    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    //public void is a method other scripts can communicate with.
    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnRarePowerupRoutine());

    }
    
    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            //posToSpawn is a manually created variable referenced in the Instantiate process
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            //newEnemy is a local variable of type GameObject which stores the Instantiate method.
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            //because newEnemy equals the Instantiate process and is a GameObject, we can access the parent object through its transform
            //which is assigned to the transform of _enemyContainer.
            newEnemy.transform.parent = _enemyContainer.transform;
            
            yield return new WaitForSeconds(5.0f);
        }
       
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            int randomPowerup = Random.Range(0, 5);
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
}
