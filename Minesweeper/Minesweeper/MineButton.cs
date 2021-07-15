using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Minesweeper
{
    class MineButton
    {
        Button button = new Button();
       public enum EnumStates { NotClicked, ClickedEmpty, ClickedNextToBomb, Bomb };

        public string[] states = Enum.GetNames(typeof(EnumStates));
        string currentState;

        public Button Button
        {
            get { return this.button; } 
            set { this.button = value; }
        }
        public string CurrentState
        {
            get { return this.currentState; }
            set { this.currentState = value; }
        }
        public MineButton(Button button)
        {
            this.button = Button;
            this.currentState = "NotClicked";
        }


    }
}
