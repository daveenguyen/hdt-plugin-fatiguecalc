
namespace FatigueCalc
{
    class FatigueCalc
    {

        PlayerInfo[] players = new PlayerInfo[2];

        public PlayerInfo MainPlayer
        {
            get { return players[0]; }
            set { players[0] = value; }
        }
        
        public PlayerInfo OpponentPlayer
        {
            get { return players[1]; }
            set { players[1] = value; }
        }

        public FatigueCalc()
        {
            MainPlayer = new PlayerInfo();
            OpponentPlayer = new PlayerInfo();
        }

        public int TurnsLeft
        {
            get;
            set;
        }

        public bool isPlayersTurn = false;

        public bool playerWinsFatigueWar()
        {
            int curPlayer = 0;

            if (isPlayersTurn)
            {
                curPlayer = 1;
            }

            TurnsLeft = 0;

            while(true)
            {
                players[curPlayer].Hp = players[curPlayer].Hp - players[curPlayer].FatigueDamage - players[(curPlayer + 1) % 2].Damage + players[curPlayer].Recovery;

                if (players[curPlayer].Hp <= 0)
                {
                    return isPlayersTurn;
                }

                players[curPlayer].CardsInDeck--;
                if (players[curPlayer].CardsInDeck <= 0)
                {
                    players[curPlayer].FatigueDamage++;
                }

                curPlayer = (curPlayer + 1) % 2;

                players[curPlayer].Hp = players[curPlayer].Hp - players[curPlayer].FatigueDamage - players[(curPlayer + 1) % 2].Damage + players[curPlayer].Recovery;

                if (players[curPlayer].Hp <= 0)
                {
                    return !isPlayersTurn;
                }

                players[curPlayer].CardsInDeck--;
                if (players[curPlayer].CardsInDeck <= 0)
                {
                    players[curPlayer].FatigueDamage++;
                }

                curPlayer = (curPlayer + 1) % 2;

                TurnsLeft++;
            }
        }
    }
}
