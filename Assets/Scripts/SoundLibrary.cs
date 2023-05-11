using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundLibrary : MonoBehaviour
{
    [HideInInspector]
    public static AudioSource sound;
    public List<AudioClip> audioClips;

    // Static Dictionary to reference audioclips
    public static Dictionary<string, AudioClip> library = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
        foreach (AudioClip clip in audioClips)
        {
            if (!library.ContainsKey(clip.name))
                library.Add(clip.name, clip);
            //Debug.Log(clip.name);
        }
    }
}
