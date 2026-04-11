using UnityEngine;

public class AudioManger : MonoBehaviour
{
    private static AudioManger _instance;
    private static AudioManger Instance { get
        { 
            if(_instance == null) _instance = new AudioManger();
            return _instance;
        } 
    }

    private void PlayAudio()
    {

    }
}
