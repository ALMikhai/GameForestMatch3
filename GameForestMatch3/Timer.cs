using Microsoft.Xna.Framework;

namespace GameForestMatch3
{
    public class Timer
    {
        public delegate void TimerEventHandler();

        public event TimerEventHandler TimeOut;

        public int SecondsLeft => _milliseconds / 1000;

        private int _milliseconds;

        public Timer(int milliseconds)
        {
            _milliseconds = milliseconds;
        }

        public void Update(GameTime gameTime)
        {
            if (_milliseconds <= 0)
            {
                TimeOut?.Invoke();
            }
            else
            {
                _milliseconds -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }
    }
}