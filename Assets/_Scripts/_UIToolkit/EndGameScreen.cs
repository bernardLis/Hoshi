using Hoshi.Core;

namespace Hoshi
{
    public class EndGameScreen : FullScreenElement
    {
        public EndGameScreen() : base()
        {
            SetTitle("Wow! Good game!");

            AddContinueButton();
            AddResetButton();
            AddQuitButton();
            
        }

        void AddQuitButton()
        {
            throw new System.NotImplementedException();
        }

        void AddResetButton()
        {
            throw new System.NotImplementedException();
        }
    }
}