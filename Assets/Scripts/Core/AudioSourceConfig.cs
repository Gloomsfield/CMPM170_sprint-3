using UnityEngine;

public class AudioSourceConfig
{
    int spatialBlend = 1;
    int maxDistance = 100;
    int minDistance = 1;
    int pitch = 1;
    float volume = AudioManager.Instance.getSFXVolume();

}
