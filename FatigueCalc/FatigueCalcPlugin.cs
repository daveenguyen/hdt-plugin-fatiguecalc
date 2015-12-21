using Hearthstone_Deck_Tracker.Plugins;
using System;

namespace FatigueCalc
{
    public class FatigueCalcPlugin : IPlugin
    {

        private String
            _pluginName = "Fatigue Calculator",
            _author = "Davee",
            _desc = "A simple calculator based on tominko's \"Who dies first? A fatigue Table\" post.",
            _buttonText = "Settings";

        private int
            _majVer = 0,
            _minVer = 1,
            _buildVer = 0;

        public System.Windows.Controls.MenuItem MenuItem
        {
            get { return null; }
        }

        public void OnButtonPress()
        {
        }

        public void OnLoad()
        {
            PluginCode.Load();
        }

        public void OnUnload()
        {
            PluginCode.ClearText();
        }

        public void OnUpdate()
        {
        }

        #region
        public string Author
        {
            get { return _author; }
        }

        public string ButtonText
        {
            get { return _buttonText; }
        }

        public string Description
        {
            get { return _desc; }
        }

        public string Name
        {
            get { return _pluginName; }
        }

        public Version Version
        {
            get { return new Version(_majVer, _minVer, _buildVer); }
        }
        #endregion

    }
}
