using Common.Data;
using Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISystemConfig : UIWindow
{
    public Image musicOff;
    public Image soundOff;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    

    // Start is called before the first frame update
    void Start()
    {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
     /**   
        Config.MusicOn= this.toggleMusic.isOn;
        Config.SoundOn = this.toggleSound.isOn;
        Config.MusicVolume = (int)this.sliderMusic.value;
        Config.SoundVolume = (int)this.sliderSound.value;
    **/
        }
    public override void OnYesClick() {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicToggle() {
        musicOff.enabled = !toggleMusic.isOn;
        Config.MusicOn = toggleMusic.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundToggle()
    {
        
        soundOff.enabled = !toggleSound.isOn;
        Config.SoundOn = toggleSound.isOn;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundVolumn() {
        Config.SoundVolume = (int)sliderSound.value;
        PlaySound();
    }

    public void MusicVolumn()
    {

        Config.MusicVolume = (int)sliderMusic.value;
        //PlayMusic();
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if (Time.realtimeSinceStartup-lastPlay>0.1) {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }

    }
    private void PlayMusic()
    {
        if (Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            MapDefine map = DataManager.Instance.Maps[MapService.Instance.CurrentMapId];
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlayMusic(map.Music);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Config.MusicOn = this.toggleMusic.isOn;
        //Config.SoundOn = this.toggleSound.isOn;
        //Config.MusicVolume = (int)this.sliderMusic.value;
        //Config.SoundVolume = (int)this.sliderSound.value;
    }
}
