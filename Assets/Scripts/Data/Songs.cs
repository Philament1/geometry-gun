using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Songs
{
    public struct Song
    {
        public string name;
        public float minPitch, maxPitch, pitchIncreaseRate;

        public Song(string _name, float _minPitch, float _maxPitch)
        {
            name = _name;
            minPitch = _minPitch;
            maxPitch = _maxPitch;
            pitchIncreaseRate = (maxPitch - minPitch) / MAX_PITCH_TIME;
        }
    }

    const float MAX_PITCH_TIME = 45f;

    public static Song current;
    public static readonly Song[] songs = new Song[]
    {
        new Song("Caramelldansen", 0.6f, 1.3f),
        new Song("22", 0.9f, 1.2f),
        new Song("All Star", 0.9f, 1.4f),
        new Song("Jenny", 0.8f, 1.3f),
        new Song("Love Like Woe", 0.7f, 1.3f)
    };
}
