using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public int spawnInterval = 5;
    public int spawnDistanceMin = 5, spawnDistanceMax = 10;
    public List<GameObject> passiveMobs, hostileMobs;
    [Range(5, 50)] public int passiveMax = 10, hostileMax = 10;

    private static Transform player, passives, hostiles;
    public static Bounds map;
    private static float lastSpawnTime;
    public static int passiveCount, hostileCount;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        passives = GetComponentsInChildren<Transform>()[1];
        hostiles = GetComponentsInChildren<Transform>()[2];
        map = new Bounds(Vector3.zero, new Vector3(45, 45, 0));
        lastSpawnTime = Time.time;

        for (int i = 0; i < 3; i++) Spawn(true);
        for (int i = 0; i < 3; i++) Spawn(false);

    }

    private void FixedUpdate()
    {
        if (Time.time == lastSpawnTime + spawnInterval)
        {
            lastSpawnTime = Time.time;

            if (Random.Range(0, 2) == 0)
            {
                if (passiveCount < passiveMax) Spawn(true);
            }
            else if (hostileCount < hostileMax) Spawn(false);
        }
    }

    private void Spawn(bool passive)
    {
        GameObject mob = Instantiate(
            passive ? passiveMobs[Random.Range(0, passiveMobs.Count)] : hostileMobs[Random.Range(0, hostileMobs.Count)],
            passive ? passives : hostiles);

        mob.transform.position = (Vector2)player.position
            + Random.Range(spawnDistanceMin, spawnDistanceMax) * Random.insideUnitCircle.normalized;

        if (!map.Contains(mob.transform.position)) mob.GetComponent<MobBehavior>().Despawn();
    }
}