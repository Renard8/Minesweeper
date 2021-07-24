using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    class MineButton
    {
        Button button = new Button();
        public enum EnumStates
        {
            Empty,
            NextToBomb,
            Bomb
        };

        public string[] states = Enum.GetNames(typeof(EnumStates));
        string currentState;
        int adjacentBombs;
        bool isRevealed;
        public Button Button
        {
            get
            {
                return this.button;
            }
            set
            {
                this.button = value;
            }
        }
        public string CurrentState
        {
            get
            {
                return this.currentState;
            }
            set
            {
                this.currentState = value;
            }
        }
        public int AdjacentBombs
        {
            get
            {
                return this.adjacentBombs;
            }
            set
            {
                this.adjacentBombs = value;
            }
        }
        public bool IsRevealed
        {
            get
            {
                return this.isRevealed;
            }
            set
            {
                this.isRevealed = value;
            }
        }
        public MineButton(Button button)
        {
            this.button = Button;
            this.currentState = "Empty";
            this.adjacentBombs = 0;
            this.isRevealed = false;
        }


    }
}
