using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;

namespace GameForestMatch3.Utils
{
    public class MoveTileAnimation
    {
        private readonly int _animationTime;
        private int _currentTime;
        private readonly Vector2 _direction;
        private readonly TileView _tileView;
        private readonly Vector2 _destination;
        public bool IsFinished { get; private set; }

        public MoveTileAnimation(int milliseconds, TileView tileView, Vector2 destination)
        {
            _animationTime = milliseconds;
            _currentTime = 0;
            _tileView = tileView;
            _destination = destination;
            IsFinished = false;
            _direction = (destination - _tileView.Position) / _animationTime;
        }

        public void Update(GameTime gameTime)
        {
            var milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            _currentTime += milliseconds;
            if (_currentTime >= _animationTime)
            {
                _tileView.Position = _destination;
                IsFinished = true;
                return;
            }
            
            _tileView.Position += _direction * milliseconds;
        }
    }
}