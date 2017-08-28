using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form2 : Form
    {
        private int player1Score;
        private int player2Score;
        private PictureBox[] gameSlots;
        private Winner winner;
        private PlayerTurn playerTurn;

        private enum PlayerTurn
        {
            None,
            Player1,
            Player2
        };

        private enum Winner
        {
            None,
            Player1,
            Player2,
            Draw
        }

        public Form2()
        {
            InitializeComponent();
            gameSlots = new PictureBox[9]
             {
                 pictureBox0, pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5,
                pictureBox6, pictureBox7, pictureBox8
             };
        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            OnNewGame();
        }

        

        private void OnClick(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;

            if (playerTurn == PlayerTurn.None)
                return;

            // making sure that the image won't change once its clicked
            if (picture.Image != null)
                return;

            if (playerTurn == PlayerTurn.Player1)
                picture.Image = player1.Image;
            else
                picture.Image = player2.Image;

            //checking if there is a winner
            winner = CheckWinner();
            //updating the turns switching
            TurnsManaging(winner);
            //displaying the winner on a messageBox if there is one
            ShowWinner(winner);
            //showing the turn in the label
            ShowTurn();
        }


        private void TurnsManaging(Winner winner)
        {
            //switching players every turn
            if (winner == Winner.None)
            {
                playerTurn = playerTurn == PlayerTurn.Player1 ? PlayerTurn.Player2 : PlayerTurn.Player1;
            }
            else
            {
                playerTurn = PlayerTurn.None;
            }

        }

        private Winner CheckWinner()
        {
            
            PictureBox[] winnerSlots =
            {
                //checking rows
                pictureBox0, pictureBox1, pictureBox2,
                pictureBox3, pictureBox4, pictureBox5,
                pictureBox6, pictureBox7, pictureBox8,
                //checking cols
                pictureBox0, pictureBox3, pictureBox6,
                pictureBox1, pictureBox4, pictureBox7,
                pictureBox2, pictureBox5, pictureBox8,
                //checking diagonals
                pictureBox0, pictureBox4, pictureBox8,
                pictureBox2, pictureBox4, pictureBox6

            };

            for (int i = 0; i < winnerSlots.Length; i+=3)
            {
                if (winnerSlots[i].Image != null)
                {
                    if (winnerSlots[i].Image == winnerSlots[i+1].Image && winnerSlots[i].Image == winnerSlots[i+2].Image)
                    {
                        if (winnerSlots[i].Image == player1.Image)
                            return Winner.Player1; 

                        return Winner.Player2;
                    }
                }
              
            }


            foreach(PictureBox slot in gameSlots)
                if (slot.Image == null)
                    return Winner.None;

            return Winner.Draw;

        }

        private void ShowWinner(Winner winner)
        {
            if (winner == Winner.Player1)
            {
                player1Score++;
                MessageBox.Show("Player 1 Won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (winner == Winner.Player2)
            {
                player2Score++;
                MessageBox.Show("Player 2 Won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

            }
            else if (winner == Winner.Draw)
            {
                MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void OnNewGame()
        {
            //resetting the slots of the game
            foreach (PictureBox slot in gameSlots)
            {
                slot.Image = null;
                
            }           
            lblScorep1.Text += player1Score.ToString();
            lblScorep2.Text += player2Score.ToString();

            playerTurn = winner == Winner.Player1 ? PlayerTurn.Player2 : PlayerTurn.Player1; 
            winner = Winner.None;
            ShowTurn();
        }

        

        private void ShowTurn()
        {
            string status = "";
            status = DisplayTurn(winner);

            lblScorep1.Text = "Score: " + player1Score.ToString();
            lblScorep2.Text = "Score: " + player2Score.ToString();
            lblStatus.Text = status;
        }


        private string DisplayTurn(Winner winner)
        {
            string status ="";
            switch (winner)
            {
                case Winner.None:

                    if (playerTurn == PlayerTurn.Player1)
                        status = "Turn: Player 1";
                    else
                    {
                        status = "Turn: Player2";
                    }
                    break;
                case Winner.Player1:
                    status = "Player 1 Won";

                    break;
                case Winner.Player2:
                    status = "Player 2 Won";

                    break;
                case Winner.Draw:
                    status = "It's a draw!";
                    break;
            }

            return status;
        }
        //starting a new game event
        private void btnNewGame_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Would you like to start a new game?","New Game", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
                OnNewGame();
        }

        //closing the form notice
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit Tic Tac Toe?", "Quit", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                e.Cancel = true;
        }

        //reseting the players scores
        private void resetBtn_Click(object sender, EventArgs e)
        {
            player1Score = 0;
            player2Score = 0;
            lblScorep1.Text = "Score: " + player1Score.ToString();
            lblScorep2.Text = "Score: " + player2Score.ToString();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"   Tic Tac Toe desktop game   
    created by Raz in 2017", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
