using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class Form3 : Form
    {
        private int m_playerScore = 0;
        private int m_computerScore = 0;
        private int m_switchTurnsInDraw = 0;
        private PictureBox[] gameSlots;
        private Winner m_winner = Winner.None;
        private PlayerTurn m_playerTurn;

        private enum PlayerTurn
        {
            None,
            Player,
            Computer
        };

        private enum Winner
        {
            None,
            Player,
            Computer,
            Draw
        }


        public Form3()
        {
            InitializeComponent();
            //set the pictures array containing the images of all the slots of the game.
            gameSlots = new PictureBox[]
            {
                 pictureBox0, pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5,
                pictureBox6, pictureBox7, pictureBox8
            };

        }

        private void Form1_Load(object sender, EventArgs e)
        { 
            OnNewGame();
        }

        private void ComputerPlay()
        {
            bool isMarked = false;


            //mark the center as the first move
            if (!PositionCenter(isMarked))
            {
                //check wether you can win or need to block to prevent player from winning
                if (!BlockOrWin(isMarked))
                {
                    //get the first corner availible marked
                    if (!PositionCorners(isMarked))
                    {
                        //if all previous failed, find the first avaliable slot and mark it.
                        if (!FindFreeSlots(isMarked))
                            return;
                    }
                }
            }



        }

        private bool FindFreeSlots(bool isMarked)
        {
            foreach (PictureBox pic in gameSlots)
            {
                if (pic.Image == null)
                {
                    pic.Image = computerPlayer.Image;
                    return true;
                }
            }
            return isMarked;
        }

        //check wether two slots are taken either by computer or by player, and block it or win the game by marking in the third slot
        private bool BlockOrWin(bool isMarked)
        {
            PictureBox[] slots =
            {
                //horizontal checks
                pictureBox0, pictureBox1, pictureBox2,
                pictureBox3, pictureBox4, pictureBox5,
                pictureBox6, pictureBox7, pictureBox8,
                pictureBox0, pictureBox2, pictureBox1,
                pictureBox3, pictureBox5, pictureBox4,
                pictureBox6, pictureBox8, pictureBox7,

                pictureBox2,pictureBox1,pictureBox0,
                pictureBox4,pictureBox5,pictureBox3,
                pictureBox7,pictureBox8,pictureBox6,
                //vertical checks
                pictureBox0, pictureBox3, pictureBox6,
                pictureBox1, pictureBox4, pictureBox7,
                pictureBox2, pictureBox5, pictureBox8,

                pictureBox0, pictureBox6, pictureBox3,
                pictureBox1, pictureBox7, pictureBox4,
                pictureBox2, pictureBox8, pictureBox5,

                pictureBox3,pictureBox6,pictureBox0,
                pictureBox4,pictureBox7,pictureBox1,
                pictureBox5,pictureBox8,pictureBox2,
                //diagonal checks
                pictureBox0, pictureBox4, pictureBox8,
                pictureBox2, pictureBox4, pictureBox6,
                pictureBox0, pictureBox8, pictureBox4,
                pictureBox2, pictureBox6, pictureBox4,
                pictureBox4,pictureBox8,pictureBox0,
                pictureBox4,pictureBox6,pictureBox2


            };


            for (int i = 0; i < slots.Length; i+=3)
            {
                if((slots[i].Image == player1.Image && slots[i+1].Image == player1.Image && slots[i+2].Image == null ) ||
                  (slots[i].Image == computerPlayer.Image && slots[i+1].Image == computerPlayer.Image && slots[i+2].Image == null))
                {
                    slots[i + 2].Image = computerPlayer.Image;
                    return true;
                }
            }

            return isMarked;
        }

        private bool PositionCorners(bool isMarked)
        {
            //get the corners and mark the first one which is not marked.
            PictureBox[] corners = {pictureBox0, pictureBox2, pictureBox6, pictureBox8};

            foreach (PictureBox corner in corners)
            {
                if (corner.Image == null)
                {
                    corner.Image = computerPlayer.Image;
                    return true;
                }
            }
            return isMarked;
        }

        //mark the center of the board if it is not already marked.
        private bool PositionCenter(bool isMarked)
        {
            
            int startSlot = 0;
            int endSlot = gameSlots.Length;
            int middleSlot = (startSlot + endSlot) / 2;

            if (gameSlots[middleSlot].Image == null)
            {
                gameSlots[middleSlot].Image = computerPlayer.Image;
                isMarked = true;
            }
            return isMarked;
        }

        private void TurnsManaging(Winner winner)
        {
            //switching players every turn
            if(winner == Winner.None)
            {
                m_playerTurn = m_playerTurn == PlayerTurn.Player ? PlayerTurn.Computer : PlayerTurn.Player;
            }
            else
            {
                m_playerTurn = PlayerTurn.None;
            }
                
        }


        private void OnClick(object sender, EventArgs e)
        {
            PictureBox picture = sender as PictureBox;

            //setting player mode to none to prevent the game keep working after the game is over.
            if (m_playerTurn == PlayerTurn.None)
                return;

            // making sure that the image won't change once its clicked
            if (picture.Image != null)
                return;

            if (m_playerTurn == PlayerTurn.Player)
                picture.Image = player1.Image;
             
             m_winner = CheckWinner();            
             ShowWinner(m_winner);
             ShowTurn();
             TurnsManaging(m_winner);

            //once the player finishes his turn, the event call immidiatly to the AI computer method to do its move.
            if (m_playerTurn == PlayerTurn.Computer && m_winner == Winner.None)
            {
                
                ComputerPlay();
                m_winner = CheckWinner();
                ShowWinner(m_winner);
                TurnsManaging(m_winner);
                ShowTurn();
            }
        }

        private void ShowWinner(Winner winner)
        {
            if (winner == Winner.Player)
            {
                m_playerScore++;
                MessageBox.Show("You Won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else if (winner == Winner.Computer)
            {
                    m_computerScore++;
                    MessageBox.Show("Computer won!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                
            }
            else if (winner == Winner.Draw)
            {
                MessageBox.Show("It's a draw!", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
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
                            return Winner.Player; 

                        return Winner.Computer;
                    }
                }
              
            }

            foreach(PictureBox pic in gameSlots)
                if (pic.Image == null)
                    return Winner.None;

            return Winner.Draw;
        }

     
        private void OnNewGame()
        {
            //setting the slots to empty 
            foreach (PictureBox picture in gameSlots)
            {
                picture.Image = null;
                
            }
            //displaying the players score
            lblScorep1.Text += m_playerScore.ToString();
            lblScorep2.Text += m_computerScore.ToString();

            NewGameTurns();
            //Set m_winner to none every new game
            m_winner = Winner.None;
            if (m_playerTurn == PlayerTurn.Computer)
            {
                ComputerPlay();
                TurnsManaging(m_winner);
            }
                
            ShowTurn();
        }


        private void NewGameTurns()
        {
            //make sure the opposite player who won the game gets to start playing.if its Draw then switch between the two.
            if(m_winner == Winner.Player)
                m_playerTurn = PlayerTurn.Computer;
            else if(m_winner == Winner.Computer || m_winner == Winner.None)
                m_playerTurn = PlayerTurn.Player; 
            else if (m_winner == Winner.Draw)
            {
                m_switchTurnsInDraw = m_switchTurnsInDraw == 0 ? 1 : 0;
                m_playerTurn = m_switchTurnsInDraw == 0 ? PlayerTurn.Computer : PlayerTurn.Player;
            } 
        }


        private void ShowTurn()
        {
            string status = "";
            status = DisplayTurn(m_winner);

            lblScorep1.Text = "Score: " + m_playerScore.ToString();
            lblScorep2.Text = "Score: " + m_computerScore.ToString();
            lblStatus.Text = status;
        }


        private string DisplayTurn(Winner winner)
        {
            string status = "";
            switch (winner)
            {
                case Winner.None:

                    if (m_playerTurn == PlayerTurn.Player)
                        status = "Turn: Player ";
                    else
                    {
                        status = "Turn: Computer ";
                    }
                    break;
                case Winner.Player:
                    status = "You won!";

                    break;
                case Winner.Computer:
                    status = "Computer Won!";

                    break;
                case Winner.Draw:
                    status = "It's a draw!";
                    break;
            }

            return status;
        }


        private void btnNewGame_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Would you like to start a new game?","New Game", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if(result == DialogResult.Yes)
                OnNewGame();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to quit Tic Tac Toe?", "Quit", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.No)
                e.Cancel = true;
            

        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            //set the scores to 0 and update the scores text in the client.
            m_playerScore = 0;
            m_computerScore = 0;
            lblScorep1.Text = "Score: " + m_playerScore.ToString();
            lblScorep2.Text = "Score: " + m_computerScore.ToString();
        }

        private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(@"   Tic Tac Toe desktop game   
    created by Raz in 2017", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
