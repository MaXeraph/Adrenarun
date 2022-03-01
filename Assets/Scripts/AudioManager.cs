using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static GameObject player;
    private static AudioSource _dashAudio;
    private static AudioSource _fireAudio;
    private static AudioSource _impactAudio;
    private static AudioSource _injuryAudio;
    private static AudioSource _musicAudio;
    private static AudioSource _reloadAudio;
    private static AudioSource _walkAudio;

    private static bool _walkAudioPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        InitComponents();
    }

    void InitComponents() {
        AudioSource[] audios = GetComponents<AudioSource>();
        _dashAudio = audios[0];
        _fireAudio = audios[1];
        _impactAudio = audios[2];
        _injuryAudio = audios[3];
        _musicAudio = audios[4];
        _reloadAudio = audios[5];
        _walkAudio = audios[6];

        player = GameObject.FindGameObjectWithTag("Player");
    }

    public static void PlayDashAudio() {
        _dashAudio.Play();
    }

    public static void PlayFireAudio() {
        _fireAudio.Play();
    }

    public static void PlayImpactAudio() {
        _impactAudio.Play();
    }

    public static void PlayInjuryAudio() {
        _injuryAudio.Play();
    }

    public static void PlayWalkAudio() {
        if (!_walkAudioPlaying) {
            _walkAudio.Play();
            _walkAudioPlaying = true;
        }
    }

    public static void UpdateMusicAudio(float pitch) {
        _musicAudio.pitch = pitch;
    }

    public static void StopWalkAudio() {
        _walkAudio.Stop();
        _walkAudioPlaying = false;
    }

    public static void PlayReloadAudio() {
        float _reloadSpeed = player.GetComponent<Weapon>()._reloadSpeed;
        _reloadAudio.pitch = 1 / _reloadSpeed;
        _reloadAudio.Play();
    }



}
