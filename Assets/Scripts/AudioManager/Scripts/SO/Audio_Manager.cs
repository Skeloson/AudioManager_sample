using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class SoundData
{
    // -- FIELDS

    #region variables
    [SerializeField] private string MyName;
    [SerializeField] private AudioClip[] ClipArray = new AudioClip[1];
    [SerializeField, Range( 0, 1 )] private float Volume = 1;
    [SerializeField, Range( -3, 3 )] private float Pitch = 1;
    [SerializeField, Range( 0, 1 )] private float SpatialBlend = 1;
    [SerializeField] private bool Islooping;
    [SerializeField] private bool StartAtSpecificTime = false;
    [SerializeField] private float StartAtTime = 0;
    [SerializeField] private AudioMixerGroup MyMixer;
    [SerializeField] float MinDistance = 1;
    [SerializeField] float MaxDistance = 4;
    [SerializeField] AudioRolloffMode VolumeRollof = AudioRolloffMode.Logarithmic;
    #endregion

    // -- PROPERTIES

    public string Name => MyName;
    public bool Loop => Islooping;
    public float TimeToStart => (StartAtSpecificTime) ? StartAtTime : 0;
    public AudioMixerGroup Mixer => MyMixer;

    // -- METHODS

    public AudioClip GetClip()
    {
        if( ClipArray.Length == 1 )
        {
            return ClipArray[0];
        }

        int rand = Random.Range( 0, ClipArray.Length );
        return ClipArray[rand];
    }

    public void PlayClip(
        AudioSource audio_source, 
        float volume_multiplicator
        )
    {
        audio_source.clip = GetClip();

        audio_source.outputAudioMixerGroup = Mixer;
        audio_source.loop = Loop;
        audio_source.volume = Volume * volume_multiplicator;
        audio_source.pitch = Pitch;
        audio_source.spatialBlend = SpatialBlend;
        audio_source.minDistance = MinDistance;
        audio_source.maxDistance = MaxDistance;
        audio_source.rolloffMode = VolumeRollof;

        if( TimeToStart > 0 )
        {
            audio_source.time = TimeToStart;
        }

        audio_source.Play();
    }
}

[System.Serializable]
public class SoundList
{
    // -- FIELDS

    [SerializeField] string MyName = default;
    [SerializeField] SoundData[] SoundArray = default;

    // -- PROPERTIES

    public string Name => MyName;

    // -- METHODS

    public SoundData GetData(string name)
    {
        string sound_name = name;
        if( sound_name.Contains( "/" ) )
        {
            sound_name = sound_name.Split( '/' )[sound_name.Split( '/' ).Length - 1];
        }
        for (int i = 0; i < SoundArray.Length; i++)
        {
            if( SoundArray[i].Name.Equals( sound_name, System.StringComparison.Ordinal ) )
            {
                return SoundArray[i];
            }
        }
        return SoundArray[0];
    }

    public SoundData PlayClip(
        string name,
        AudioSource audio_source, 
        float volume_multiplicator=1
        )
    {
        SoundData data = GetData(name);

        if(data!=null)
        {
            data.PlayClip(audio_source, volume_multiplicator );
        }
        return data;
    }

    public string[] GetIDs()
    {
        string[] keys = new string[SoundArray.Length];

        for (int index = 0; index < SoundArray.Length; index++)
        {
            keys[index] =$"{Name}/{SoundArray[index].Name}";
        }

        return keys;
    }
}

[CreateAssetMenu(fileName = "Audio_Manager", menuName = "Data/Audio_Manager", order = 8)]
public class Audio_Manager : ScriptableObject
{
    // -- FIELDS

    [SerializeField] SoundList[] SoundBank = default;

    // -- METHODS

    #region Functions
    public SoundData GetSound(string name)
    {
        string bank_name = name.Split('/')[0];
        for (int l_i = 0; l_i < SoundBank.Length; l_i++)
        {
            if( SoundBank[l_i].Name.Equals( bank_name, System.StringComparison.Ordinal ) )
            {
                return SoundBank[l_i].GetData( name );
            }
        }
        return SoundBank[0].GetData(name);
    }

    public SoundData PlayClip(
        string name, 
        AudioSource audio_source, 
        float volume_multiplicator = 1
        )
    {
        string bank_name = name.Split('/')[0];
        for (int l_i = 0; l_i < SoundBank.Length; l_i++)
        {
            if( SoundBank[l_i].Name.Equals( bank_name, System.StringComparison.Ordinal ) )
            {
                return SoundBank[l_i].PlayClip( name, audio_source, volume_multiplicator );
            }
        }
        return SoundBank[0].PlayClip(name, audio_source, volume_multiplicator);
    }

    public string[] GetIDs()
    {
        List<string> keys = new List<string>();
        for (int i = 0; i < SoundBank.Length; i++)
        {
            keys.AddRange(SoundBank[i].GetIDs());
        }
        return keys.ToArray();
    }
    #endregion

#if UNITY_EDITOR
    /// <summary>
    /// Menu permettant de trouver un Audio manager automatiquement
    /// </summary>
    [UnityEditor.MenuItem("Tools/Audio manager")]
    static void OpenData()
    {
        string[] l_items = UnityEditor.AssetDatabase.FindAssets("t:Audio_Manager");
        if (l_items != null && l_items.Length > 0)
        {
            Audio_Manager l_manager = UnityEditor.AssetDatabase.LoadAssetAtPath<Audio_Manager>(UnityEditor.AssetDatabase.GUIDToAssetPath(l_items[0]));
            UnityEditor.Selection.activeObject = l_manager;
        }
        else
        {
            Debug.LogError("Il n'y a pas d'Audio_Manager déjà créée.");
        }
    }
#endif
}