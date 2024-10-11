using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class Game : MonoBehaviour
{
    [SerializeField] private GameObject playBtn;
    [SerializeField] private GameObject menuBtn;
    [SerializeField] private GameObject settingBtn;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject levelPanel;

    [SerializeField] private GameObject endLvl;
    [SerializeField] private GameObject overTime;

    [SerializeField] private LevelGenerate lg;

    public TextMeshProUGUI wlTxt;
    public TextMeshProUGUI rnTxt;

    public bool lvlIsActive = false;
    private int soundSet;
    private int musicSet;

    [SerializeField] private Slider sound, music;
    [SerializeField] private AudioMixerGroup mixer;
    [SerializeField] private AudioSource aEffsects;
    float vol;

    private Timer _time;
    [SerializeField] private GameObject timer;

    // Start is called before the first frame update
    void Start()
    {
        _time = timer.GetComponent<Timer>();
        LoadMusic();
        LoadSound();
        //lg = new LevelGenerate();
    }

    // Update is called once per frame
    void Update()
    {
        if (_time.time <= 0 && lvlIsActive == true)
        {
            endLvl.SetActive(true);
            lvlIsActive = false;
            _time.startTimer = false;
            wlTxt.text = "LOSE";
            rnTxt.text = "RETRY";
            overTime.SetActive(true);
        }
    }

    public void clickPlayBtn()
    {
        playBtn.SetActive(false);
        levelPanel.SetActive(true);
        lvlIsActive = true;
        menuBtn.SetActive(true);
        _time.startTimer = true;
        _time.time = 30;
        lg.LoadLevel();
        aEffsects.Play();
    }

    public void clickMenuBtn()
    {
        playBtn.SetActive(true);
        levelPanel.SetActive(false);
        lvlIsActive = false;
        menuBtn.SetActive(false);
        settingBtn.SetActive(true);
        settingPanel.SetActive(false);
        aEffsects.Play();
    }

    public void clickReady()
    {
        if (lg.isReady == true)
        {
            endLvl.SetActive(true);
            lvlIsActive = false;
            _time.startTimer = false;
            wlTxt.text = "WIN";
            rnTxt.text = "NEXT";
            overTime.SetActive(false);
            lg.lastLvl++;
            PlayerPrefs.SetInt("lastLvl", lg.lastLvl);
            PlayerPrefs.Save();
            aEffsects.Play();
        }
    }

    public void clickHome() 
    {
        playBtn.SetActive(true);
        levelPanel.SetActive(false);
        menuBtn.SetActive(false);
        endLvl.SetActive(false);
        aEffsects.Play();
    }

    public void clickRNBtn()
    {
        if (rnTxt.text != "NEXT")
        {
            overTime.SetActive(true);
        }

        endLvl.SetActive(false);
        _time.startTimer = true;
        _time.time = 30;
        lvlIsActive = true;
        lg.LoadLevel();
        aEffsects.Play();
    }

    public void clickSetting()
    {
        if (lvlIsActive == true)
            levelPanel.SetActive(false);
        else
        {
            playBtn.SetActive(false);
            menuBtn.SetActive(true);
        }

        _time.startTimer = false;
        settingBtn.SetActive(false);
        settingPanel.SetActive(true);
        aEffsects.Play();
    }

    public void clickSave()
    {
        if (lvlIsActive == true)
        { 
            levelPanel.SetActive(true);
            _time.startTimer = true;
        }
        else
            playBtn.SetActive(true);

        settingBtn.SetActive(true);
        settingPanel.SetActive(false);
        aEffsects.Play();
    }

    void LoadMusic()
    {
        if (PlayerPrefs.HasKey("music"))
        {
            music.value = PlayerPrefs.GetFloat("music");
            Setting.Value(ref vol, music.value);
            //mixer.audioMixer.SetFloat("Music", vol);
        }
        else
        {
            music.value = 1;
            Setting.Value(ref vol, music.value);
        }
            mixer.audioMixer.SetFloat("Music", vol);
    }

    void LoadSound()
    {
        if (PlayerPrefs.HasKey("sound"))
        {
            sound.value = PlayerPrefs.GetFloat("sound");
            Setting.Value(ref vol, sound.value);
            //mixer.audioMixer.SetFloat("Sound", vol);
        }
        else
        {
            sound.value = 1;
            Setting.Value(ref vol, sound.value);
        }
            mixer.audioMixer.SetFloat("Sound", vol);
    }
}
