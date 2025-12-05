using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    [Header("音效文件存放地址")]
    public string addressAudio;
    private Dictionary<string, AudioClip> audioClips;
    private List<AudioSource> audioSourceList;
    private void Start()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(addressAudio);
        foreach (AudioClip clip in clips)
        {
            audioClips[clip.name] = clip;
        }
        for(int i = 0;i<12;i++)
        {
            audioSourceList.Add(
            gameObject.AddComponent<AudioSource>());
        }
    }


    public void PlayOnce(string name)
    {
        foreach (AudioSource source in audioSourceList)
        {
            if(source.clip == null)
            {
                source.clip = audioClips[name];
                source.Play();
                DelayHelper.CallDelayed(() =>
                {
                    source.clip = null;
                }, audioClips[name].length);
                break;
            }
        }
    }
    public void PlayLoop(string name)
    {
        foreach (AudioSource source in audioSourceList)
        {
            if (source.clip == null)
            {
                source.clip = audioClips[name];
                source.loop = true;
                source.Play();
                break;
            }
        }
    }
}
