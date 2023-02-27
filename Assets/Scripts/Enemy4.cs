using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    [SerializeField]
    private float _speed = 15;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private int _enemyMovement;
    private SpawnManager _spawnManager;

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
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -3)
        {
            transform.Translate((Vector3.right + Vector3.up) * _speed * Time.deltaTime);
        }

        if(transform.position.x >= 11 && transform.position.y <= 0)
        {
            transform.position = new Vector3(-11f, -3, 0);
        }
        if(transform.position.x == -11f)
        {
            transform.Translate((Vector3.right + Vector3.up) * _speed * Time.deltaTime);
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
