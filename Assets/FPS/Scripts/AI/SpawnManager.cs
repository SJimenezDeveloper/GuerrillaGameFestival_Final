using System.Collections;
using UnityEngine;
using Unity.FPS.Game;
using System;

namespace Unity.FPS.AI
{
    public class SpawnManager : MonoBehaviour
    {
        public GameObject[] spawnPoints;
        public GameObject[] enemies;
        public int waveCount;
        public int wave;
        public int enemyType;
        public bool spawning;

        private int enemiesSpawned;
        private GameFlowManager gameManager;

        private void Start()
        {
            waveCount = 5;
            wave = 1;
            spawning = false;
            enemiesSpawned = 0;
            gameManager = FindObjectOfType<GameFlowManager>();

        }

        private void Update()
        {
            if (gameManager.GameIsEnding)
            {
                return;
            }
            if (!spawning)
            {
                StartCoroutine(SpawnWave(waveCount));
            }
        }

        IEnumerator SpawnWave(int waveCount)
        {
            spawning = true;
            yield return new WaitForSeconds(5);
            for (int i = 0; i < waveCount; i++)
            {
                SpawnEnemy(wave);
                yield return new WaitForSeconds(1);
            }
            wave++;
            spawning = false;
            waveCount += 2;
            yield break;
        }

        private void SpawnEnemy(int wave)
        {
            int spawnPos = UnityEngine.Random.Range(0, spawnPoints.Length);

            if (wave < 3)
            {
                enemyType = 0;
            }
            else if (wave < 6)
            {
                enemyType = UnityEngine.Random.Range(0, Math.Min(2, enemies.Length));
            }
            else
            {
                enemyType = UnityEngine.Random.Range(0, Math.Min(3, enemies.Length));
            }

            if (enemies[enemyType] != null && spawnPoints[spawnPos] != null)
            {
                GameObject newEnemy = Instantiate(
                    enemies[enemyType],
                    spawnPoints[spawnPos].transform.position,
                    spawnPoints[spawnPos].transform.rotation
                );

                // Buscar el PatrolPath más cercano al enemigo generado
                PatrolPath closestPath = FindClosestPatrolPath(newEnemy.transform.position);
                if (closestPath != null)
                {
                    EnemyPatrol patrol = newEnemy.GetComponent<EnemyPatrol>();
                    if (patrol != null)
                    {
                        patrol.PatrolPath = closestPath;
                    }
                }
            }
        }

        private PatrolPath FindClosestPatrolPath(Vector3 position)
        {
            PatrolPath[] allPaths = FindObjectsOfType<PatrolPath>();
            PatrolPath closestPath = null;
            float shortestDistance = Mathf.Infinity;

            foreach (var path in allPaths)
            {
                if (path.PathNodes.Count > 0)
                {
                    float distance = Vector3.Distance(position, path.PathNodes[0].position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        closestPath = path;
                    }
                }
            }

            return closestPath;
        }
    }
}
