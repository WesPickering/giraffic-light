using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /*
    #region Lights
    static List<int> intersectionLights = new List<int>();
    static List<LightsUI> lightUIs = new List<LightsUI>();

    public static int AddLight(LightsUI lui)
    {
        lightUIs.Add(lui);
        intersectionLights.Add(0);
        lui.SetRed();
        return lightUIs.Count - 1;
    }

    public static void ChangeLight(int index)
    {
        switch (intersectionLights[index])
        {
            case 0: intersectionLights[index] = 1; lightUIs[index].SetGreen(); break;
            case 1: intersectionLights[index] = 0; lightUIs[index].SetRed(); break;
        }
    }
    #endregion
    */

    #region Spawning
    public List<Transform> carSpawns;
    public List<Transform> pedestrianSpawns;
    public Transform trainSpawn;

    public List<GameObject> carPrefabs;
    public List<GameObject> pedestrianPrefabs;
    public List<GameObject> trainPrefabs;

    public float carSpawnDelay;
    public float pedestrianSpawnDelay;

    float carSpawnTimer;
    float pedestrianSpawnTimer;

    private void Update()
    {
        carSpawnTimer -= Time.deltaTime;
        if(carSpawnTimer < 0)
        {
            SpawnCar();
            carSpawnTimer = carSpawnDelay;
        }

        pedestrianSpawnTimer -= Time.deltaTime;
        if (pedestrianSpawnTimer < 0)
        {
            SpawnPedestrian();
            pedestrianSpawnTimer = pedestrianSpawnDelay;
        }
    }

    public void SpawnCar()
    {
        Transform spawnLocation = carSpawns[Random.Range(0, carSpawns.Count)];
        GameObject car = Instantiate(carPrefabs[Random.Range(0, carPrefabs.Count)], spawnLocation.position, spawnLocation.rotation) as GameObject;
        car.GetComponent<CarScript>().startCar(spawnLocation.position, spawnLocation.up);
    }

    public void SpawnPedestrian()
    {
        Transform spawnLocation = pedestrianSpawns[Random.Range(0, pedestrianSpawns.Count)];
        GameObject pedestrian = Instantiate(pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Count)], spawnLocation.position, spawnLocation.rotation) as GameObject;
        pedestrian.GetComponent<PedestrianScript>().SpawnPedestrian(spawnLocation.position, spawnLocation.up);
    }

    public void SpawnTrain()
    {
        GameObject train = Instantiate(trainPrefabs[Random.Range(0, trainPrefabs.Count)], trainSpawn.position, trainSpawn.rotation) as GameObject;
    }
    #endregion

    
    #region Score
    static int score;

    public static void PlusScore()
    {
        Debug.Log("yay, you got points. actually not really cuz theres no point system yet lol");
    }
    #endregion
}
