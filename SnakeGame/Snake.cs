using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SnakeGame
{
    public partial class Snake : Form, View
    {
        protected SnakeGameModel sgm = null;
        protected SnakeGameView sgv = null;
        protected SnakeGameController sgc = null;

        protected static Snake _instance = null;

        public bool xMode = false;

        public static void Debug(string str)
        {
            if (_instance.chbDebug.Checked)
            {
                Console.Write(str);
                System.Diagnostics.Debug.WriteLine(str);
            }
        }

        public Snake()
        {
            //Singletron here
            if(_instance == null)
            {
                InitializeComponent();
                Snake._instance = this;
            }
        }

        public void DisposeGame()
        {
            sgm = null;
            sgv.Dispose();
            sgv = null;
            sgv = null;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if(sgv != null)
            {
                DisposeGame();
            }
            try
            {
                Snake.Debug("create view");
                if(xMode) sgv = new SnakeGameView(2*40, 2*40);
                else sgv = new SnakeGameView(40, 40);
                Snake.Debug("create model");
                if (xMode) sgm = new SnakeGameModel(2*40, 2*40);
                else sgm = new SnakeGameModel(40, 40);
                Snake.Debug("create controller");
                sgc = new SnakeGameController();
                Snake.Debug("attach model");
                sgc.AddModel(sgm);
                Snake.Debug("attach view");
                sgm.AttachObserver(sgv);
                sgm.AttachObserver(this);
                Snake.Debug("set controller");
                sgv.setController(sgc);
                Snake.Debug("Start the controller");
                sgc.Start();
                sgv.Run();
            } catch
            {
                Snake.Debug("Error starting game");
            }
        }

        public void setController(Controller c)
        {
            return;
        }

        delegate void SetTextCallback(string text);
        private void SetScore(string score)
        {
            if(this.lblScore.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetScore);
                this.Invoke(d, new object[] { score });
            } else
            {
                this.lblScore.Text = score;
            }
        }

        public void Notify(Model m)
        {
            if(m is SnakeGameModel)
            {
                SnakeGameModel sbm = m as SnakeGameModel;
                SetScore((sbm.SnakeLength() - SnakeGameModel.SNAKE_INIT_SIZE).ToString());
            }
        }

        private void Snake_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void Snake_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(sgv != null)
            {
                sgv.Dispose();
                sgv.Exit();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!xMode)
            {
                xMode = true;
                button1.BackColor = Color.DarkRed;
            }
            else
            {
                xMode = false;
                button1.BackColor = Color.White;
            }
        }
    }
}
