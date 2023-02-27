using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    private float _speed = 2.5f;
    [SerializeField]
    private float _range = 5f;
    private int _enemyMovement;
    private bool _stopTheRock = false;
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
        RamPlayer();
        
    }
    void RamPlayer()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _range)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * 3f * Time.deltaTime);
        }
    }
    void CalculateMovement()
    {

        if (_enemyMovement == 1)
        {
            transform.Translate((Vector3.up + Vector3.left) * _speed * Time.deltaTime);
        }
        if (_enemyMovement == 2)
        {
            transform.Translate((Vector3.up + Vector3.right) * _speed * Time.deltaTime);
        }
        
        if (transform.position.x > 11.75f)
        {
            transform.position = new Vector3(Random.Range(0, -11.75f), -5, 0);
        }
        if (transform.position.x < -11.75f)
        {
            transform.position = new Vector3(Random.Range(0, 11.75f), -5, 0);
        }
        if (transform.position.y > 7f && transform.position.x < -1f)
        {
            transform.position = new Vector3(Random.Range(0, 11.75f), -5, 0);
        }
        if (transform.position.y > 7f && transform.position.x > 1f)
        {
            transform.position = new Vector3(Random.Range(0, -11.75f), -5, 0);
        }
        
        if (transform.position.y >= 0)
        {
            StartCoroutine(Stop_HammerTime());
        }
    }
    IEnumerator Stop_HammerTime()
    {
        _speed = 0;
        yield return new WaitForSeconds(3.5f);
        if (transform.position.y != 0)
        {
            _speed = 2.5f;
            if (_stopTheRock == true)
            {
                _speed = 0;
            }
            RamPlayer();

        }

    }
        
    private void OnTriggerEnter2D(Collider2D other)
    {
       
       
        if (other.tag == "Player")
        { 
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {    
                StartCoroutine(Stop_HammerTime());
                player.Damage();
            }           
            _anim.SetTrigger("OnEnemyDeath");
            _stopTheRock = true;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        }
        if (other.tag == "Laser")
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                StartCoroutine(Stop_HammerTime());
                _player.AddScore(Random.Range(10, 15));
            }          
            _anim.SetTrigger("OnEnemyDeath");
            _stopTheRock = true;
            _audioSource.Play();
            _spawnManager.EnemyDeath();
            //Collider is destroyed to disable explosion sound after one shot.
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 2.3f);
        } 
    }
}
