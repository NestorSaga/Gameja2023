using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using UnityEngine.SceneManagement;


public class MainMenuController : MonoBehaviour
{
    public FMOD.Studio.EventInstance eventInstance;
    public string path;



    private void Start()
    {
        StartMenuMusic();
    }

    public void StartMenuMusic()
    {
        eventInstance = FMODUnity.RuntimeManager.CreateInstance(path);

        eventInstance.start();
        eventInstance.setVolume(0.5f);
    }

    public void StopMusicAndChangeScene()
    {
        DontDestroyOnLoad(transform.gameObject);
        eventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        eventInstance.release();
    }


}
