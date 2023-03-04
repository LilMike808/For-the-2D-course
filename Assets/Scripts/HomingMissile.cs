using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private GameObject _enemyTarget;
    private GameObject[] _enemy;
    private float _speed = 8f;
    // Start is called before the first frame update
    void Start()
    {
        _enemy = GameObject.FindGameObjectsWithTag("Enemy");

        if(_enemy == null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 8)
        {
            Destroy(this.gameObject);
        }
        ComplexMovement();
        TargetClosestEnemy();
    }
    void ComplexMovement()
    {
        if(_enemyTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _enemyTarget.transform.position, _speed * Time.deltaTime);

            Vector3 direction = (_enemyTarget.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            float offset = -90f;

            transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
        }
    }
    void TargetClosestEnemy()
    {
        float closestenemy = Mathf.Infinity;

        foreach (GameObject enemy in _enemy)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            if(distance < closestenemy)
            {
                closestenemy = distance;
                _enemyTarget = enemy;
            }
        }
    }
}
