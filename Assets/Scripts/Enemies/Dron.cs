using UnityEngine;

public class Dron : Enemy
{
    [Range(1, 10)] [SerializeField] private int health = 3;
    private void Awake()
    {
        initialHitPoints = health;
    }
    private void Start()
    {
        hitPoints = initialHitPoints;
        Debug.Log(hitPoints);
    }
}
