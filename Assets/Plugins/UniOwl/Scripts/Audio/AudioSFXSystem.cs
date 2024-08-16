using UniOwl.Pool;
using UnityEngine;
using UnityEngine.Audio;

namespace UniOwl.Audio
{
    public class AudioSFXSystem : MonoBehaviour
    {
        private static AudioSFXSystem _instance;

        [SerializeField] private AudioSource audioSourcePrefab;
        [SerializeField] private AudioMixerGroup sfxGroup;

        private ComponentPool<AudioSource> _pool;

        private void Awake()
        {
            _instance = this;
            _pool = new ComponentPool<AudioSource>(audioSourcePrefab, 0);
        }

        public static void PlayCue2D(AudioContainer container) =>
            PlayCue(container, Vector3.zero, _instance.transform, true);
        
        public static void PlayCue(AudioContainer container, Vector3 position) =>
            PlayCue(container, position, _instance.transform, false);
        
        public static void PlayCue(AudioContainer container, Vector3 position, Transform parent) =>
            PlayCue(container, position, parent, false);

        private static void PlayCue(AudioContainer container, Vector3 position, Transform parent, bool is2D)
        {
            if (!container) return;
            
            var clip = container.GetRandomClip();

            var source = _instance._pool.Get(position, Quaternion.identity, parent);
            source.clip = clip;
            source.outputAudioMixerGroup = container.GroupOverride ? container.GroupOverride : _instance.sfxGroup;
            source.spatialBlend = is2D ? 0f : 1f;
            
            source.volume = container.GetVolume();
            source.pitch = container.GetPitch();
            
            source.Play();
            
            Destroy(source.gameObject, clip.length);
        }

        public static void PlayClip2D(AudioClip clip) =>
            PlayClip(clip, Vector3.zero, _instance.transform, true);

        public static void PlayClip(AudioClip clip, Vector3 position) =>
            PlayClip(clip, position, _instance.transform, false);

        public static void PlayClip(AudioClip clip, Vector3 position, Transform parent) =>
            PlayClip(clip, position, parent, false);
        
        private static void PlayClip(AudioClip clip, Vector3 position, Transform parent, bool is2D)
        {
            if (!clip) return;
            
            var source = _instance._pool.Get(position, Quaternion.identity, parent);
            source.clip = clip;
            source.outputAudioMixerGroup = _instance.sfxGroup;
            source.spatialBlend = is2D ? 0f : 1f;
            source.Play();
            
            Destroy(source.gameObject, clip.length);
        }
    }
}
