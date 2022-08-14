using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Generator : MonoBehaviour
{
    public TextAsset mapData;

    public GameObject FloorPrefab;
    public GameObject WallPrefab;

    public List<Vector2Int> DungeonTiles = new();

    private void Start() {
        DungeonTiles = JsonUtility.FromJson<MapContainer>(mapData.ToString()).Map;

        BuildDungeon();
    }

    public void BuildDungeon() {
        foreach (Vector2Int item in DungeonTiles) {
            Instantiate(FloorPrefab, new Vector3Int(item.x, 0, item.y), Quaternion.identity);
            SpawnWallsForTile(item);
        }
    }

    public void SpawnWallsForTile(Vector2Int position) {
        for (int xx = -1; xx <= 1; xx++) {
            for (int yy = -1; yy <= 1; yy++) {
                if (Mathf.Abs(xx) == Mathf.Abs(yy)) {
                    continue;
                }
                Vector2Int gridPos = position + new Vector2Int(xx, yy);
                if (!DungeonTiles.Contains(gridPos)) {
                    Vector3 direction = new Vector3(gridPos.x, 0, gridPos.y) - new Vector3(position.x, 0, position.y);
                    Instantiate(WallPrefab, new Vector3(position.x, 0, position.y), Quaternion.LookRotation(direction));
                }
            }
        }
    }
}