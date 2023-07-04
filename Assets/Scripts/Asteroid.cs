using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotateSpeed = 3.0f;
    [SerializeField]
    private GameObject _explosionPrefab;
    private SpawnManager _spawnManager;
    private UI_Manager _uiManager;
    private bool _isBossAsteroids = false;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_isBossAsteroids == true)
        {
            BossAsteroids();
        }
        else
        {
            RegularAsteroid();
        }
        if(transform.position.y < -8 && _isBossAsteroids == true || transform.position.y < -8 && _isBossAsteroids == false)
        {
            Destroy(this.gameObject);
        }
    }
    void RegularAsteroid()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }
    void BossAsteroids()
    {
        transform.Translate(Vector3.down * 10 * Time.deltaTime);
    }
    public void AssignBossAsteroid()
    {
        _isBossAsteroids = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && _isBossAsteroids == false)
        {           
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);     
            Destroy(other.gameObject);         
            Destroy(this.gameObject);
            int wavenumber = 1;
            _uiManager.DisplayWaveNumber(wavenumber);
            _uiManager.DestroyClueText();
            _spawnManager.StartSpawning();
            
        }
        else if (other.tag == "Laser" && _isBossAsteroids == true)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        else if (other.tag == "Player" && _isBossAsteroids == true)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
