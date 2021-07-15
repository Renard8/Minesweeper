using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Minesweeper.MineButton;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //Class Minebutton array
        MineButton[,] mineButtons = new MineButton[9, 9];

        private void Form1_Load(object sender, EventArgs e)
        {

            //Adding the button objects to the array
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mineButtons[j, i] = new MineButton(new Button());
                    mineButtons[j, i].Button.Click += buttonClick;
                }

            }

            //resizing and placing buttons
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mineButtons[j, i].Button.Size = new Size(25, 25);
                    mineButtons[j, i].Button.Location = new Point((i + 1) * 25, (j + 1) * 25);
                    Controls.Add(mineButtons[j, i].Button);

                }

            }

            //populating the bombs
            int bombs = 0;
            List<string> bombCheck = new List<string>();
            while (bombs != 10)
            {
                bombs = 0;
                Random rndRow = new Random();
                Random rndCollumn = new Random();
                int row = rndRow.Next(0, 8);
                int collumn = rndCollumn.Next(0, 8);

                mineButtons[collumn, row].CurrentState = "Bomb";


                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++) if (mineButtons[j, i].CurrentState == "Bomb") bombs++;

            }

            //debug code
            /*int bombcount = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++) if (mineButtons[j, i].CurrentState == "Bomb")
                    {
                        bombcount++;
                        mineButtons[j, i].Button.Text = "*";
                    }
            label1.Text = bombcount.ToString();*/

        }

        public void buttonClick(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;
            int collumn = 0, row = 0;

            //locating the button in the array
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (mineButtons[i, j].Button == currentButton)
                    {
                        collumn = i;
                        row = j;
                    }
                }
            }


            if (mineButtons[collumn, row].CurrentState == "Bomb")
            {

                MessageBox.Show("You hit a bomb" + Environment.NewLine + "Game over!");

            }

        }

    }
}
