using System.Windows.Forms;
using Accessibility;
using GameForestMatch3.Tiles;
using Microsoft.Xna.Framework;

namespace GameForestMatch3
{
    public class MoveTileAnimation // TODO Maybe IsFinished is event?
    {
        private readonly int _animationTime;
        private int _currentTime;
        private readonly Vector2 _direction;
        private readonly TileView _tileView;
        public bool IsFinished { get; private set; }

        public MoveTileAnimation(int milliseconds, TileView tileView, Vector2 destination)
        {
            _animationTime = milliseconds;
            _currentTime = 0;
            _tileView = tileView;
            IsFinished = false;
            _direction = (destination - _tileView.Position) / _animationTime;
        }

        public void Update(GameTime gameTime)
        {
            var milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            _currentTime += milliseconds;
            if (_currentTime >= _animationTime)
            {
                IsFinished = true;
                return;
            }

            _tileView.Position += _direction * (float) milliseconds;
        }
    }
}