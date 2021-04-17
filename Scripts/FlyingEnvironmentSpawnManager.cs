using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnvironmentSpawnManager : MonoBehaviour
{
    public static FlyingEnvironmentSpawnManager Instance = null;

    [Header("Side Objects")]
    [SerializeField] private GameObject spawnPositionLeft;
    [SerializeField] private GameObject spawnPositionRight;
    [Header("Ghost Spawn settings")]
    [SerializeField] private GameObject ghostPrefab;
    [SerializeField] private int maxAmountOfGhosts = 4;
    [SerializeField] private float minimumGhostSpawnTime = 5f;
    [SerializeField] private float maximumGhostSpawnTime = 25f;
    [Header("Bird Spawn Settings")]
    [SerializeField] private GameObject birdPrefab;
    [SerializeField] private int maxAmountOfBirds = 4;
    [SerializeField] private float minimumBirdsSpawnTime = 5f;
    [SerializeField] private float maximumBirdsSpawnTime = 25f;
    [Header("Runtime Variables")]
    public int numberOfActiveGhosts = 0;
    public int numberOfActiveBirds = 0;

    private Coroutine spawnGhosts;
    private Coroutine spawnBirds;

    private void Awake()
    {
        // Make sure that this class is a singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        // Start the spawning Coroutines
        spawnGhosts = StartCoroutine(SpawnGhosts());
        spawnBirds = StartCoroutine(SpawnBirds());
    }

    IEnumerator SpawnGhosts()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minimumGhostSpawnTime, maximumGhostSpawnTime));

            if (numberOfActiveGhosts < maxAmountOfGhosts)
            {
                SpawnGhost();
            }
        }
    }

    IEnumerator SpawnBirds()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minimumBirdsSpawnTime, maximumBirdsSpawnTime));

            if (numberOfActiveBirds < maxAmountOfBirds)
            {
                SpawnBird();
            }
        }
    }

    /// <summary>
    /// Spawns in a ghost randomly on the left or right side of the screen.
    /// </summary>
    public void SpawnGhost()
    {
        numberOfActiveGhosts++;
        ChooseSpawnPoint(ghostPrefab);
    }

    /// <summary>
    /// Spawns in a bird randomly on the left or right side of the screen.
    /// </summary>
    public void SpawnBird()
    {
        numberOfActiveBirds++;
        ChooseSpawnPoint(birdPrefab);
    }

    /// <summary>
    /// Stops the constant spawning of birds.
    /// </summary>
    public void StopSpawningBirds()
    {
        if (spawnBirds != null)
        {
            StopCoroutine(spawnBirds);
        }
    }

    /// <summary>
    /// Stops the constant spawning of ghosts.
    /// </summary>
    public void StopSpawningGhosts()
    {
        if (spawnGhosts != null)
        {
            StopCoroutine(spawnGhosts);
        }
    }

    private void ChooseSpawnPoint(GameObject prefab)
    {
        // Using the spawn_point enum to randomly choose a side
        if (Random.Range(0, 2) == (int)Spawn_Point.Left)
        {
            Instantiate(prefab, spawnPositionLeft);
        }
        else
        {
            Instantiate(prefab, spawnPositionRight);
        }
    }

    
}
