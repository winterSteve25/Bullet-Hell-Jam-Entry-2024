using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTransfer
{
    private AudioClip[] sounds;
    public SoundTransfer(AudioClip[] clips)
    {
        this.sounds = clips;
    }
    public AudioClip WordToAudioClip(string word)
    {
        return word switch
        {
            "absorp"        => sounds[0],
            "absorp_mode"   => sounds[1],
            "button"        => sounds[2],
            "dies"          => sounds[3],
            "explode"       => sounds[4],
            "heal"          => sounds[5],
            "hurt"          => sounds[6],
            "lazer_shoot"   => sounds[7],
            "lazer_charging"=> sounds[8],
            "pickup"        => sounds[9],
            "shoot"         => sounds[10],
            _ => throw new System.NotImplementedException(word),
        };
    }
}
public class SoundsManager : MonoBehaviour
{
    private static AudioSource audioSource;
    public AudioClip[] sounds;
    private static SoundTransfer soundTransfer;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soundTransfer = new SoundTransfer(sounds);
    }


    public static void PlaySound(string keyword)
    {
        audioSource.PlayOneShot(soundTransfer.WordToAudioClip(keyword));
    }
}
