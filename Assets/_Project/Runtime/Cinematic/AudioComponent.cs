using UnityEngine;
using UnityEngine.Audio;

namespace Runtime.Cinematic
{
    public class AudioComponent : MonoBehaviour
    {
        public float MasterVolume
        {
            get
            {
                _mixer.GetFloat("MasterVolume", out var result);
                return result;
            }
            set
            {
                _mixer.SetFloat("MasterVolume", value);
            }
        }
        
        public float MusicVolume
        {
            get
            {
                _mixer.GetFloat("MusicVolume", out var result);
                return result;
            }
            set
            {
                _mixer.SetFloat("MusicVolume", value);
            }
        }
        
        public float SoundVolume
        {
            get
            {
                _mixer.GetFloat("SoundVolume", out var result);
                return result;
            }
            set
            {
                _mixer.SetFloat("SoundVolume", value);
            }
        }

        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _sound;
        [SerializeField] private AudioMixer _mixer;
        
        public void PlaySound(AudioClip clip, float volumeScale) =>
            _sound.PlayOneShot(clip, volumeScale);

        public void PlayMusic(AudioClip music, float volumeScale)
        {
            _music.Stop();
            _music.volume = volumeScale;
            _music.resource = music;
            _music.Play();
        }
    }
}