using Jamcelin.Runtime.Core;
using Runtime.Cinematic;
using UnityEngine;
using Zenject;

namespace _Project.Runtime.Gameplay
{
    [CreateAssetMenu]
    public class MusicInstaller : JamInstaller
    {
        [SerializeField] private bool _game;
        protected override void Install()
        {
            Container.BindInterfacesTo<MusicStarter>()
                .AsSingle()
                .WithArguments(_game);
        }
    }

    public class MusicStarter : IInitializable
    {
        [Inject] private bool _isGame;
        
        public void Initialize()
        {
            if (_isGame)
                Audio.PlayMusic(Audio.Database.GameMusic, Audio.Database.GameMusicVolume);
            else
                Audio.PlayMusic(Audio.Database.MainMenuMusic, Audio.Database.MainMenuMusicVolume);
        }
    }
}