using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4;
    private Player _player;
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioSource _laserSoundEffect;
    [SerializeField]
    private AudioSource _laserHitSound;
    [SerializeField]
    private AudioSource _shieldHitSound;
    [SerializeField]
    private GameObject _laserPrefab;
    private float _fireRate = 3.0f;
    private float _canFire = -1f;
    private int _enemyMovement;
    private SpawnManager _spawnManager;
    [SerializeField]
    private int _lives = 2;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _thruster;
    private bool _isShieldsActive;
    private SpriteRenderer _spriteRenderer;
    private bool _isTookAHitActive = false;
    private bool _wasHitByMissile = false;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
        _enemyMovement = Random.Range(1, 4);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if(_player == null)
        {
            Debug.Log("the Player is NULL.");
        }
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The Audio Source on the Enemy is NULL");
        }
        if (_enemyMovement == Random.Range(1, 3))
        {
            _isShieldsActive = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (_isShieldsActive == false)
        {
            _shieldVisualizer.SetActive(false);
        }
        if(_lives < 1)
        {
            _spriteRenderer.color = new Color(0, 2, 70, 2);
        }
        CalculateMovement();
        //fire laser method below
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
            _laserSoundEffect.Play();
        }       
    }
   public void IsEnemyLaser()
    {
        _laserHitSound.Pause();
    }
    public void IsNotEnemyLaser()
    {
        if(_isShieldsActive == true)
        {
            _laserHitSound.Pause();
            _shieldHitSound.Play();
            return;
        }
        if(_isShieldsActive == false && _lives > 0)
        {
            _laserHitSound.Play();
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
    private IEnumerator GotDamaged()
    {
        _isTookAHitActive = true;
        StartCoroutine(TookAHit());
        yield return new WaitForSeconds(0.8f);
        _isTookAHitActive = false;
    }
    private IEnumerator TookAHit()
    {
        while(_isTookAHitActive == true)
        {
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 0.240556f, 0.240556f);
            yield return new WaitForSeconds(0.13f);
            _spriteRenderer.color = new Color(1, 1, 1);
        }
    }
    public void Damage()
    {
        if (_wasHitByMissile == false)
        {
            if (_isShieldsActive == false)
            {
                _lives--;
                StartCoroutine(GotDamaged());
            }
            if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _shieldHitSound.Play();
                return;
            }           
        }
        else if (_wasHitByMissile == true)
        {
            if(_isShieldsActive == false)
            {
                _lives -= 2;
            }
            else if (_isShieldsActive == true)
            {
                _isShieldsActive = false;
                _lives -= 1;
                StartCoroutine(GotDamaged());
                return;
            }
        }
        if (_lives < 1)
        {
            _isTookAHitActive = false;
            _thruster.gameObject.SetActive(false);
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0;
            _audioSource.Play();
            //My formula is that if an Enemy dies at all via the Player, even via Player collision, Player gets points, unless course instructs otherwise.
            _player.AddScore(Random.Range(3, 5));
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
        if(other.tag == "Player")
        {
            _wasHitByMissile = false;

            if (_player != null)
            {
                _player.Damage();
            }
        }
        if (other.tag == "Thruster" && _speed > 0)
        {
            _wasHitByMissile = false;
            if (_lives > 0)
            {
                _laserHitSound.Play();
            }
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
