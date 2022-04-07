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
    private static AudioSource _portalAudio;
    private static AudioSource _menuSelectAudio;
	private static AudioSource _pickUpItemAudio;
	private static AudioSource _consumeHealthPillAudio;
	public static AudioSource _nearDeathAudio;
	private static AudioSource _enemyDeathAudio;
	private static AudioSource _enemyTankDeathAudio;
	public static MonoBehaviour instance; // So we can use this monobehaviour to start coroutines in non-monos



    private static bool _walkAudioPlaying = false;
    // Start is called before the first frame update
    void Awake()
    {
        InitComponents();
		instance = this;
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
        _portalAudio = audios[7];
        _menuSelectAudio = audios[8];
		_pickUpItemAudio = audios[9];
		_consumeHealthPillAudio = audios[10];
		_nearDeathAudio = audios[11];
		_enemyDeathAudio = audios[12];
		_enemyTankDeathAudio = audios[13];

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

    public static void PlayPortalAudio() {
        _portalAudio.Play();
    }

    public static void PlayMenuSelectAudio() {
		_menuSelectAudio.Play();
    }	

	public static void PlayPickUpItemAudio() {
		_pickUpItemAudio.Play();
	}

	public static void PlayConsumeHealthPillAudio() {
		_consumeHealthPillAudio.Play();
	}

	public static void PlayNearDeathAudio() {
		_nearDeathAudio.Play();
	}

	public static IEnumerator StopNearDeathAudio() {
		return FadeOut(_nearDeathAudio,2f);
	}

	public static IEnumerator FadeOut (AudioSource audioSource, float FadeTime) {
        float startVolume = audioSource.volume;
 
        while (audioSource.volume > 0) {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;
 
            yield return null;
        }
 
        audioSource.Stop();
        audioSource.volume = startVolume;
    }

	public static void PlayEnemyDeathAudio() {
		_enemyDeathAudio.Play();
	}

	public static void PlayEnemyTankDeathAudio() {
		_enemyTankDeathAudio.Play();
	}
}
