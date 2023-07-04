using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;
    private bool _isEnemyLaser = false;
    private Player _player;
    private Enemy5 _enemy5;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindObjectOfType<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false)
        {
            MoveUP();
        }
        else
        {
            MoveDOWN();
        }

    }
    void MoveUP()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 8)
        {
            //parent of tripleshot lasers
            if (transform.parent != null)
            {
                //all lasers in tripleshot get destroyed because parent gets destroyed. 
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if (transform.position.x > 11)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if (transform.position.x < -11f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    void MoveDOWN()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if(transform.position.y > 8)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if (transform.position.x > 11f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
        if (transform.position.x < -11f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                _player.IsNotThePlayerLaser();
                player.Damage();
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "Player" && _isEnemyLaser == false)
        {
            _player.IsThePlayerLaser();
        }
        if (other.tag == "Powerup" && _isEnemyLaser == true)
        {
            Powerup powerup = other.GetComponent<Powerup>();
            if (powerup != null)
            {
                powerup.UpInSmoke();
                Destroy(other.gameObject);
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "Enemy" && _isEnemyLaser == false)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.IsNotEnemyLaser();
                enemy.Damage();
            }
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy" && _isEnemyLaser == true)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.IsEnemyLaser();
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "Enemy2" && _isEnemyLaser == false)
        {
            Destroy(this.gameObject);
        }
        else if (other.tag == "Enemy2" && _isEnemyLaser == true)
        {
            Enemy2 enemy2 = other.GetComponent<Enemy2>();
            if(enemy2 != null)
            {
                enemy2.IsEnemyLaser();
            }
        }
        if (other.tag == "Enemy3" && _isEnemyLaser == false)
        {
            Enemy3 enemy3 = other.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.IsNotEnemyLaser();
                enemy3.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Enemy3" && _isEnemyLaser == true)
        {
            Enemy3 enemy3 = other.GetComponent<Enemy3>();
            if (enemy3 != null)
            {
                enemy3.IsEnemyLaser();
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "Enemy4" && _isEnemyLaser == false)
        {
            Destroy(this.gameObject);
            Enemy4 enemy4 = other.GetComponent<Enemy4>();
            if (enemy4 != null)
            {
                enemy4.IsNotEnemyLaser();
                enemy4.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Enemy4" && _isEnemyLaser == true)
        {
            Enemy4 enemy4 = other.GetComponent<Enemy4>();
            if (enemy4 != null)
            {
                enemy4.IsEnemyLaser();
                Destroy(this.gameObject);
            }
        }
        if (other.tag == "Enemy5" && _isEnemyLaser == false)
        {
            Destroy(this.gameObject);
            Enemy5 enemy5 = other.GetComponent<Enemy5>();
            if (enemy5 != null)
            {
                enemy5.IsNotEnemyLaser();
                enemy5.Damage();
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Enemy5" && _isEnemyLaser == true)
        {
            Enemy5 enemy5 = other.GetComponent<Enemy5>();
            enemy5.IsEnemyLaser();
            Destroy(this.gameObject);
        }
        if (other.tag == "Lightning" && _isEnemyLaser == false)
        {
            Lightning lightning = other.GetComponent<Lightning>();
            if (lightning != null)
            {
                lightning.LightningIntercepts();
            }
            Destroy(this.gameObject);
        }
        if (other.tag == "Pulse" && _isEnemyLaser == true)
        {
            Destroy(this.gameObject);
        }
        if (other.tag == "Boss" && _isEnemyLaser == false)
        {
            DreadBoss dreadboss = other.GetComponent<DreadBoss>();
            if (dreadboss != null)
            {
                dreadboss.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}

