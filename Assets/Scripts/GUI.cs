using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;


public class GUI : MonoBehaviour
{
    public GameObject CanvasGame, CanvasPause;//, HelpPanel, OptionsPanel, TrophiesPanel, InventoryPanel;
    public GameObject PauseButton, StartButton;//,HelpButton, OptionsButton, TrophiesButton, InventoryButton;
    public GameObject[] Buttons,Panels;
    public GameObject Dude;
    public GameObject StartBreaker;
    public Text[] CountersUIText;
    public AudioMixer mixer;
    public Slider scrollMusic, scrollAmbient, scrollSFX;

    // Use this for initialization
    void Start()
    {
       
        if (PlayerPrefs.HasKey(AUDIO_MUSIC_VOLUME))
        {
            scrollMusic.value = PlayerPrefs.GetFloat(AUDIO_MUSIC_VOLUME);
            mixer.SetFloat(AUDIO_MUSIC_VOLUME, scrollMusic.value == scrollMusic.minValue ? -80 : scrollMusic.value);
            scrollAmbient.value = PlayerPrefs.GetFloat(AUDIO_AMBIENT_VOLUME);
            mixer.SetFloat(AUDIO_AMBIENT_VOLUME, scrollAmbient.value == scrollAmbient.minValue ? -80 : scrollAmbient.value);
            scrollSFX.value = PlayerPrefs.GetFloat(AUDIO_SFX_VOLUME);
            mixer.SetFloat(AUDIO_SFX_VOLUME, scrollSFX.value == scrollSFX.minValue ? -80 : scrollSFX.value);

            foreach (Text textUI in CountersUIText)
            {
             Stats.AddStatCounter(textUI.name, PlayerPrefs.GetFloat(textUI.name));
            }
        }
    }

    public void ResetOptions()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        StartBreaker.GetComponent<Rigidbody2D>().isKinematic = false;
        Destroy(StartBreaker.transform.parent.gameObject, 3);
        Dude.SetActive(true);
        PauseButton.SetActive(true);
        StartButton.SetActive(false);
    }

    public void Restart()
    {
        Application.LoadLevel(Application.loadedLevelName);
    }

    public void Pause()
    {
        if (Time.timeScale == 0.0f)
        {
            Time.timeScale = 1.0f;
        }
        else
        {
            Time.timeScale = 0.0f;
            foreach (Text textUI in CountersUIText)
            {
                textUI.text = Stats.GetStatCounter(textUI.name);
            }
            DisableEnable(3);
        }

        CanvasPause.SetActive(Time.timeScale == 0.0f);
        CanvasGame.SetActive(Time.timeScale == 1.0f);


    }

    public void Help()
    {
        DisableEnable(0);
    }

    public void Options()
    {
        DisableEnable(1);
    }

    public void Trophy()
    {
        DisableEnable(2);
    }

    public void Inventory()
    {
        DisableEnable(3);
    }

    private void DisableEnable(int whichOne)
    {

        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i!=whichOne)
            {
                Buttons[i].GetComponent<Image>().color = new Color(255,255,255,130);
                Panels[i].SetActive(false);
            }
        }
        Buttons[whichOne].GetComponent<Image>().color = new Color(255, 255, 255, 255);
        Panels[whichOne].SetActive(true);
    }

    public void VolumeMusic()
    {
        mixer.SetFloat(AUDIO_MUSIC_VOLUME, scrollMusic.value== scrollMusic.minValue ? -80:scrollMusic.value);
    }
    public void VolumeAmbient()
    {
        mixer.SetFloat(AUDIO_AMBIENT_VOLUME, scrollAmbient.value == scrollAmbient.minValue ? -80 : scrollAmbient.value);
    }
    public void VolumeSFX()
    {
        mixer.SetFloat(AUDIO_SFX_VOLUME, scrollSFX.value == scrollSFX.minValue ? -80 : scrollSFX.value);
    }

    const string    AUDIO_MUSIC_VOLUME = "MusicVol",
                    AUDIO_AMBIENT_VOLUME = "AmbientVol",
                    AUDIO_SFX_VOLUME = "SFXVol"
                ;

    public void Quit()
    {
        PlayerPrefs.SetFloat(AUDIO_MUSIC_VOLUME, scrollMusic.value);
        PlayerPrefs.SetFloat(AUDIO_AMBIENT_VOLUME, scrollAmbient.value);
        PlayerPrefs.SetFloat(AUDIO_SFX_VOLUME, scrollSFX.value);

        foreach (Text textUI in CountersUIText)
        {
            PlayerPrefs.SetFloat(textUI.name, Stats.GetStatCounterFloat(textUI.name));
        }

        PlayerPrefs.Save();
        
        Application.Quit();
    }

}
