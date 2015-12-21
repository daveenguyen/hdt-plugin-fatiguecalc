using Hearthstone_Deck_Tracker;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Enums;
using Hearthstone_Deck_Tracker.Hearthstone;
using Hearthstone_Deck_Tracker.Hearthstone.Entities;
using System.Collections.Generic;
using System.Linq;

namespace FatigueCalc
{
    class PluginCode
    {
        private static PluginTextbox _textBox;
        private static FatigueCalc _calc;
        private static int? _player;

        private static Entity[] Entities
        {
            // Get the Game.Entities
            get
            {
                return Helper.DeepClone<Dictionary<int, Entity>>(
                    Hearthstone_Deck_Tracker.API.Core.Game.Entities).Values.ToArray<Entity>();
            }
        }

        private static Entity PlayerEntity
        {
            // Get the Entity representing the player, there is also the equivalent for the Opponent
            get { return Entities == null ? null : Entities.First(x => x.IsPlayer); }
        }
        
        public static void Load()
        {
            _player = null;
            _calc = new FatigueCalc();
            _textBox = new PluginTextbox();
            ClearText();

            GameEvents.OnGameEnd.Add(ClearText);
            GameEvents.OnInMenu.Add(ClearText);

            GameEvents.OnGameStart.Add(NewGame);
            GameEvents.OnPlayerDraw.Add(updateInfo);
            GameEvents.OnOpponentDraw.Add(updateInfo);
        }

        // Set the player controller id, used to tell who controls a particular
        // entity (card, health etc.)
        private static void NewGame()
        {
            _player = null;
            if (PlayerEntity != null)
                _player = PlayerEntity.GetTag(GAME_TAG.CONTROLLER);

            ClearText();
        }

        public static void ClearText() {
            _textBox.Text = "";
        }

        public static void updateInfo()
        {
            if (_player == null)
                NewGame();

            updateCalcData();

            ClearText();
            _textBox.Text += string.Format("Player HP: {0}\n", _calc.MainPlayer.Hp);
            _textBox.Text += string.Format("\tR: {0}\n\tD: {1}\n\tF: {2}\n\tDeck: {3}\n", _calc.MainPlayer.Recovery, _calc.OpponentPlayer.Damage, _calc.MainPlayer.FatigueDamage, _calc.MainPlayer.CardsInDeck);
            
            _textBox.Text += string.Format("\nOpponent HP: {0}\n", _calc.OpponentPlayer.Hp);
            _textBox.Text += string.Format("\tR: {0}\n\tD: {1}\n\tF: {2}\n\tDeck: {3}\n", _calc.OpponentPlayer.Recovery, _calc.MainPlayer.Damage, _calc.OpponentPlayer.FatigueDamage, _calc.OpponentPlayer.CardsInDeck);

            if (_calc.playerWinsFatigueWar())
            {
                _textBox.Text += string.Format("\nPlayer wins in {0} turns.", _calc.TurnsLeft);
            }
            else
            {
                _textBox.Text += string.Format("\nOpponent wins in {0} turns.", _calc.TurnsLeft);
            }
        }

        public static void updateInfo(Card c)
        {
            updateInfo();
        }

        static void updateCalcData()
        {
            int recovery = 0;
            int damage = 0;

            _calc.MainPlayer.Hp = Hearthstone_Deck_Tracker.Utility.BoardDamage.EntityHelper.GetHeroEntity(true).Health;
            _calc.MainPlayer.Hp += Hearthstone_Deck_Tracker.Utility.BoardDamage.EntityHelper.GetHeroEntity(true).GetTag(GAME_TAG.ARMOR);
            _calc.MainPlayer.Recovery = 0;
            _calc.MainPlayer.Damage = 0;
            _calc.MainPlayer.FatigueDamage = Hearthstone_Deck_Tracker.API.Core.Game.Player.Fatigue;
            _calc.MainPlayer.CardsInDeck = Hearthstone_Deck_Tracker.API.Core.Game.Player.DeckCount;

            _calc.OpponentPlayer.Hp = Hearthstone_Deck_Tracker.Utility.BoardDamage.EntityHelper.GetHeroEntity(false).Health;
            _calc.OpponentPlayer.Hp += Hearthstone_Deck_Tracker.Utility.BoardDamage.EntityHelper.GetHeroEntity(false).GetTag(GAME_TAG.ARMOR);
            _calc.OpponentPlayer.Recovery = 0;
            _calc.OpponentPlayer.Damage = 0;
            _calc.OpponentPlayer.FatigueDamage = Hearthstone_Deck_Tracker.API.Core.Game.Opponent.Fatigue;
            _calc.OpponentPlayer.CardsInDeck = Hearthstone_Deck_Tracker.API.Core.Game.Opponent.DeckCount;

            _calc.isPlayersTurn = Hearthstone_Deck_Tracker.Utility.BoardDamage.EntityHelper.IsPlayersTurn();   

            foreach (var e in Entities)
            {
                recovery = 0;
                damage = 0;
                if (e.IsInPlay)
                {
                    if (!e.IsMinion)
                    {
                        // Damage and recovery from Hero Powers
                        if (e.Card.Name.Contains("Dire Shapeshift"))
                        {
                            damage += 2;
                            recovery += 2;
                        }
                        else if (e.Card.Name.Contains("Shapeshift"))
                        {
                            damage += 1;
                            recovery += 1;
                        }
                        else if (e.Card.Name.Contains("Ballista Shot"))
                        {
                            damage += 3;
                        }
                        else if (e.Card.Name.Contains("Steady Shot"))
                        {
                            damage += 2;
                        }
                        else if (e.Card.Name.Contains("Fireblast Rank 2"))
                        {
                            damage += 2;
                        }
                        else if (e.Card.Name.Contains("Fireblast"))
                        {
                            damage += 1;
                        }
                        else if (e.Card.Name.Contains("Lesser Heal"))
                        {
                            recovery += 2;
                        }
                        else if (e.Card.Name.Contains("Heal"))
                        {
                            recovery += 4;
                        }
                        else if (e.Card.Name.Contains("Tank Up!"))
                        {
                            recovery += 4;
                        }
                        else if (e.Card.Name.Contains("Armor Up!"))
                        {
                            recovery += 2;
                        }
                        else if (e.Card.Name.Contains("Mind Shatter"))
                        {
                            damage += 3;
                        }
                        else if (e.Card.Name.Contains("Mind Spike"))
                        {
                            damage += 2;
                        }
                        else if (e.Card.Name.Contains("Lightning Jolt"))
                        {
                            damage += 2;
                        }
                    }
                    else
                    {
                        // Damage from minions in play.
                        damage += e.Attack;
                    }
                }

                if (e.GetTag(GAME_TAG.CONTROLLER) == _player)
                {
                    _calc.MainPlayer.Damage += damage;
                    _calc.MainPlayer.Recovery += recovery;
                }
                else
                {
                    _calc.OpponentPlayer.Damage += damage;
                    _calc.OpponentPlayer.Recovery += recovery;
                }
            }
        }
    }
}
