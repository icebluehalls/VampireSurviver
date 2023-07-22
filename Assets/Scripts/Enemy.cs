using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyMoveType
{
    Respawn,
    ChasingPlayer,
    GoMainClassPath,
    GoMainBigPath,
    GoBigClassPath
}

public class Enemy : MonoBehaviour
{
    private PlayerPosition _playerPosition;
    
    private EnemyMoveType _enemyMoveType;
    private Vector2 _moveDirection;
    private PlayerPosition _currentStation;
    
    private string _currentTileTag;
    private Rigidbody2D _rigidbody2D;
    private Vector2 originalVelocity;
    private float _hp;
    private float _exp;
    public float _speed;

    public void Init(Vector2 moveDirection, float hp, float exp, float speed)
    {
        _hp = hp;
        _exp = exp;
        _speed = speed;
        _moveDirection = moveDirection;
        _enemyMoveType = EnemyMoveType.Respawn;
        _playerPosition = GameManager.Instance.player.playerPosition;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        originalVelocity = _rigidbody2D.velocity;
    }

    // Update is called once per frame
    public void EnemyUpdate()
    {

        if (_playerPosition != GameManager.Instance.player.playerPosition)
        {
            if (_enemyMoveType == EnemyMoveType.Respawn)
            {
                
            }
            else
            {
                ChangeEnemyMove();
            }
        }
        
        switch (_enemyMoveType)
        {
            case EnemyMoveType.Respawn:
                transform.position += (Vector3)(_moveDirection * Time.deltaTime * _speed);
                break;
            case EnemyMoveType.ChasingPlayer:
                ChasingPlayer();
                break;
            case EnemyMoveType.GoBigClassPath:
            case EnemyMoveType.GoMainBigPath:
            case EnemyMoveType.GoMainClassPath:
                Vector2 moveAmount = _moveDirection * _speed * Time.deltaTime;
                transform.position += (Vector3)moveAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void ChasingPlayer()
    {
        // 플레이어의 방향으로 이동
        Vector2 direction = (GameManager.Instance.player.transform.position - transform.position).normalized;
        transform.Translate(direction * _speed * Time.deltaTime);
    }
    void ChangeEnemyMove()
    {
        _playerPosition = GameManager.Instance.player.playerPosition;

        if (_playerPosition == _currentStation)
        {
            _enemyMoveType = EnemyMoveType.ChasingPlayer;
        }
        else
        {
            Vector2 directionPosition = Vector2.zero;
            if (_playerPosition == PlayerPosition.MainHall)
            {
                if (_currentStation == PlayerPosition.BigHall)
                {
                    directionPosition = EnemyManager.Instance.GetRandomMainBigPathPosition();
                    _enemyMoveType = EnemyMoveType.GoMainBigPath;
                }
                else if (_currentStation == PlayerPosition.LeftClass)
                {
                    directionPosition = EnemyManager.Instance.GetRandomClassMainPathPosition();
                    _enemyMoveType = EnemyMoveType.GoMainClassPath;
                }
                else
                {
                    Debug.Log("case1");
                }
            }
            else if (_playerPosition == PlayerPosition.BigHall)
            {
                if (_currentStation == PlayerPosition.MainHall)
                {
                    directionPosition = EnemyManager.Instance.GetRandomMainBigPathPosition();
                    _enemyMoveType = EnemyMoveType.GoMainBigPath;
                }
                else if (_currentStation == PlayerPosition.LeftClass)
                {
                    directionPosition = EnemyManager.Instance.GetRandomClassBigPathPosition();
                    _enemyMoveType = EnemyMoveType.GoBigClassPath;
                }
                else
                {
                    Debug.Log("case2");
                }
            }
            else if (_playerPosition == PlayerPosition.LeftClass)
            {
                if (_currentStation == PlayerPosition.MainHall)
                {
                    directionPosition = EnemyManager.Instance.GetRandomClassMainPathPosition();
                    _enemyMoveType = EnemyMoveType.GoMainClassPath;
                }
                else if (_currentStation == PlayerPosition.BigHall)
                {
                    directionPosition = EnemyManager.Instance.GetRandomClassBigPathPosition();
                    _enemyMoveType = EnemyMoveType.GoBigClassPath;
                }
                else
                {
                    Debug.Log("case3");
                }
            }
            else
            {
                Debug.Log("case4");
            }

            if (directionPosition == Vector2.zero)
            {
                Debug.LogError("directionPosition is zero");
                return;
            }
            _moveDirection = (directionPosition - (Vector2)transform.position).normalized;

        }
    }
    
    private void Dead()
    {
        GameManager.Instance.UpdateExp(_exp);
        EnemyManager.Instance.RemoveEnemy(this);
        Destroy(gameObject);
    }

    IEnumerator InitVelocity()
    {
        yield return new WaitForSeconds(0.5f);
        _rigidbody2D.velocity = originalVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("MainHall"))
        {
            _currentStation = PlayerPosition.MainHall;
            ChangeEnemyMove();
        }
        else if (other.transform.CompareTag("BigHall"))
        {
            _currentStation = PlayerPosition.BigHall;
            ChangeEnemyMove();
        }
        else if (other.transform.CompareTag("LeftClass"))
        {
            _currentStation = PlayerPosition.LeftClass;
            ChangeEnemyMove();
        }

        if (other.CompareTag("Sword"))
        {
            Sword sword = other.gameObject.GetComponent<Sword>();

            _hp -= GameManager.Instance.player.sword.damage;

            if (_hp <= 0)
            {
                Dead();
            }
            
            Vector2 bounceDirection = transform.position - other.transform.position;

            // 정규화하여 방향 벡터만을 얻습니다.
            bounceDirection = bounceDirection.normalized;

            // 플레이어를 반대방향으로 튕겨냅니다.
            _rigidbody2D.AddForce(bounceDirection * GameManager.Instance.player.sword.nuckback, ForceMode2D.Force);

            StartCoroutine(InitVelocity());
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.CompareTag("Bullet"))
        {
            Bullet bullet = other.gameObject.GetComponent<Bullet>();
            bullet.enemyTouchCount--;

            if (bullet.enemyTouchCount <= 0)
            {
                Destroy(other.gameObject);
            }
            
            _hp -= bullet.damage;
            
            if (_hp <= 0)
            {
                Dead();
            }
        }
    }
}
