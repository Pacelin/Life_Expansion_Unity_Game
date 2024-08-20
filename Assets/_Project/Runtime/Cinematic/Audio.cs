using System;
using UnityEngine;

namespace Runtime.Cinematic
{
    public class Audio : IDisposable
    {
        public static AudioDatabase Database => _database;
        
        public static float MasterVolume
        {
            get => _component.MasterVolume;
            set => _component.MasterVolume = value;
        }
        public static float MusicVolume
        {
            get => _component.MusicVolume;
            set => _component.MusicVolume = value;
        }
        public static float SoundVolume
        {
            get => _component.SoundVolume;
            set => _component.SoundVolume = value;
        }
        
        private static AudioComponent _component;
        private static AudioDatabase _database;
        
        public Audio(AudioComponent component, AudioDatabase database)
        {
            _component = component;
            _database = database;
        }

        public void Dispose()
        {
            _component = null;
            _database = null;
        }

        public static void PlayMusic(AudioClip clip, float volume) => _component.PlayMusic(clip, volume);
        public static void PlaySound(AudioClip clip, float volume) => _component.PlaySound(clip, volume);
    }
}