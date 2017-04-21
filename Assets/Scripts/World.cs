using UnityEngine;
using System.Collections.Generic;

public class World : MonoBehaviour
{
    public Map[] Maps;
    private List<Map> bufferedMaps;
    int level = 0;
    Map currentMap;
    GameObject firstPlatform;

    // Use this for initialization
    void Start()
    {
        NextMap();
        firstPlatform = (GameObject)Instantiate(currentMap.getGround(), Vector2.zero, Quaternion.identity);
        firstPlatform.GetComponent<Platform>().Init(null, true);
        level = 0;
    }

    public Map GetCurrentMap()
    {
        return currentMap;
    }

    public int GetLevel()
    {
        return level;
    }


    public void NextMap()
    {

        level++;

     
        if (bufferedMaps == null)
        {
            bufferedMaps = new List<Map>();
        }

        if (bufferedMaps.Count == 0)
        {
            foreach (var item in Maps)
            {
                bufferedMaps.Add(item);
            }
        }

        int wichMap = Random.Range(0, bufferedMaps.Count);

        if (currentMap!=null)
        {
            Destroy(currentMap.gameObject);
        }

        currentMap = Instantiate(bufferedMaps[wichMap]);
        bufferedMaps.Remove(bufferedMaps[wichMap]);

        GameObject.FindGameObjectWithTag("Sky")
            .GetComponent<Weatherer>().SetWeather(currentMap);

        Camera.main.GetComponentInChildren<Spawner>().StartSpawning();
      
    }

}
