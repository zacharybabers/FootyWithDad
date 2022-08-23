using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource ballAudioSource;

    [SerializeField] private List<AudioClip> kickClips;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomKick()
    {
        int kick = Random.Range(0, kickClips.Count);
        ballAudioSource.PlayOneShot(kickClips[kick]);
    }
}
