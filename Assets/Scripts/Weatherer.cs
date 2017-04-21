using UnityEngine;
using System.Collections;

public class Weatherer : MonoBehaviour
{

    private float hour = 8, minutes = 0;
    private float delay = 0.1f;
    private SpriteRenderer spriteRenderer;
    private float maxDark = 20;
    private int nightStart = 19, nightEnd = 23, morningStart = 3, morningEnd = 7;
    private Light skyLight;
    private GameObject moon, sun;
    private float OrbitSpeed = 13.0f;
    private Camera mainCam;
    private GameObject weatherGenrerated;

    // Use this for initialization
    void Start()
    {

        mainCam = Camera.main;
        foreach (Transform item in gameObject.GetComponentInChildren<Transform>())
        {
            if (item.name == "moon")
            {
                moon = item.gameObject;
            }
            if (item.name == "sun")
            {
                sun = item.gameObject;
            }

        }

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        skyLight = gameObject.GetComponentInChildren<Light>();

        InvokeRepeating("tick", delay, delay);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mainCam.transform.position.x, transform.position.y, 0);
    }

    void tick()
    {
        addMinute(1);
        setClarity();
        setMoonAndSun();
    }

    void addMinute(float m)
    {
        minutes += m;
        if (minutes >= 60)
        {
            minutes = 0;
            addHour(1);
        }

    }

    void addHour(float m)
    {
        hour += m;
        if (hour > 23)
        {
            hour = 0;
        }

    }

    void setClarity()
    {

        float newCompColor;
        //Night
        if (hour >= nightStart && hour <= nightEnd)
        {
            newCompColor = (255 - maxDark) / ((nightEnd - nightStart) * 60);
            newCompColor = (((hour - nightStart) * 60 + minutes) * newCompColor);
            newCompColor = Mathf.Clamp(1.0f - newCompColor / (255 - maxDark), 0.2f, 1.0f);
            changeSkyColor(newCompColor);
        }
        //Morning
        if (hour >= morningStart && hour <= morningEnd)
        {
            newCompColor = (255 - maxDark) / ((morningEnd - morningStart) * 60);
            newCompColor = (((hour - morningStart) * 60 + minutes) * newCompColor);
            newCompColor = Mathf.Clamp(newCompColor / (255 - maxDark), 0.2f, 1.0f);
            changeSkyColor(newCompColor);
        }

    }

    void changeSkyColor(float rgb)
    {

        Color newColor = new Color(rgb, rgb, rgb);
        spriteRenderer.color = newColor;
        skyLight.intensity = Mathf.Clamp(rgb, 0.5f, 0.8f);
    }

    void setMoonAndSun()
    {

        if (sun != null)
        {
            sun.transform.RotateAround(transform.position + new Vector3(0, -3, 0), -Vector3.forward, Time.deltaTime * OrbitSpeed);
        }
        if (moon != null)
        {
            moon.transform.RotateAround(transform.position + new Vector3(0, -3, 0), -Vector3.forward, Time.deltaTime * OrbitSpeed);
        }
    }

    public void SetWeather(Map currentMap)
    {
        if (weatherGenrerated != null)
        {
            Destroy(weatherGenrerated, 0);

        }
        GameObject weatherModel = currentMap.getWeather();
        weatherGenrerated = (GameObject)Instantiate(weatherModel, gameObject.transform.position,Quaternion.identity);
        weatherGenrerated.transform.parent = gameObject.transform;
        weatherGenrerated.transform.Translate(Vector2.up * Camera.main.orthographicSize);
        weatherGenrerated.transform.rotation = weatherModel.transform.rotation;
       
        //weatherGenrerated.transform.position = new Vector3(0, (2f * Camera.main.orthographicSize / 2), 0);

    }
}
