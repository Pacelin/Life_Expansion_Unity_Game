using UnityEngine;

namespace Runtime.Cinematic
{
    [CreateAssetMenu]
    public class AudioDatabase : ScriptableObject
    {
        public AudioClip MainMenuMusic => _mainMenu;
        public float MainMenuMusicVolume => _mainMenuVolumeScale;
        public AudioClip GameMusic => _game;
        public float GameMusicVolume => _gameVolumeScale;
        
        public AudioClip BuildSound => _build;
        public float BuildSoundVolume => _buildVolumeScale;
        public AudioClip DestroySound => _destroy;
        public float DestroySoundVolume => _destroyVolumeScale;

        [Header("Music")] 
        [SerializeField] private AudioClip _mainMenu;
        [SerializeField] private float _mainMenuVolumeScale = 1;
        [SerializeField] private AudioClip _game;
        [SerializeField] private float _gameVolumeScale = 1;
        [Header("Sounds")]
        [SerializeField] private AudioClip _build;
        [SerializeField] private float _buildVolumeScale = 1;
        [SerializeField] private AudioClip _destroy;
        [SerializeField] private float _destroyVolumeScale = 1;
    }
}