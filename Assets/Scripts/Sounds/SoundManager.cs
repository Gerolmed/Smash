using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager {

    private Dictionary<int, float> volSettings;

    private static SoundManager instance;

    public static SoundManager getInstance() {
        if (instance == null)
            new SoundManager();

        return instance;
    }

    public SoundManager() {
        instance = this;

        volSettings = new Dictionary<int, float>();
    }

    public void setSound(SoundType soundType, float value) {
        int soundId = (int) soundType;
        if (volSettings.ContainsKey(soundId))
            volSettings.Remove(soundId);
        volSettings.Add(soundId, value);
    }

    public float getSound(SoundType soundType)
    {
        int soundId = (int)soundType;

        if (volSettings.ContainsKey(soundId))
            return volSettings[soundId];

        return 1f;
    }

    public enum SoundType {
        MUSIC
    }
}
