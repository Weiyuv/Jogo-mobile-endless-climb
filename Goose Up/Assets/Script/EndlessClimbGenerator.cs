using UnityEngine;
using System.Collections.Generic;

public class EndlessClimbGenerator : MonoBehaviour
{
    [System.Serializable]
    public class PlatformSpawnRule
    {
        public int minScore;
        public GameObject prefab;
        public float chance = 0.2f;
    }

    public GameObject defaultPlatformPrefab;
    public List<PlatformSpawnRule> spawnRules;

    public Transform player;

    public int platformsAhead = 15;

    public float minYDistance = 2f;
    public float maxYDistance = 4f;

    public float minXDistance = 1f;
    public float maxXDistance = 3f;

    public float minTotalDistance = 2.5f;

    private float highestY;
    private float lastX;
    private Vector3 lastPos;

    private List<GameObject> platforms = new List<GameObject>();

    void Start()
    {
        highestY = player.position.y;
        lastX = player.position.x;
        lastPos = player.position;

        for (int i = 0; i < platformsAhead; i++)
        {
            SpawnPlatform();
        }
    }

    void Update()
    {
        if (player.position.y + 6f > highestY)
        {
            SpawnPlatform();
        }

        for (int i = platforms.Count - 1; i >= 0; i--)
        {
            if (platforms[i] != null &&
                platforms[i].transform.position.y < player.position.y - 15f)
            {
                Destroy(platforms[i]);
                platforms.RemoveAt(i);
            }
        }
    }

    void SpawnPlatform()
    {
        Vector3 pos;

        int attempts = 0;

        do
        {
            float yStep = Random.Range(minYDistance, maxYDistance);
            float newY = highestY + yStep;

            float xOffset = Random.Range(minXDistance, maxXDistance) *
                            (Random.value > 0.5f ? 1 : -1);
            float newX = lastX + xOffset;

            pos = new Vector3(newX, newY, 0);

            attempts++;

        } while (Vector3.Distance(pos, lastPos) < minTotalDistance && attempts < 10);

        highestY = pos.y;
        lastX = pos.x;
        lastPos = pos;

        GameObject prefabToSpawn = defaultPlatformPrefab;

        int score = ScoreManager.instance != null ? ScoreManager.instance.score : 0;

        for (int i = 0; i < spawnRules.Count; i++)
        {
            if (score >= spawnRules[i].minScore)
            {
                if (Random.value < spawnRules[i].chance)
                {
                    prefabToSpawn = spawnRules[i].prefab;
                }
            }
        }

        GameObject plat = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        platforms.Add(plat);
    }
}