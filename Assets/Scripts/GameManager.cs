using System;
using System.Linq;
using UnityEngine;
using Random = System.Random;
public enum LevelUpEvent
{
    SwordDamageUp,
    SwordSpeedUp,
    SwordRangeUp,
    SwordKnockbackUp,
    BulletDamageUp,
    BulletSpeedUp,
    BulletReflect,
    BulletTouchCountUp,
    PlayerSpeedUp
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    public int level = 0;
    private LevelUpEvent[] _levelUpEvents;
    // Start is called before the first frame update
    private DateTime _startTime;
    private float _currentEXP = 0;
    private float _maxEXP = 100;
    private bool _isLevelUp = false;
    public bool IsGameEnd;
    private int enemyDeadCount = -1;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        IsGameEnd = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _currentEXP = 0;
        _maxEXP = 100;
        _isLevelUp = false;
        _startTime = DateTime.Now;
        UIManager.Instance.Reset();
        UIManager.Instance.UpdateSliderBar(_currentEXP, _maxEXP);
        LevelUpEvent();
        EnemyDead();
    }

    public void EnemyDead()
    {
        enemyDeadCount++;
        UIManager.Instance.UpdateKillEnemy(enemyDeadCount);
    }

    void Update()
    {
        if (_isLevelUp)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ChooseLevelUp(_levelUpEvents[0]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ChooseLevelUp(_levelUpEvents[1]);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ChooseLevelUp(_levelUpEvents[2]);
            }
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            UpdateExp(10);
        }

    }

    public void GameEnd()
    {
        UIManager.Instance.ShowGameOver(_startTime);
        IsGameEnd = true;
    }

    public void UpdateExp(float exp)
    {
        _currentEXP += exp;
        if (_currentEXP > _maxEXP)
        {
            _currentEXP -= _maxEXP;
            LevelUpEvent();
        }
        
        UIManager.Instance.UpdateSliderBar(_currentEXP, _maxEXP);
    }

    private void LevelUpEvent()
    {
        level++;
        UIManager.Instance.SetLevelUp(level);

        if (_isLevelUp)
        {
            int random = UnityEngine.Random.Range(0, _levelUpEvents.Length);
            ChooseLevelUp(_levelUpEvents[random]);
        }
        _isLevelUp = true;
        GetThreeRandomEvents();
        
        if(level % 3 == 0)
        {
            EnemyManager.Instance.LevelUp();
        }
        _maxEXP = _maxEXP * 1.1f;
    }

    public void ChooseLevelUp(LevelUpEvent levelUpEvent)
    {
        switch (levelUpEvent)
        {
            case global::LevelUpEvent.SwordDamageUp:
                player.GetComponent<Sword>().damage += 1;
                break;
            case global::LevelUpEvent.SwordSpeedUp:
                player.GetComponent<Sword>().CoolTimeDown(0.5f);
                break;
            case global::LevelUpEvent.SwordRangeUp:
                player.GetComponent<Sword>().MultipleSize(1.1f);
                break;
            case global::LevelUpEvent.SwordKnockbackUp:
                player.GetComponent<Sword>().nuckback += 100;
                break;
            case global::LevelUpEvent.BulletDamageUp:
                player.GetComponent<Shooting>().damage += 1;
                break;
            case global::LevelUpEvent.BulletSpeedUp:
                player.GetComponent<Shooting>().bulletSpeed += 2;
                break;
            // case global::LevelUpEvent.BulletCountup:
            //     break;
            // case global::LevelUpEvent.BulletKnockbackUp:
            //     player.GetComponent<Shooting>().bulletSpeed += 2;
            //     break;
            case global::LevelUpEvent.BulletReflect:
                player.GetComponent<Shooting>().ChangeBullet(BulletType.Reflect);
                break;
            case global::LevelUpEvent.BulletTouchCountUp:
                player.GetComponent<Shooting>().enemyTouchCount++;
                break;
            case global::LevelUpEvent.PlayerSpeedUp:
                player.GetComponent<Player>().SetSpeed(player.speed + 1);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelUpEvent), levelUpEvent, null);
        }

        _isLevelUp = false;
        UIManager.Instance.CloseLevelUpCard();
    }

    public string GetCardInfo(LevelUpEvent levelUpEvent)
    {
        string content = string.Empty;
        switch (levelUpEvent)
        {
            case global::LevelUpEvent.SwordDamageUp:
                content = "검의 공격력이 증가합니다.";
                break;
            case global::LevelUpEvent.SwordSpeedUp:
                content = "검의 쿨타임이 감소합니다.";
                break;
            case global::LevelUpEvent.SwordRangeUp:
                content = "검의 범위가 10% 증가합니다.";
                break;
            case global::LevelUpEvent.SwordKnockbackUp:
                content = "검의 넉백 정도가 증가합니다.";
                break;
            case global::LevelUpEvent.BulletDamageUp:
                content = "총알 대미지가 증가합니다.";
                break;
            case global::LevelUpEvent.BulletSpeedUp:
                content = "총알 속도가 증가합니다.";
                break;
            // case global::LevelUpEvent.BulletCountup:
            //     content = "총알 장탄수가 증가합니다.";
            //     break;
            // case global::LevelUpEvent.BulletKnockbackUp:
            //     content = "총알 넉백 정도가 증가합니다.";
            //     break;
            case global::LevelUpEvent.BulletReflect:
                content = "총알이 반사됩니다";
                break;
            case global::LevelUpEvent.BulletTouchCountUp:
                content = "총알이 적에게 맞을 수 있는 횟수가 증가합니다.";
                break;
            case global::LevelUpEvent.PlayerSpeedUp:
                content = "플레이어 속도가 증가합니다.";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(levelUpEvent), levelUpEvent, null);
        }

        return content;
    }
    
    private void GetThreeRandomEvents()
    {
        Random rng = new Random();
        _levelUpEvents = Enum.GetValues(typeof(LevelUpEvent))
            .Cast<LevelUpEvent>()
            .OrderBy(x => rng.NextDouble())
            .Take(3)
            .ToArray();
        
        UIManager.Instance.ShowLevelUpCard(_levelUpEvents);
    }
}
