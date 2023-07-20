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
    BulletCountup,
    BulletKnockbackUp,
    BulletReflect,
    MaxHpUp,
    PlayerSpeedUp
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Player player;
    // Start is called before the first frame update

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void GameEnd()
    {
        
    }

    public void LevelUpEvent()
    {
        
    }
    
    public LevelUpEvent[] GetThreeRandomEvents()
    {
        Random rng = new Random();
        return Enum.GetValues(typeof(LevelUpEvent))
            .Cast<LevelUpEvent>()
            .OrderBy(x => rng.NextDouble())
            .Take(3)
            .ToArray();
    }
}
