using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Config
    {
    public static bool MusicOn
    {
        get { return PlayerPrefs.GetInt("Music", 1) == 1; }
        set {
            PlayerPrefs.SetInt("Music",value?1:0);
            SoundManager.Instance.MusicOn = value;
        }
    }
    public static bool SoundOn
    {
        get { return PlayerPrefs.GetInt("Sound", 1) == 1; }
        set {
            PlayerPrefs.SetInt("Sound",value?1:0);
            SoundManager.Instance.SoundOn = value;

        }
    
    }

    public static int SoundVolume
    {
        get {
            //int a=PlayerPrefs.GetInt("SoundVolume", 2);
            return PlayerPrefs.GetInt("SoundVolume",2);

        }
        set {
            PlayerPrefs.SetInt("SoundVolume",value);
            SoundManager.Instance.SoundVolume = value;
        }
    }

    public static int MusicVolume
    {
        get
        {
            //int b = PlayerPrefs.GetInt("MusicVolume", 2);
            return PlayerPrefs.GetInt("MusicVolume", 2);

        }
        set
        {
            PlayerPrefs.SetInt("MusicVolume", value);
            SoundManager.Instance.MusicVolume = value;
        }
    }

    ~Config() {
        PlayerPrefs.Save();
    }
    }

