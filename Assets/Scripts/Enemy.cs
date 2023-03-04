using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private int _enemyMovement;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private bool _isShieldsActive = true;
    // Start is called before the first frame update
    void Start()
    {
        _enemyMovement = Random.Range(1, 4);

        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        
        if(_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Enemy is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire && _speed > 0)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].AssignEnemyLaser();
            }
        }
        Vector3 enemyOffset = (transform.position + new Vector3(0, -0.75f, 0));
        
        RaycastHit2D hit = Physics2D.Raycast(enemyOffset, Vector3.down * 10f, LayerMask.GetMask("PowerUps"));
        if (hit.collider != null)
        {
            Debug.DrawRay(transform.position, Vector3.down * 10f, Color.red);
            if (hit.collider.CompareTag("PowerUp"))
            {
                Debug.Log("The Powerup was detected");
                GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();
                
                for (int i = 0; i < Lasers.Length; i++)
                {
                    Lasers[i].AssignEnemyLaser();
                }
            }
        }
    }
    void CalculateMovement()
    {

        switch (_enemyMovement)
        {
            case 1:
                transform.Translate((Vector3.down + Vector3.left) * (_speed / 2) * Time.deltaTime);
                break;
            case 2:
                transform.Translate((Vector3.down + Vector3.right) * (_speed / 2) * Time.deltaTime);
                break;
            case 3:
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                break;
            default:
                break;
        }
        
        if (transform.position.y <= -7f)
        {
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);

        }
        if (transform.position.x <= -11.75f)
        {
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);

        }
        if (transform.position.x >= 11.75f)
        {
            float RandomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(RandomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _speed > 0 && _isShieldsActive == true)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        else if (other.tag == "Player" && _speed > 0 && _isShieldsActive == false)
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            Destroy(this.gameObject, 2.3f);
        }
        if (other.tag == "Laser" && _isShieldsActive == true)
        {
            Destroy(other.gameObject);
            _isShieldsActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        if (other.tag == "Laser" && _isShieldsActive == false)
        { 
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(Random.Range(5, 7));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            //Collider is destroyed to disable explosion sound after one shot.
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);           
        }
        if (other.tag == "Missile")
        {
            Destroy(other.gameObject);
            _shieldVisualizer.SetActive(false);
            if(_player != null)
            {
                _player.AddScore(Random.Range(5, 7));
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            //Collider is destroyed to disable explosion sound after one shot.
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
    }
}
