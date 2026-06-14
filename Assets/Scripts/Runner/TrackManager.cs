using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float initialSpeed = 6f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float maxSpeed = 20f;

    [Serializable]
    public class CollectibleEntry
    {
        public GameObject prefab;
        public float weight = 1f;
        public float yOffset = 0f;
    }

    [Header("Collectibles")]
    [SerializeField] private CollectibleEntry[] collectiblePrefabs;
    [SerializeField][Range(0f, 1f)] private float collectibleChance = 0.5f;

    [Header("Obstacles")]
    [SerializeField] private CollectibleEntry[] obstaclePrefabs;
    [SerializeField][Range(0f, 1f)] private float obstacleChance = 0.3f;

    [Header("Track")]
    [SerializeField] private float laneWidth = 2.5f;
    [SerializeField] private float spawnInterval = 8f;
    [SerializeField] private float lookaheadDistance = 60f;
    [SerializeField] private float despawnDistance = 10f;

    private readonly List<GameObject> activeCollectibles = new();
    private readonly List<GameObject> activeObstacles = new();
    private float furthestSpawnedZ;
    private float worldSpeed;

    public float WorldSpeed => worldSpeed;

    public void ResetSpeed() => worldSpeed *= 0.9f;

    private void Start()
    {
        worldSpeed = initialSpeed;
        furthestSpawnedZ = -spawnInterval;
        FillAhead();
    }

    private void Update()
    {
        worldSpeed = Mathf.MoveTowards(worldSpeed, maxSpeed, acceleration * Time.deltaTime);

        float scroll = worldSpeed * Time.deltaTime;
        furthestSpawnedZ -= scroll;
        ScrollCollectibles(scroll);
        FillAhead();
        DespawnBehind();
    }

    private void FillAhead()
    {
        while (furthestSpawnedZ < lookaheadDistance)
        {
            furthestSpawnedZ += spawnInterval;
            SpawnStrip(furthestSpawnedZ);
        }
    }

    private void ScrollCollectibles(float scroll)
    {
        foreach (GameObject c in activeCollectibles)
        {
            if (c == null) continue;
            Vector3 pos = c.transform.localPosition;
            pos.z -= scroll;
            c.transform.localPosition = pos;
        }

        foreach (GameObject o in activeObstacles)
        {
            if (o == null) continue;
            Vector3 pos = o.transform.localPosition;
            pos.z -= scroll;
            o.transform.localPosition = pos;
        }
    }

    private void DespawnBehind()
    {
        for (int i = activeCollectibles.Count - 1; i >= 0; i--)
        {
            GameObject c = activeCollectibles[i];
            if (c == null || c.transform.localPosition.z < -despawnDistance)
            {
                if (c != null) Destroy(c);
                activeCollectibles.RemoveAt(i);
            }
        }

        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            GameObject o = activeObstacles[i];
            if (o == null || o.transform.localPosition.z < -despawnDistance)
            {
                if (o != null) Destroy(o);
                activeObstacles.RemoveAt(i);
            }
        }
    }

    private void SpawnStrip(float centerZ)
    {
        bool[] laneHasObstacle = new bool[5];

        if (obstaclePrefabs != null && obstaclePrefabs.Length > 0)
        {
            for (int lane = 0; lane < 5; lane++)
            {
                if (UnityEngine.Random.value > obstacleChance) continue;

                float x = (lane - 2) * laneWidth;
                float zOffset = UnityEngine.Random.Range(-spawnInterval * 0.3f, spawnInterval * 0.3f);
                CollectibleEntry obstacleEntry = PickWeighted(obstaclePrefabs);
                GameObject o = Instantiate(obstacleEntry.prefab, transform);
                o.transform.localPosition = new Vector3(x, obstacleEntry.yOffset, centerZ + zOffset);
                activeObstacles.Add(o);
                laneHasObstacle[lane] = true;
            }
        }

        if (collectiblePrefabs == null || collectiblePrefabs.Length == 0) return;

        for (int lane = 0; lane < 5; lane++)
        {
            if (laneHasObstacle[lane]) continue;
            if (UnityEngine.Random.value > collectibleChance) continue;

            float x = (lane - 2) * laneWidth;
            float zOffset = UnityEngine.Random.Range(-spawnInterval * 0.3f, spawnInterval * 0.3f);
            CollectibleEntry collectibleEntry = PickWeighted(collectiblePrefabs);
            GameObject c = Instantiate(collectibleEntry.prefab, transform);
            c.transform.localPosition = new Vector3(x, collectibleEntry.yOffset, centerZ + zOffset);
            activeCollectibles.Add(c);
        }
    }

    private CollectibleEntry PickWeighted(CollectibleEntry[] entries)
    {
        float total = 0f;
        foreach (var entry in entries)
            total += entry.weight;

        float roll = UnityEngine.Random.Range(0f, total);
        float cumulative = 0f;
        foreach (var entry in entries)
        {
            cumulative += entry.weight;
            if (roll < cumulative)
                return entry;
        }

        return entries[entries.Length - 1];
    }
}
