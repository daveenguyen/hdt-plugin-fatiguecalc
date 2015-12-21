using Hearthstone_Deck_Tracker;
using System.Windows.Controls;

namespace FatigueCalc
{
    class PluginTextbox
    {
        private static HearthstoneTextBlock _info;

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

            // Get the HDT Overlay canvas object
            var canvas = Hearthstone_Deck_Tracker.API.Core.OverlayCanvas;
            // Get canvas centre
            var fromTop = canvas.Height / 2;
            var fromLeft = canvas.Width / 2;
            // Give the text block its position within the canvas, roughly in the center
            Canvas.SetTop(_info, fromTop);
            Canvas.SetLeft(_info, fromLeft);

            // Add the text block to the canvas
            canvas.Children.Add(_info);
        }
    }
}
