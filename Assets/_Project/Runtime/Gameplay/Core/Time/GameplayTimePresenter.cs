using System;
using R3;
using UnityEngine;
using Zenject;

namespace Runtime.Gameplay.Core
{
    public class GameplayTimePresenter : IInitializable, IDisposable
    {
        private readonly GameplayTime _model;
        private readonly GameplayTimeView _view;
        private readonly CompositeDisposable _disposables;
        
        public GameplayTimePresenter(GameplayTime model, GameplayTimeView view)
        {
            _model = model;
            _view = view;
            _disposables = new();
        }
        
        public void Initialize()
        {
            _model.Value.Subscribe(time => _view.SetTime(time))
                .AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}