using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance = null;
    [SerializeField] private Tilemap[] respawnTilemap; // 몬스터를 생성할 타일맵
    [SerializeField] private GameObject enemyPrefab; // 생성할 몬스터 프리팹
    [SerializeField] private GameObject enemyPrefab2; // 생성할 몬스터 프리팹

    [SerializeField] private GameObject effectPrefab;
    [SerializeField] private GameObject hpItemPrefab;

    private float _currentHp = 1;
    private float _currentSpeed = 3;
    private float _currentExp = 5;
    [SerializeField] private Tilemap mainBigPath; 
    private List<Vector3Int> mainBigPathPositions = new List<Vector3Int>();
    [SerializeField] private Tilemap classMainPath;
    private List<Vector3Int> classMainPathPositions = new List<Vector3Int>();
    [SerializeField] private Tilemap classBigPath;
    private List<Vector3Int> classBigPathPositions = new List<Vector3Int>();


    private List<Tuple<Tilemap, List<Vector3Int>>> respawntilemapList = new List<Tuple<Tilemap, List<Vector3Int>>>();
    public List<Enemy> enemyList = new List<Enemy>();

    private float respawnTime = 3.0f;
    private WaitForSeconds respawnTimeCoroutine;
    private float respawnCount = 3;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        respawnTimeCoroutine = new WaitForSeconds(respawnTime);
        
        for (int i = 0; i < respawnTilemap.Length; i++)
        {
            List<Vector3Int> tilePositions = new List<Vector3Int>();

            foreach (var pos in respawnTilemap[i].cellBounds.allPositionsWithin)
            {
                if (respawnTilemap[i].HasTile(pos))
                {
                    tilePositions.Add(pos);
                }
            }
            respawntilemapList.Add(new Tuple<Tilemap, List<Vector3Int>>(respawnTilemap[i], tilePositions));
        }
        
        foreach (var pos in mainBigPath.cellBounds.allPositionsWithin)
        {
            if (mainBigPath.HasTile(pos))
            {
                mainBigPathPositions.Add(pos);
            }
        }
        
        foreach (var pos in classBigPath.cellBounds.allPositionsWithin)
        {
            if (classBigPath.HasTile(pos))
            {
                classBigPathPositions.Add(pos);
            }
        }
        
        foreach (var pos in classMainPath.cellBounds.allPositionsWithin)
        {
            if (classMainPath.HasTile(pos))
            {
                classMainPathPositions.Add(pos);
            }
        }

        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void LevelUp()
    {
        respawnCount += Random.Range(1, 3);
        _currentHp = Random.Range(_currentHp, _currentHp + 0.5f);
        _currentExp = Random.Range(_currentExp, _currentExp + 10);
        _currentSpeed = Random.Range(_currentSpeed, _currentSpeed * 1.1f);
    }

    void Update()
    {
        
        if (GameManager.Instance.IsGameEnd)
        {
            return;
        }
        
        for(int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].EnemyUpdate();
        }
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Instantiate(effectPrefab, enemy.transform.position, Quaternion.identity);
        if (Random.Range(0, 10) <= 2)
        {
            Instantiate(hpItemPrefab, enemy.transform.position, Quaternion.identity);
        }
        GameManager.Instance.EnemyDead();
        enemyList.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    public Vector3 GetRandomMainBigPathPosition()
    {
        // 무작위로 하나의 타일맵을 선택합니다.
        int randomTileIndex = Random.Range(0,mainBigPathPositions.Count);
        
        // 타일 위치 중 무작위로 하나를 선택합니다.
        Vector3Int spawnPos = mainBigPathPositions[randomTileIndex];
        // 선택한 타일의 월드 위치를 가져옵니다.
        Vector3 worldPos = mainBigPath.CellToWorld(spawnPos);

        return worldPos;
    }
    
    public Vector3 GetRandomClassMainPathPosition()
    {
        // 무작위로 하나의 타일맵을 선택합니다.
        int randomTileIndex = Random.Range(0,classMainPathPositions.Count);
        
        // 타일 위치 중 무작위로 하나를 선택합니다.
        Vector3Int spawnPos = classMainPathPositions[randomTileIndex];
        // 선택한 타일의 월드 위치를 가져옵니다.
        Vector3 worldPos = classMainPath.CellToWorld(spawnPos);

        return worldPos;
    }
    
    public Vector3 GetRandomClassBigPathPosition()
    {
        // 무작위로 하나의 타일맵을 선택합니다.
        int randomTileIndex = Random.Range(0,classBigPathPositions.Count);
        
        // 타일 위치 중 무작위로 하나를 선택합니다.
        Vector3Int spawnPos = classBigPathPositions[randomTileIndex];
        // 선택한 타일의 월드 위치를 가져옵니다.
        Vector3 worldPos = classBigPath.CellToWorld(spawnPos);

        return worldPos;
    }

    IEnumerator SpawnEnemyCoroutine()
    {
        while (true)
        {
            // 무작위로 하나의 타일맵을 선택합니다.
            int randomTilemapIndex = Random.Range(0, respawntilemapList.Count);
            int randomTileIndex = Random.Range(0,respawntilemapList[randomTilemapIndex].Item2.Count);
            
            // 타일 위치 중 무작위로 하나를 선택합니다.
            Vector3Int spawnPos = respawntilemapList[randomTilemapIndex].Item2[randomTileIndex];
            // 선택한 타일의 월드 위치를 가져옵니다.
            Vector3 worldPos = respawntilemapList[randomTilemapIndex].Item1.CellToWorld(spawnPos);

            // 해당 위치에 몬스터를 생성합니다.

            for (int i = 0; i < respawnCount; i++)
            {
                string respawnName = respawnTilemap[randomTilemapIndex].name;
                Vector2 vector2;
                if (respawnName.StartsWith("Top"))
                {
                    vector2 = Vector2.down;
                }
                else if (respawnName.StartsWith("Bottom"))
                {
                    vector2 = Vector2.up;
                }
                else if (respawnName.StartsWith("Right"))
                {
                    vector2 = Vector2.left;
                }
                else
                {
                    vector2 = Vector2.zero;
                    Debug.LogError("존재하지 않는 타일맵 이름 : " + respawnName);
                    continue;
                }

                GameObject enemy;
                if (Random.Range(0, 10) > 5)
                {
                   enemy = Instantiate(enemyPrefab, worldPos, Quaternion.identity);
                }
                else
                {
                    enemy = Instantiate(enemyPrefab2, worldPos, Quaternion.identity);
                }
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                
                enemyList.Add(enemyComponent);
                enemyComponent.Init(vector2, 
                    Random.Range(_currentHp - 1, _currentHp + 1),
                    Random.Range(_currentExp, _currentExp + 5), 
                    Random.Range(_currentSpeed* 0.9f, _currentSpeed * 1.1f) 
                );
            }
            
            yield return respawnTimeCoroutine; // 1초 후에 실행
        }
    }
}