using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private float _speed = 10;
    private GameObject _enemyTarget;
    private GameObject[] _enemy;
    private GameObject[] _enemy2;
    private GameObject[] _enemy3;
    private GameObject[] _enemy4;
    private GameObject[] _enemy5;
    private GameObject[] _dreadBoss;
    [SerializeField]
    private GameObject _missileThruster;
    [SerializeField]
    private AudioSource _missileLaunchSound;
    [SerializeField]
    private AudioSource _audioSource;
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _missileLaunchSound.Play();
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");
        _enemy2 = GameObject.FindGameObjectsWithTag("Enemy2");
        _enemy3 = GameObject.FindGameObjectsWithTag("Enemy3");
        _enemy4 = GameObject.FindGameObjectsWithTag("Enemy4");
        _enemy5 = GameObject.FindGameObjectsWithTag("Enemy5");
        _dreadBoss = GameObject.FindGameObjectsWithTag("Boss");
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        if (_enemy == null)
        {
            return;
        }
        else if (_enemy2 == null)
        {
            return;
        }
        else if (_enemy3 == null)
        {
            return;
        }
        else if(_enemy4 == null)
        {
            return;
        }
        else if(_enemy5 == null)
        {
            return;
        }
        else if(_dreadBoss == null)
        {
            return;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);
        if (transform.position.y > 9)
        {
            Destroy(this.gameObject);
        }
        if (_enemyTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _enemyTarget.transform.position, _speed * Time.deltaTime);

            Vector3 direction = (_enemyTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float offset = -90f;
            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
        TargetClosestEnemy();
    }
    void TargetClosestEnemy()
    {
        if (_enemyTarget == null)
        {
            float closestenemy = Mathf.Infinity;

            foreach (GameObject enemy in _enemy)
            {
                if (enemy != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy.transform.position);

                    if (distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = enemy;
                    }
                }
            }
            foreach (GameObject enemy2 in _enemy2)
            {
                if (enemy2 != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy2.transform.position);

                    if (distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = enemy2;
                    }
                }
            }
            foreach (GameObject enemy3 in _enemy3)
            {
                if (enemy3 != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy3.transform.position);

                    if (distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = enemy3;
                    }
                }
            }
            foreach (GameObject enemy4 in _enemy4)
            {
                if (enemy4 != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy4.transform.position);

                    if (distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = enemy4;
                    }
                }
            }
            foreach (GameObject enemy5 in _enemy5)
            {
                if (enemy5 != null)
                {
                    float distance = Vector3.Distance(transform.position, enemy5.transform.position);

                    if (distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = enemy5;
                    }
                }
            }
            foreach (GameObject dreadboss in _dreadBoss)
            {
                if (dreadboss != null)
                {
                   float distance = Vector3.Distance(transform.position, dreadboss.transform.position);

                   if(distance < closestenemy)
                    {
                        closestenemy = distance;
                        _enemyTarget = dreadboss;
                    }
                }     
            }
        }
    }   
    private void Destroyed()
    {
        _anim.SetTrigger("OnEnemyDeath");
        _missileLaunchSound.Stop();
        _missileThruster.gameObject.SetActive(false);
        _speed = 0;
        _audioSource.Play();
        Destroy(GetComponent<Collider2D>());
        Destroy(this.gameObject, 2.3f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            Enemy enemy = other.transform.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.MissileHit();
                enemy.Damage();
                Destroyed();
            }
        }
        else if (other.tag == "Enemy2")
        {
            Enemy2 enemy2 = other.transform.GetComponent<Enemy2>();
            if(enemy2 != null)
            {
                Destroyed();
            }
        }
        else if (other.tag == "Enemy3")
        {
            Enemy3 enemy3 = other.transform.GetComponent<Enemy3>();
            if(enemy3 != null)
            {
                enemy3.MissileHit();
                enemy3.Damage();
                Destroyed();
            }
        }
        else if (other.tag == "Enemy4")
        {
            Enemy4 enemy4 = other.transform.GetComponent<Enemy4>();
            if(enemy4 != null)
            {
                enemy4.MissileHit();
                enemy4.Damage();
                Destroyed();
            }
        }
        else if (other.tag == "Enemy5")
        {
            Enemy5 enemy5 = other.transform.GetComponent<Enemy5>();
            if(enemy5 != null)
            {
                enemy5.MissileHit();
                enemy5.Damage();
                Destroyed();
            }
        }
        else if(other.tag == "Boss")
        {
            DreadBoss dreadboss = other.GetComponent<DreadBoss>();
            if(dreadboss != null)
            {
                dreadboss.MissileHit();
                dreadboss.Damage();
                Destroyed();
            }
            
        }
    }
}
