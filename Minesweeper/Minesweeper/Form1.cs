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
        int flagsBomb = 0, placedFlags = 0;

        private void Form1_Load(object sender, EventArgs e)
        {

            //Adding the buttons
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    mineButtons[j, i] = new MineButton(new Button());
                    mineButtons[j, i].Button.MouseUp += ButtonClick;
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

                mineButtons[collumn, row].CurrentState = EnumStates.Bomb;


                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        if (mineButtons[j, i].CurrentState == EnumStates.Bomb) bombs++;

            }

            //checking if a space is next to a bomb
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    if (mineButtons[j, i].CurrentState == EnumStates.Bomb)
                    {

                        for (int ti = -1; ti <= 1; ti++)
                            for (int tj = -1; tj <= 1; tj++)
                            {
                                if ((j + tj >= 0) && (j + tj <= 8) && (i + ti >= 0) && (i + ti <= 8))
                                    if (mineButtons[j + tj, i + ti].CurrentState != EnumStates.Bomb)
                                    {
                                        mineButtons[j + tj, i + ti].AdjacentBombs++;
                                        mineButtons[j + tj, i + ti].CurrentState = EnumStates.NextToBomb;
                                    }
                            }
                    }

                }

            //debug code
           /* int bombcount = 0;
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++) if (mineButtons[j, i].CurrentState == EnumStates.Bomb)
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
                        if (mineButtons[collumn + tj, row + ti].CurrentState == EnumStates.Empty && mineButtons[collumn + tj, row + ti].IsRevealed == false)
                        {
                            mineButtons[collumn + tj, row + ti].Button.BackColor = Color.Gray;
                            mineButtons[collumn + tj, row + ti].IsRevealed = true;

                            adjacentCollumn.Add(collumn + tj);
                            adjacentRow.Add(row + ti);
                        }
                        else if (mineButtons[collumn + tj, row + ti].CurrentState == EnumStates.NextToBomb && mineButtons[collumn + tj, row + ti].IsRevealed == false)
                        {
                            if (mineButtons[collumn + tj, row + ti].IsRevealed == false)
                            {
                                mineButtons[collumn + tj, row + ti].Button.Text = mineButtons[collumn + tj, row + ti].AdjacentBombs.ToString();
                                mineButtons[collumn + tj, row + ti].IsRevealed = true;
                            }
                        }
                }

            for (int i = 0; i < adjacentCollumn.Count; i++)
            {
                CheckAdjacentSpaces(adjacentCollumn[i], adjacentRow[i]);
            }
        }


        public void ButtonClick(object sender, MouseEventArgs e)
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

            //if we click with the right button
            if (e.Button == MouseButtons.Right)
            {


                //checks to see if a spot can have a flag placed
                if (placedFlags != 10)
                {
                    if (mineButtons[collumn, row].IsRevealed == false && mineButtons[collumn, row].IsFlagged == false)
                    {
                        mineButtons[collumn, row].Button.Text = "F";
                        mineButtons[collumn, row].IsFlagged = true;
                        placedFlags++;
                        if (mineButtons[collumn, row].CurrentState == EnumStates.Bomb) flagsBomb++;
                    }

                    else if (mineButtons[collumn, row].IsRevealed == false && mineButtons[collumn, row].IsFlagged == true)
                    {
                        mineButtons[collumn, row].Button.Text = "";
                        mineButtons[collumn, row].IsFlagged = false;
                        placedFlags--;
                    }
                }

                //checks win condition
                if (flagsBomb == 10)
                {
                    MessageBox.Show("You won!!");
                    Application.Exit();
                }


                label1.Text = "Number of flags placed: " + placedFlags;

            }

            //if we click with the left button
            else if (e.Button == MouseButtons.Left && mineButtons[collumn, row].IsFlagged == false)
            {
                //losing state
                if (mineButtons[collumn, row].CurrentState == EnumStates.Bomb)
                {
                    for (int i = 0; i < 9; i++)
                        for (int j = 0; j < 9; j++)
                            if (mineButtons[j, i].CurrentState == EnumStates.Bomb)
                                mineButtons[j, i].Button.Text = "*";

                    MessageBox.Show("You hit a bomb" + Environment.NewLine + "Game over!");
                    Application.Exit();

                }

                else if (mineButtons[collumn, row].CurrentState == EnumStates.NextToBomb)
                {
                    if (mineButtons[collumn, row].IsRevealed == false)
                    {
                        currentButton.Text = mineButtons[collumn, row].AdjacentBombs.ToString();
                        mineButtons[collumn, row].IsRevealed = true;
                    }
                }

                else if (mineButtons[collumn, row].CurrentState == EnumStates.Empty)
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
}
