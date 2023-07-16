using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap; // 몬스터를 생성할 타일맵
    [SerializeField] private GameObject enemyPrefab; // 생성할 몬스터 프리팹
    public int spawnCount = 5; // 생성할 몬스터의 수
    
    void Start()
    {
        List<Vector3Int> spawnLocations = new List<Vector3Int>();

        for (int n = tilemap.cellBounds.xMin; n < tilemap.cellBounds.xMax; n++)
        {
            for (int p = tilemap.cellBounds.yMin; p < tilemap.cellBounds.yMax; p++)
            {
                Vector3Int localPlace = (new Vector3Int(n, p, (int)tilemap.transform.position.y));
                Vector3 place = tilemap.CellToWorld(localPlace);
                if (tilemap.HasTile(localPlace))
                {
                    // 이 타일에서 몬스터를 생성합니다.
                    spawnLocations.Add(localPlace);
                }
            }
        }

        // 지정된 수의 몬스터를 랜덤한 위치에 생성합니다.
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3Int spawnLocation = spawnLocations[Random.Range(0, spawnLocations.Count)];
            Vector3 spawnPosition = tilemap.GetCellCenterWorld(spawnLocation);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}