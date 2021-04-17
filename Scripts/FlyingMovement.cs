using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FlyingTypeEnum
{
    ghost,
    bird
}
/// <summary>
/// Script for movement of flying background objects.
/// </summary>
public class FlyingMovement : MonoBehaviour
{
    [Header("Unit settings")]
    [SerializeField] private FlyingTypeEnum flyingType;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float maxY;
    [SerializeField] private float minY;

    private Vector3 targetPos;

    private void Awake()
    {
        // Based on position get the other side of the screen as target direction 
        if (transform.position.x > 0)
        {
            targetPos = new Vector3(-28, transform.position.y);
        }
        else
        {
            targetPos = new Vector3(28, transform.position.y);
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void Update()
    {
        // Randomly go up and down
        float y = targetPos.y;
        y += Random.Range(-1f, 1f);
        y = Mathf.Clamp(y,minY,maxY);
        targetPos.y = y;

        // Lerp towards the target
        transform.position = Vector3.Lerp(transform.position, targetPos, movementSpeed * Time.fixedDeltaTime);

        // If it reached the other side of screen destroy itself
        if (targetPos.x > 0)
        {
            if (transform.position.x > 25)
            {
                switch (flyingType)
                {
                    case FlyingTypeEnum.ghost:
                        FlyingEnvironmentSpawnManager.Instance.numberOfActiveGhosts--;
                        break;
                    case FlyingTypeEnum.bird:
                        FlyingEnvironmentSpawnManager.Instance.numberOfActiveBirds--;
                        break;
                }
                Destroy(gameObject);
            }            
        }
        else if (targetPos.x < 0)
        {
            if (transform.position.x < -25)
            {
                switch (flyingType)
                {
                    case FlyingTypeEnum.ghost:
                        FlyingEnvironmentSpawnManager.Instance.numberOfActiveGhosts--;
                        break;
                    case FlyingTypeEnum.bird:
                        FlyingEnvironmentSpawnManager.Instance.numberOfActiveBirds--;
                        break;
                }
                Destroy(gameObject);
            }
        }
    }
}
