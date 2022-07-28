namespace MineSweeper
{
    public class GameField
    {
        public GameField()
        {
            is_bomb = false;
            is_open = false;
            is_marked = false;
            is_defused = false;
            bomb_counter = 0;
            upper_layer = Constants.SQUARE;

        }
        public bool is_bomb;
        public bool is_open;
        public bool is_defused;
        public bool is_marked;
        private int bomb_counter;

        public int BombCounter
        {
            set
            {
                if (value >= 0 && value < 8)
                    bomb_counter = value;
                else
                    throw new Exception("Wrong Data func: Seter of BombCounter");
            }
            get
            {
                return bomb_counter;
            }
        }
        private string upper_layer;
        public string UpperLayer
        {
            get
            {
                return upper_layer;
            }
            set
            {
                if (value == Constants.BOOM ||
                    value == Constants.BOMB ||
                    value == Constants.FLAG ||
                    value == Constants.SQUARE ||
                    value == Constants.QUESTION ||
                    value == Constants.FALSE_DEFUSE ||
                    value == Constants.DEFUSED_BOMB ||
                    value == "  " ||
                    value == bomb_counter.ToString() + " ")
                    upper_layer = value;
                else
                    throw new Exception("Wrong Data func: Seter of UpperLayer");

            }
        }
        public void OpenSquare()
        {
            is_open = true;
            if (bomb_counter != 0)
                upper_layer = bomb_counter.ToString() + " ";
            else if (!is_bomb)
                upper_layer = "  ";
            else
                upper_layer = Constants.BOOM;

        }
    }
}