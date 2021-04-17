using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class used to manage the power up drops.
/// </summary>
public class PowerupManager : MonoBehaviour
{
    public static PowerupManager Instance;

    [Header("Drop prefabs")]
    [SerializeField] private GameObject redLotusPrefab;
    [SerializeField] private GameObject blueLotusPrefab;
    [Header("Drop settings")]
    [SerializeField] private int minimumBeforeDrop = 3;
    [SerializeField] private int maximumBeforeDrop = 15;
    [SerializeField] private int maximumDifferenceBetweenDrops = 3;   
    
    private int killsWithoutDrop;
    private int blueLotusDrops;
    private int redLotusDrops;

    private void Awake()
    {
        // Make this class a singleton
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// This function will determine if you get a drop and returns it.
    /// </summary>
    /// <returns>Returns Null if there is no drop, returns drop prefab is there is a drop.</returns>
    public GameObject GetDrop()
    {
        // Checks if we are due for a drop.
        if (killsWithoutDrop >= maximumBeforeDrop)
        {
            killsWithoutDrop = 0;
            return GetRandomLotus();
        }
        // Check if we hit the trashhold to get a drop.
        else if (killsWithoutDrop >= minimumBeforeDrop)
        {
            // Randomly check for a drop.
            int random = Random.Range(0, 100);
            if (random < 69)
            {
                killsWithoutDrop = 0;
                return GetRandomLotus();
            }
            else
            {
                killsWithoutDrop++;
                return null;
            }
        }
        // We get no drop and update our counter.
        else
        {
            killsWithoutDrop++;
            return null;
        }
    }

    private GameObject GetRandomLotus()
    {
        // Check if the either lotus has dropped too much compaired to the other.
        if (blueLotusDrops - redLotusDrops >= maximumDifferenceBetweenDrops)
        {            
            redLotusDrops++;
            return redLotusPrefab;
        }
        else if (redLotusDrops - blueLotusDrops >= maximumDifferenceBetweenDrops)
        {
            blueLotusDrops++;
            return blueLotusPrefab;
        }
        // If the difference between the 2 drops are close randomize 1. 
        else
        {
            if (Random.Range(0,2) == 0)
            {
                redLotusDrops++;
                return redLotusPrefab;
            }
            else
            {
                blueLotusDrops++;
                return blueLotusPrefab;
            }
        }
    }
}
