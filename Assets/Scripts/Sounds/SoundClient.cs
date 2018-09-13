using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClient : MonoBehaviour {

    public SoundManager.SoundType type;

    [Range(0f,1f)]
    public float translationValue = 1;

	// Update is called once per frame
	void Update () {
        foreach (AudioSource src in GetComponents<AudioSource>())
        {
            src.volume = translationValue * SoundManager.getInstance().getSound(type);
        }
	}
}
