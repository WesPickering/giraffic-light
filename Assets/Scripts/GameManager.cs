using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Instance
    public static GameManager Instance;

    private void Awake()
    {
        if(Instance != this && Instance != null)
        {
            Debug.Log("multiple gamemanagers found");
            return;
        }

        Instance = this;
    }
    #endregion

    #region Spawning
    [SerializeField]
    List<Transform> horizontalCarSpawns;
    [SerializeField]
    List<Transform> verticalCarSpawns;
    [SerializeField]
    List<Transform> pedestrianSpawns;
    [SerializeField]
    Transform trainSpawn;

    [SerializeField]
    List<GameObject> horizontalCarPrefabs;
    [SerializeField]
    List<GameObject> verticalCarPrefabs;
    [SerializeField]
    List<GameObject> pedestrianPrefabs;
    [SerializeField]
    List<GameObject> trainPrefabs;

    [SerializeField]
    float startCarSpawnDelay;
    [SerializeField]
    float maxCarSpawnDelay;
    float currCarSpawnDelay;

    [SerializeField]
    float startPedestrianSpawnDelay;
    [SerializeField]
    float maxPedestrianSpawnDelay;
    float currPedestrianSpawnDelay;

    [SerializeField]
    float startTrainSpawnDelay;
    [SerializeField]
    float maxTrainSpawnDelay;
    float currTrainSpawnDelay;

    float carSpawnTimer;
    float pedestrianSpawnTimer;
    float trainSpawnTimer;

    //Used to calculate lerp function
    float timeElapsed;

    [SerializeField]
    Animation trainSignal;

    private void Start() {
        trainSpawnTimer = maxTrainSpawnDelay;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        carSpawnTimer -= Time.deltaTime;
        if (carSpawnTimer < 0)
        {

            SpawnCar();
            currCarSpawnDelay = Mathf.Lerp(startCarSpawnDelay, maxCarSpawnDelay, timeElapsed / 200);
            carSpawnTimer = currCarSpawnDelay;
        }

        pedestrianSpawnTimer -= Time.deltaTime;
        if (pedestrianSpawnTimer < 0)
        {
            SpawnPedestrian();
            currPedestrianSpawnDelay = Mathf.Lerp(startPedestrianSpawnDelay, maxPedestrianSpawnDelay, timeElapsed / 200);
            pedestrianSpawnTimer = currPedestrianSpawnDelay;
        }

        trainSpawnTimer -= Time.deltaTime;
        if (trainSpawnTimer < 0)
        {
            SpawnTrain();
            currTrainSpawnDelay = Mathf.Lerp(startTrainSpawnDelay, maxTrainSpawnDelay, timeElapsed / 200);
            trainSpawnTimer = currTrainSpawnDelay;
        }
        if (trainSpawnTimer < 3 && !trainSignal.isPlaying)
        {
            trainSignal.Play();
        }
    }

    void SpawnCar()
    {
        Transform spawnLocation;
        GameObject car;
        if (Random.Range(0,10) >= 5)
        {
            spawnLocation = horizontalCarSpawns[Random.Range(0, horizontalCarSpawns.Count)];
            car = Instantiate(horizontalCarPrefabs[Random.Range(0, horizontalCarPrefabs.Count)], spawnLocation.position, spawnLocation.rotation) as GameObject;
        } else
        {
            spawnLocation = verticalCarSpawns[Random.Range(0, verticalCarSpawns.Count)];
            car = Instantiate(verticalCarPrefabs[Random.Range(0, verticalCarPrefabs.Count)], spawnLocation.position, spawnLocation.rotation) as GameObject;
        }
        car.GetComponent<CarScript>().startCar(spawnLocation.position, -spawnLocation.right);
    }

    void SpawnPedestrian()
    {
        Transform spawnLocation = pedestrianSpawns[Random.Range(0, pedestrianSpawns.Count)];
        GameObject pedestrian = Instantiate(pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Count)], spawnLocation.position, spawnLocation.rotation) as GameObject;
        pedestrian.GetComponent<PedestrianScript>().SpawnPedestrian(spawnLocation.position, spawnLocation.up);
    }

    void SpawnTrain()
    {
        GameObject train = Instantiate(trainPrefabs[Random.Range(0, trainPrefabs.Count)], trainSpawn.position, trainSpawn.rotation) as GameObject;
    }
    #endregion
    
    #region Score
    int score;

    [SerializeField]
    Text scoreText;

    public void PlusScore(int value)
    {
        Debug.Log(value + " points");
        score += value;

        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = score.ToString();
    }
    #endregion

    #region Audio
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    AudioClip click1;
    [SerializeField]
    AudioClip click2;
    [SerializeField]
    AudioClip splat;

    public void PlayClick1()
    {
        audioSource.clip = click1;
        audioSource.Play();
    }

    public void PlayClick2()
    {
        audioSource.clip = click2;
        audioSource.Play();
    }

    public void PlaySplat()
    {
        audioSource.clip = splat;
        audioSource.Play();
    }
    #endregion
}
