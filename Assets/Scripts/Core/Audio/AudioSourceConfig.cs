using UnityEngine;

public class AudioSourceConfig
{
   public int spatialBlend = 1;
   public int maxDistance = 100;
   public int minDistance = 1;
   public int pitch = 1;
   public float volume = 1;

    public AudioSourceConfig(float volume) {
        this.volume = volume;
    }
}
