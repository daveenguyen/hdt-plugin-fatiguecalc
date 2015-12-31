using Hearthstone_Deck_Tracker;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FatigueCalc
{
    class PluginTextbox
    {
        private static HearthstoneTextBlock _info;
        private Canvas _canvas = Hearthstone_Deck_Tracker.API.Core.OverlayCanvas;

        public string Text
        {
            get { return _info.Text; }
            set { _info.Text = value; }
        }

        public PluginTextbox()
        {
            // A text block using the HS font
            _info = new HearthstoneTextBlock();
            _info.Text = "";
            _info.FontSize = 18;

            SetPositionInMs(5000);

            // Add the text block to the canvas
            _canvas.Children.Add(_info);
        }

        private async void SetPositionInMs(int ms)
        {
            await Task.Delay(ms);

            // Get canvas centre
            var fromTop = _canvas.Height / 2;
            var fromLeft = _canvas.Width / 2;

            // Give the text block its position within the canvas, roughly in the center
            Canvas.SetTop(_info, fromTop);
            Canvas.SetLeft(_info, fromLeft);
        }

        public void Unload()
        {
            _canvas.Children.Remove(_info);
        }
    }
}
