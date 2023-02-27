using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private float _speed = 7f;
    private int _enemyMovement;
    [SerializeField]
    private GameObject _enemy2Lasers;
    private float _fireRate = 3f;
    private float _canFire = -1f;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>(); 
        
        _enemyMovement = Random.Range(1, 3);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Time.time > _canFire && _speed > 0)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemy2Lasers, transform.position, Quaternion.identity);
            Laser[] Lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < Lasers.Length; i++)
            {
                Lasers[i].AssignEnemyLaser();
            }
        }
    }
    void CalculateMovement()
    {

        if (_enemyMovement == 1)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }
        if (_enemyMovement == 2)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        if (transform.position.x > 11.75f)
        {
            float randomx = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(-11.75f, randomx, 0);
        }
        if (transform.position.x < -11.75f)
        {
            float randomy = Random.Range(-1f, 5.60f);
            transform.position = new Vector3(11.75f, randomy, 0);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && _speed > 0)
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.Damage();
            }
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            Destroy(this.gameObject, 2.3f);
        }
        if(other.tag == "Laser")
        {
            Destroy(other.gameObject);

            if(_player != null)
            {
                _player.AddScore(Random.Range(7,9));
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
