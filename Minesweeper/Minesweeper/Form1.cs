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
                    for (int j = 0; j < 9; j++)
                        if (mineButtons[j, i].CurrentState == "Bomb") bombs++;

            }

            //checking if a space is next to a bomb
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (mineButtons[j, i].CurrentState == "Bomb")
                    {

                        for (int ti = -1; ti <= 1; ti++)
                            for (int tj = -1; tj <= 1; tj++)
                            {
                                if ((j + tj >= 0) && (j + tj <= 8) && (i + ti >= 0) && (i + ti <= 8))
                                    if (mineButtons[j + tj, i + ti].CurrentState != "Bomb")
                                    {
                                        mineButtons[j + tj, i + ti].AdjacentBombs++;
                                        mineButtons[j + tj, i + ti].CurrentState = "NextToBomb";
                                    }
                            }
                    }

                }

            //debug code
           /* int bombcount = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++) if (mineButtons[j, i].CurrentState == "Bomb")
                    {
                        bombcount++;
                        mineButtons[j, i].Button.Text = "*";
                    }
            label1.Text = bombcount.ToString();
           */
        }


        public void CheckAdjacentSpaces(int collumn, int row)
        {
            List<int> adjacentCollumn = new List<int>();
            List<int> adjacentRow = new List<int>();

            for (int ti = -1; ti <= 1; ti++)
                for (int tj = -1; tj <= 1; tj++)
                {
                    if ((collumn + tj >= 0) && (collumn + tj <= 8) && (row + ti >= 0) && (row + ti <= 8))
                        if (mineButtons[collumn + tj, row + ti].CurrentState == "Empty" && mineButtons[collumn+tj,row+ti].IsRevealed==false)
                        {
                            mineButtons[collumn+tj, row+ti].Button.BackColor = Color.Gray;
                            mineButtons[collumn+tj, row+ti].IsRevealed = true;

                            adjacentCollumn.Add(collumn + tj);
                            adjacentRow.Add(row + ti);
                        }
                        else if (mineButtons[collumn + tj, row + ti].CurrentState == "NextToBomb" && mineButtons[collumn + tj, row + ti].IsRevealed == false)
                        {
                            if (mineButtons[collumn+tj, row+ti].IsRevealed == false)
                            {
                                mineButtons[collumn + tj, row + ti].Button.Text = mineButtons[collumn+tj, row+ti].AdjacentBombs.ToString();
                                mineButtons[collumn + tj, row + ti].IsRevealed = true;
                            }
                        }
                }

             for(int i=0;i<adjacentCollumn.Count;i++)
            {
                CheckAdjacentSpaces(adjacentCollumn[i], adjacentRow[i]);
            }
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
                currentButton.Text = "*";
                MessageBox.Show("You hit a bomb" + Environment.NewLine + "Game over!");
                Application.Exit();

            }
            else if (mineButtons[collumn, row].CurrentState == "NextToBomb")
            {
                if (mineButtons[collumn, row].IsRevealed == false)
                {
                    currentButton.Text = mineButtons[collumn, row].AdjacentBombs.ToString();
                    mineButtons[collumn, row].IsRevealed = true;
                }
            }
            else if (mineButtons[collumn, row].CurrentState == "Empty")
            {
                if (mineButtons[collumn, row].IsRevealed == false)
                {
                    currentButton.BackColor = Color.Gray;
                    mineButtons[collumn, row].IsRevealed = true;
                    CheckAdjacentSpaces(collumn, row);
                }

            }


        }

    }
}
