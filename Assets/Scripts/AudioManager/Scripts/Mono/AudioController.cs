using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    // -- FIELDS

    #region variables
    [SerializeField] private Audio_Manager Manager;
    private AudioSource[] AudioSourceArray;
    private int NumberOfSources;
    private int CurrentSource;
    private Dictionary<string, int> LoopingSources = new Dictionary<string, int>();

    [Header("Optionnal")] [SerializeField] private float GlobalVolume = 1;
    #endregion

    // -- METHODS

    #region Functions
    public AudioSource PlaySound(string name)
    {
        GetAviableSource();
        SoundData data = Manager.PlayClip(name, AudioSourceArray[CurrentSource],GlobalVolume);

        if( data.Loop )
        {
            LoopingSources.Add( data.Name, CurrentSource );
        }

        CurrentSource++;

        if (CurrentSource >= NumberOfSources)
        {
            CurrentSource = 0;
        }

        return AudioSourceArray[CurrentSource];
    }

    public AudioSource PlaySound(
        string name, 
        Vector3 position
        )
    {
        AudioSource l_target = PlaySound(name);
        l_target.transform.position = position;
        return l_target;
    }
    
    private void GetAviableSource()
    {
        if( !AudioSourceArray[CurrentSource].isPlaying )
        {
            return;
        }

        bool l_break = false;
        int current = CurrentSource;

        while (!l_break)
        {
            if (AudioSourceArray[current].isPlaying)
            {
                current++;

                if (current >= NumberOfSources)
                {
                    current = 0;
                }

                if (current == CurrentSource)
                {
                    List<AudioSource> l_sources = new List<AudioSource>(AudioSourceArray);
                    GameObject l_go = new GameObject("fx");
                    l_go.transform.SetParent(transform);
                    l_sources.Add(l_go.AddComponent<AudioSource>());
                    AudioSourceArray = l_sources.ToArray();
                    CurrentSource = NumberOfSources;
                    NumberOfSources++;
                    l_break = true;
                }
            }
            else
            {
                l_break = true;
                CurrentSource = current;
            }
        }
    }

    public void StopSound(string name)
    {
        if( !LoopingSources.ContainsKey( name ) )
        {
            return;
        }

        AudioSourceArray[LoopingSources[name]].Stop();
        LoopingSources.Remove(name);
    }

    public void UpdateVolume(float volume)
    {
        GlobalVolume = volume;
    }

    #endregion
    #region Static Functions
    public static AudioSource S_PlaySound(string name)
    {
        return Instance.PlaySound(name);
    }
    
    public static AudioSource S_PlaySound(
        string name, 
        Vector3 position
        )
    {
        return Instance.PlaySound(name, position);
    }
    
    public static void S_StopSound(string name)
    {
        Instance.StopSound(name);
    }

    // -- UNITY

    // Start is called before the first frame update
    void Start()
    {
        AudioSourceArray = transform.GetComponentsInChildren<AudioSource>();
        NumberOfSources = AudioSourceArray.Length;
    }
    #endregion
}