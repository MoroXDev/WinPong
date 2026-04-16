using System.Drawing.Drawing2D;

namespace WinPong
{
    public partial class Form1 : Form
    {
        RectangleF player_racket = new (0, 0, 50, 200);
        RectangleF computer_racket = new (0, 0, 50, 200);

        int ball_x = 200;
        int ball_y = 150;

        int computer_score = 0;
        int player_score = 0;

        bool move_down = false;
        bool move_up = false;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (move_down)
            {

            }
            if (move_up)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            player_racket.X = Size.Width * 0.1f;
            player_racket.Y = Size.Height / 2;

            computer_racket.X = Size.Width * 0.9f - computer_racket.Width;
            computer_racket.Y = Size.Height / 2;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, player_racket);
            e.Graphics.FillRectangle(Brushes.White, computer_racket);
            //e.Graphics.DrawEllipse();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) move_up = true;
            if (e.KeyCode == Keys.Down) move_down = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) move_up = false;
            if (e.KeyCode == Keys.Down) move_down = false;
        }
    }
}
