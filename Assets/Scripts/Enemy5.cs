using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy5 : MonoBehaviour
{
    //alienship prefab is this script, it is enemy5. whoops.
    private Player _player;
    private Animator _anim;
    private int _oRing = 0;
    private float _speed = 2;
    [SerializeField]
    private AudioSource _audioSource;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _lightning;
    [SerializeField]
    private GameObject _rotatingBox;
    [SerializeField]
    private GameObject _teleportAnimation;
    [SerializeField]
    private GameObject _thruster1;
    [SerializeField]
    private GameObject _thruster2;
    [SerializeField]
    private GameObject _expansionVisualizer;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private int _lives = 7;
    private SpriteRenderer _spriteRenderer;
    private Vector3 _rightPoint = new Vector3(7, 2f, 0);
    private Vector3 _leftPoint = new Vector3(-7, 2f, 0);
    private Vector3 _startPoint;
    private bool _isTookAHitActive = false;
    private bool _movedRight;  
    private bool _wasHitByMissile = false;
    private bool _isRotatingBoxActive = false;
    private bool _isPauseAndTeleportActive = false;
    private bool _wasLightningHitByLaser = false;
    private bool _isMoveSideToSideEnumActive = false;
    private bool _isMoveTowardPlayerEnumActive = false;
    private bool _isPauseAndTeleportEnumActive = false;
    private bool _isReappearEnumActive = false;
    private bool _isExpansionActive = false;
    [SerializeField]
    private AudioSource _laserHitSound;
    [SerializeField]
    private AudioSource _shieldHitSound;
    [SerializeField]
    private AudioSource _electricHumSound;
    public enum MovementState
    {
        stopallcoroutinesfirst,
        movedown,
        moveside2side,
        movetowardplayer,
        pauseandteleport,
        stopallcoroutines,
        reappear
    }
    public MovementState currentState;
    // Start is called before the first frame update
    void Start()
    {
        _startPoint = new Vector3(transform.position.x, 2f, 0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _player = GameObject.FindObjectOfType<Player>();
        _audioSource = GetComponent<AudioSource>();
        currentState = MovementState.movedown;
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        EctoPlasmicLightning();
        TheRotatingBox();
        MoveTowardPlayer();
        if (_isExpansionActive == true && _oRing == 0)
        {
            Instantiate(_expansionVisualizer, transform.position, Quaternion.identity);
            _oRing += 1;
            _isExpansionActive = false;
        }
        switch (currentState)
        {
            case MovementState.stopallcoroutinesfirst:
                StopAllCoroutines();
                currentState = MovementState.movedown;
                break;
            case MovementState.movedown:
                _oRing = 0;
                _rotatingBox.gameObject.SetActive(false);
                _movedRight = false;
                _speed = 2;
                if (transform.position != _startPoint && transform.position.y > 2 && _lives > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, _startPoint, _speed * Time.deltaTime);
                }
                else
                {
                    currentState = MovementState.moveside2side;
                }
                break;
            case MovementState.moveside2side:
                _isMoveSideToSideEnumActive = true;
                if (_movedRight == false)
                {
                    MoveMidRight();
                }
                else
                {
                    MoveMidLeft();
                }
                StartCoroutine(Pause());
                if (_speed == 0)
                {
                    _isRotatingBoxActive = true;
                }
                break;
            case MovementState.movetowardplayer:
                StopCoroutine(Pause());
                _isMoveSideToSideEnumActive = false;
                _isMoveTowardPlayerEnumActive = true;
                StartCoroutine(ImperviousBoxCapturePlayer());
                break;
            case MovementState.pauseandteleport:
                _isMoveTowardPlayerEnumActive = false;
                _isPauseAndTeleportEnumActive = true;
                if (transform.position.y < 2)
                {
                    _isPauseAndTeleportActive = true;
                }
                StartCoroutine(MoveAndReappear());
                break;
            case MovementState.stopallcoroutines:
                StopAllCoroutines();
                currentState = MovementState.reappear;
                break;
            case MovementState.reappear:
                _isPauseAndTeleportEnumActive = false;
                _isReappearEnumActive = true;
                _isPauseAndTeleportActive = false;
                StartCoroutine(Reappear());
                break;
        }
    }
    void EctoPlasmicLightning()
    {
        if(transform.position.y == 2f && _isRotatingBoxActive == false && _wasLightningHitByLaser == false && _lives > 0)
        {
            _lightning.gameObject.SetActive(true);
            if (_lives > 0)
            {
                _electricHumSound.Play();
            }
        }
        else if (_isRotatingBoxActive == true && _wasLightningHitByLaser == false)
        {
            _lightning.gameObject.SetActive(false);
            _electricHumSound.Pause();
        }
        if(_lives < 1)
        {
            _electricHumSound.Stop();
        }
    }
    void TheRotatingBox()
    {
        if(_speed > 0 && _isRotatingBoxActive == true && _isPauseAndTeleportActive == false && transform.position.y < 2.1f && _lives > 0)
        {
            _rotatingBox.gameObject.SetActive(true);
        }
        else if (_isRotatingBoxActive == false && _isPauseAndTeleportActive == true)
        {
            _rotatingBox.gameObject.SetActive(false);
        }
    }
    void MoveMidRight()
    {
        if (transform.position != _rightPoint && _lives >= 1 && transform.position.y < 2.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _rightPoint, _speed * Time.deltaTime);
        }
        else
        {
            _movedRight = true;
        }
    }
    void MoveMidLeft()
    {
        if (transform.position != _leftPoint && _lives >= 1 && transform.position.y < 2.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, _leftPoint, _speed * Time.deltaTime);
        }
        else if (transform.position.y < 2.1f)
        {
            _movedRight = false;
        }
    }
    void MoveTowardPlayer()
    {
        if (_isRotatingBoxActive == true && transform.position.y < 2.1f && _player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed / 2f * Time.deltaTime);
        }
    }
    IEnumerator Pause()
    {
        if (_isMoveSideToSideEnumActive == true)
        {
            yield return new WaitForSeconds(12f);
            _speed = _speed * 0;
            yield return new WaitForSeconds(1f);
            _speed = 2;
            currentState = MovementState.movetowardplayer;
        }
    }
    IEnumerator ImperviousBoxCapturePlayer()
    {
        if (_isMoveTowardPlayerEnumActive == true)
        {
            yield return new WaitForSeconds(9);
            _isRotatingBoxActive = false;
            if (transform.position.y > 8.5f || transform.position.y < 1.9f)
            {
                currentState = MovementState.pauseandteleport;
            }
        }
    }
    IEnumerator MoveAndReappear()
    {

        if (_isPauseAndTeleportActive == true)
        {
            if (_isPauseAndTeleportEnumActive == true)
            {
                _speed = _speed * 0;
                _rotatingBox.gameObject.SetActive(false);
                yield return new WaitForSeconds(3f);
                if (_lives > 0)
                {
                    _spriteRenderer.color = new Color(0, 0, 0, 255);
                    _thruster1.gameObject.SetActive(false);
                    _thruster2.gameObject.SetActive(false);
                    _teleportAnimation.gameObject.SetActive(true);
                    yield return new WaitForSeconds(0.4f);
                    _isExpansionActive = true;
                    currentState = MovementState.stopallcoroutines;
                }
            }
        }
    }
    IEnumerator Reappear()
    {
        if (_lives > 0 && _isReappearEnumActive == true && _isPauseAndTeleportActive == false)
        {       
            yield return new WaitForSeconds(.001f);
            transform.position = new Vector3(Random.Range(-8, 8), 9, 0);
            if (transform.position.y > 8)
            {
        
                _spriteRenderer.color = new Color(255, 255, 255, 255);
                _thruster1.gameObject.SetActive(true);
                _thruster2.gameObject.SetActive(true);
                _teleportAnimation.gameObject.SetActive(false);
                currentState = MovementState.stopallcoroutinesfirst;
            }
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
    IEnumerator LaserTurnsOffLightning()
    {
        _lightning.gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        _isRotatingBoxActive = false;
        _wasLightningHitByLaser = false;
    }
    public void LaserHitsLightning()
    {
        _wasLightningHitByLaser = true;
        if (_isRotatingBoxActive == false && _speed != 0)
        {
            StartCoroutine(LaserTurnsOffLightning());
            _electricHumSound.Stop();
        }
    }
    public void MissileHit()
    {
        _wasHitByMissile = true;
        if (_isRotatingBoxActive == true)
        {
            _shieldHitSound.Play();
        }
    }
    public void IsEnemyLaser()
    {
        _laserHitSound.Pause();
    }
    public void IsNotEnemyLaser()
    {
        if (_lives > 0 && _isRotatingBoxActive == false)
        {
            _laserHitSound.Play();
            _shieldHitSound.Pause();
        }
        if (_lives > 0 && _isRotatingBoxActive == true && _isPauseAndTeleportActive == false)
        {
            _shieldHitSound.Play();
        }
    }
    public void Damage()
    {
        if (_wasHitByMissile == false && _isRotatingBoxActive == false && transform.position.y < 9)
        {
            _lives--;
            StartCoroutine(GotDamaged());
        }
        else if (_wasHitByMissile == true && _isRotatingBoxActive == false && transform.position.y < 9)
        {
            _lives -= 2;
            StartCoroutine(GotDamaged());
        }
        if (_lives < 1)
        {
            _speed = 0;
            StopAllCoroutines();
            _lightning.gameObject.SetActive(false);
            _thruster1.gameObject.SetActive(false);
            _thruster2.gameObject.SetActive(false);
            _explosion.gameObject.SetActive(true);
            _audioSource.Play();
            _player.AddScore(20);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            Destroy(GetComponent<Collider2D>());
            _spawnManager.EnemyDeath();
            Destroy(this.gameObject, 4f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isRotatingBoxActive == false)
        {
            _wasHitByMissile = false;
            if (_player != null)
            {
                _player.Damage();
            }
        }
        if(other.tag == "Thruster" && _isRotatingBoxActive == false)
        {
            _wasHitByMissile = false;
            if (_lives > 0 && _isRotatingBoxActive == false)
            {
                _laserHitSound.Play();
                Damage();
            }
        }
        if (other.tag == "Laser")
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
