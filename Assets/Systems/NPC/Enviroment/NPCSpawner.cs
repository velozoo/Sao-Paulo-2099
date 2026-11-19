
using System.Collections;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private GameObject[] npcPrefabs;
    [SerializeField] private RectTransform leftPoint;
    [SerializeField] private RectTransform rightPoint;

    [Header("Spawn Interval")]
    [SerializeField] private float minSpawnTime = 1f;
    [SerializeField] private float maxSpawnTime = 4f;

    [Header("Movement")]
    [SerializeField] private float minSpeed = 50f;
    [SerializeField] private float maxSpeed = 100f;
    [SerializeField] private float jumpHeight = 10f;
    [SerializeField] private float jumpSpeed = 5f;
    public bool npcSpawn;

    private void Start()
    {
        npcSpawn = true;
        StartSpawnNpc();
    }

    public void StartSpawnNpc()
    {
        StartCoroutine(SpawnRoutine());
    }



    private IEnumerator SpawnRoutine()
    {
        while (npcSpawn)
        {
            SpawnNPC();

            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnNPC()
    {
        GameObject prefab = npcPrefabs[Random.Range(0, npcPrefabs.Length)];

        RectTransform npc = Instantiate(prefab, transform)
            .GetComponent<RectTransform>();

        bool spawnLeft = Random.value > 0.5f;

        float spawnX;
        float targetX;

        if (spawnLeft)
        {
            spawnX = leftPoint.anchoredPosition.x;
            targetX = rightPoint.anchoredPosition.x;
        }
        else
        {
            spawnX = rightPoint.anchoredPosition.x;
            targetX = leftPoint.anchoredPosition.x;
        }

        npc.anchoredPosition = new Vector2(
            spawnX,
            leftPoint.anchoredPosition.y
        );

        NPCMovement movement = npc.GetComponent<NPCMovement>();

        movement.Initialize(
            targetX,
            Random.Range(minSpeed, maxSpeed),
            jumpHeight,
            jumpSpeed
        );
    }
}
