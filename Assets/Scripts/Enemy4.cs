using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15;
    private float _fireRate = 1.1f;
    private float _canFire = -1f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private int _enemyMovement;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _grenadePrefab;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _enemyMovement = Random.Range(1, 3);
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Time.time > _canFire && _speed > 0 && transform.position.y <= -3f)
        {   
            _canFire = Time.time + _fireRate;
            Instantiate(_grenadePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y <= -4.2f)
        {
            transform.Translate((Vector3.right + Vector3.up) * _speed * Time.deltaTime);
        }
        if (transform.position.x >= 10f)
        {
            _speed = 6;
            transform.position = new Vector3(-10, -5, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _speed > 0)
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
        if (other.tag == "Laser")
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
    }
}

        

