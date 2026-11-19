using System.Collections;
using UnityEngine;

public class TrainAnim : MonoBehaviour
{
    [SerializeField] private RectTransform trainRT;
    [SerializeField] private float spawnPos;
    [SerializeField] private float maxPos;
    [SerializeField] private float speed;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;
    public bool canSpawn;
    void Start()
    {
        ResetPosition();
        StartCoroutine(SpawnTrain());
    }

    // Update is called once per frame
    void Update()
    {
        if (trainRT.anchoredPosition.x > maxPos)
            MoveTrain();
    }

    public void MoveTrain()
    {
        trainRT.anchoredPosition += new Vector2 (speed * Time.deltaTime, 0);
    }
    public IEnumerator SpawnTrain()
    {

        yield return new WaitForSeconds(Random.Range(minTime, maxTime));

        if (canSpawn)
        {
            ResetPosition();
        }
        StartCoroutine(SpawnTrain());
    }

    public void ResetPosition()
    {
        trainRT.anchoredPosition = new Vector2(spawnPos, trainRT.anchoredPosition.y);
    }
}
