using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private GameObject _thruster;
    [SerializeField]
    private AudioSource _lasersSoundEffect;
    [SerializeField]
    private AudioSource _explosionSoundEffect;
    [SerializeField]
    private AudioSource _whooshSound;
    private SpawnManager _spawnManager;
    private float _speed = 4.2f;
    [SerializeField]
    private GameObject _enemy2Lasers;
    private float _fireRate = 9f;
    private float _canFire = -1f;
    private SpriteRenderer _spriteRenderer;
    private bool _isEnemyLaser = false;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindObjectOfType<Player>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _anim = GetComponentInChildren<Animator>();     
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
            _lasersSoundEffect.Play();
        }
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 1.5f, Vector3.down, LayerMask.GetMask("Laser"));

        if (hit.collider != null && _speed > 0)
        {
            //Noticed this applies to its own laser too but that's fine.
            if (hit.collider.CompareTag("Laser"))
            {
                transform.Translate(Vector3.right + Vector3.up * (_speed * 3) * Time.deltaTime);
                _whooshSound.Play();
            }            
        }
    }
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= 0 && transform.position.x >= 0)
        {
            transform.Translate(Vector3.left * _speed * Time.deltaTime);
        }

        if (transform.position.y <= 0 && transform.position.x <= 0)
        {
            transform.Translate(Vector3.right * _speed * Time.deltaTime);
        }
        
        if (transform.position.x > 11.75f)
        {
     
            transform.position = new Vector3(Random.Range(-8, 8), 8, 0);
        }
        if (transform.position.x < -11.75f && transform.position.y < -5)
        {
            
            transform.position = new Vector3(Random.Range(-8, 8), 8, 0);
        }
        if(transform.position.y < -6)
        {
            transform.position = new Vector3(Random.Range(-8, 8), 8, 0);
        }
    }
    public void IsEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    void Destroyed()
    {
        _thruster.gameObject.SetActive(false);
        _anim.SetTrigger("OnEnemyDeath");
        _speed = 0;
        _explosionSoundEffect.Play();
        if (_player != null)
        {
            _player.AddScore(Random.Range(5, 7));
        }
        Destroy(GetComponent<Collider2D>());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        _spawnManager.EnemyDeath();
        Destroy(this.gameObject, 2.3f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Destroyed();
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
        if (other.tag == "Thruster")
        {
            Destroyed();
        }
        if (other.tag == "Laser" && _isEnemyLaser == false)
        {
            Destroyed();
        }
        if(other.tag == "Missile")
        {
            Destroyed();
        }
        if(other.tag == "Rod")
        {
            Destroyed();
        }
    }
}
