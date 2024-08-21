using System;
using UnityEngine;

namespace Runtime.Cinematic
{
    public class Audio : IDisposable
    {
        public static AudioDatabase Database => _database;
        
        public static float MasterVolume
        {
            get => Get(_component.MasterVolume);
            set => _component.MasterVolume = Set(value);
        }
        public static float MusicVolume
        {
            get => Get(_component.MusicVolume);
            set => _component.MusicVolume = Set(value);
        }
        public static float SoundVolume
        {
            get => Get(_component.SoundVolume);
            set => _component.SoundVolume = Set(value);
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

        private static float Get(float value) =>
            Mathf.Pow(value, 5);

        private static float Set(float value) =>
            Mathf.Pow(value, 1 / 5f);
    }
}