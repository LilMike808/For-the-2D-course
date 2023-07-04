using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4 : MonoBehaviour
{
    private float _speed = 5;
    private float _fireRate = 1.9f;
    private float _canFire = -1f;
    private Player _player;
    private Animator _anim;
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _grenadeLaunchSound;
    [SerializeField]
    private AudioSource _laserHitSound;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _grenadePrefab;
    [SerializeField]
    private GameObject _thruster;
    private int _lives = 3;
    private SpriteRenderer _spriteRenderer;
    private bool _isTookAHitActive = false;
    private bool _movedRight;
    private bool _wasHitByMissile = false;
    private Vector3 _rightPoint = new Vector3(7, -4.7f, 0);
    private Vector3 _leftPoint = new Vector3(-7, -4.7f, 0);
    private Vector3 _startPoint;
    public enum MovementState
    {
        zipdown,
        moveside2side
    }
    public MovementState currentState;
    // Start is called before the first frame update
    void Start()
    {
        _startPoint = new Vector3(transform.position.x, -4.7f, 0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        currentState = MovementState.zipdown;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case MovementState.zipdown:
                if (transform.position != _startPoint)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _startPoint, _speed * Time.deltaTime);

                }
                else
                {
                    _thruster.gameObject.SetActive(false);
                    currentState = MovementState.moveside2side;
                }
                break;
            case MovementState.moveside2side:
                if(_movedRight == false)
                {
                    MoveBottomRight();
                }
                else
                {
                    MoveBottomLeft();
                }
                break;
        }
        if (Time.time > _canFire && _speed > 0 && transform.position.y < -4.6f)
        {   
            _canFire = Time.time + _fireRate;
            Instantiate(_grenadePrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            _grenadeLaunchSound.Play();
        }
    }
  
    private IEnumerator GotDamaged()
    {
        _isTookAHitActive = true;
        StartCoroutine(TookAHit());
        yield return new WaitForSeconds(0.8f);
        _isTookAHitActive = false;
    }
    private IEnumerator TookAHit()
    {
        while (_isTookAHitActive == true)
        {
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    void MoveBottomRight()
    {
        if (transform.position != _rightPoint && _lives >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _rightPoint, (_speed = 10) * Time.deltaTime);
        }
        else
        {
            _movedRight = true;
        }
    }
    void MoveBottomLeft()
    {
        if(transform.position != _leftPoint && _lives >= 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, _leftPoint, (_speed = 10) * Time.deltaTime);
        }
        else
        {
            _movedRight = false;
        }
    }
    public void IsEnemyLaser()
    {
        _laserHitSound.Pause();
    }
    public void IsNotEnemyLaser()
    {
        if (_lives > 0)
        {
            _laserHitSound.Play();
        }
    }
    public void Damage()
    {
        if (_wasHitByMissile == false)
        {
            _lives--;
            StartCoroutine(GotDamaged());
        }
        else if (_wasHitByMissile == true)
        {
            _lives -= 2;
            StartCoroutine(GotDamaged());
        }
        if (_lives < 1)
        {
            _speed = 0;
            _isTookAHitActive = false;
            _thruster.gameObject.SetActive(false);
            _anim.SetTrigger("OnEnemyDeath");
            _player.AddScore(Random.Range(8, 10));
            _audioSource.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemyDeath();
            Destroy(this.gameObject, 2.3f);
        }
    }
    public void MissileHit()
    {
        _wasHitByMissile = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _speed > 0)
        {
            _wasHitByMissile = false;
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }          
        }
        if(other.tag == "Thruster" && _speed > 0)
        {
            _wasHitByMissile = false;
            if(_lives > 0)
            _laserHitSound.Play();      
            Damage();
        }
        if(other.tag == "Laser")
        {
            _wasHitByMissile = false;
        }
        if(other.tag == "Rod")
        {
            _wasHitByMissile = false;
            IsNotEnemyLaser();
            Damage();
        }
    }
}

        

