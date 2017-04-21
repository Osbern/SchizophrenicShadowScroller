using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Map : MonoBehaviour
{
    public GameObject[] Platforms, FlyingPlatforms;

    public GameObject[] Backs;
    public GameObject[] Grasses;
    public GameObject[] Trees;
    public GameObject[] Bushes;
    public GameObject[] Houses;
    public GameObject[] Patches;
    public GameObject[] MushRooms;
    public GameObject[] Flowers;
    public GameObject[] Miscs;

    public GameObject Sky;
    public GameObject[] Clouds;
    public GameObject[] Flyers;
    
    public GameObject[] Ennemies;
    public GameObject Boss;

    public Color PortalsColor;

    public GameObject[] Weathers;

    private GameObject[][] items;
    private int[] itemsMax;

    public int MaxBacks = 10,
        MaxGrasses = 50,
        MaxTrees = 10,
        MaxBushes = 5,
        MaxHouses = 1,
        MaxPatches = 3,
        MaxMushrooms = 6,
        MaxFlowers = 8,
        MaxMiscs = 2,
        MaxClouds = 2,
        MaxFlyers = 3,
        MaxFlyingPlatforms = 3;

    public AudioClip audioMusic;

    // Use this for initialization
    void Start()
    {
     
    }

    private void init()
    {
        if (audioMusic != null)
        {
              AudioSource.PlayClipAtPoint(audioMusic, transform.position);
        }

        items = new GameObject[][] 
        { Backs, Grasses, Trees, Bushes, Houses, Patches, MushRooms, Flowers, Miscs,FlyingPlatforms };

        itemsMax = new int[] 
        { MaxBacks, MaxGrasses, MaxTrees, MaxBushes, MaxHouses, MaxPatches, MaxMushrooms, MaxFlowers, MaxMiscs,MaxFlyingPlatforms };

        if (MaxClouds > 0)
        {
            for (int i = 0; i < MaxClouds; i++)
            {
                Instantiate(Clouds[Random.Range(0, Clouds.Length)]);
            }
        }
        if (MaxFlyers > 0)
        {
            for (int i = 0; i < MaxFlyers; i++)
            {
                Instantiate(Flyers[Random.Range(0, Flyers.Length)]);
            }
        }
    }

    public Color GetPortalsColors()
    {
        return PortalsColor;
    }

    public GameObject getGround()
    {
        return Platforms[Random.Range(0, Platforms.Length)];
    }

    public List<GameObject> getGenerated()
    {
        if (items==null)
        {
            init();

        }
        List<GameObject> generated = new List<GameObject>();
        for (int i = 0; i < items.Length; i++)
        {
           
            if (items[i].Length > 0)
            {
             
                int nb = Random.Range(itemsMax[i] / 3, itemsMax[i]);
             
                for (int j = 0; j < nb; j++)
                {
                    generated.Add(items[i][Random.Range(0, items[i].Length)]);
                }
            }

        }

        return generated;
    }

    public GameObject getEnnemy()
    {
        return Ennemies[Random.Range(0, Ennemies.Length)];
    }

    public GameObject getWeather()
    {
        return Weathers[Random.Range(0, Weathers.Length)];
    }

    public GameObject getBoss()
    {
        return Boss;
    }


}
