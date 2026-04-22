using System.Drawing.Drawing2D;

namespace WinPong
{
    public partial class Form1 : Form
    {
        RectangleF player_racket = new (0, 0, 25, 100);
        RectangleF computer_racket = new (0, 0, 25, 100);

        RectangleF ball = new(0, 0, 20, 20);

        int computer_score = 0;
        int player_score = 0;

        bool move_down = false;
        bool move_up = false;

        float player_speed = 12;
        float computer_speed = 4;
        float ball_speed = 8;
        float ball_dir_x = 1;
        float ball_dir_y = 1;

        Font font = new("Arial", 12);

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            player_racket.X = Size.Width * 0.05f;
            player_racket.Y = Size.Height / 2;

            computer_racket.X = Size.Width * 0.95f - computer_racket.Width;
            computer_racket.Y = Size.Height / 2;

            ball.X = ClientSize.Width / 2;
            ball.Y = ClientSize.Height / 2;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //player_racket.Y = 0;

            if (move_down && player_racket.Y < ClientSize.Height - player_racket.Height)
            {
                player_racket.Y += player_speed;
            }
            if (move_up && player_racket.Y > 0)
            {
                player_racket.Y -= player_speed;
            }

            if (ball.IntersectsWith(player_racket))
            {
                ball_dir_x = Math.Abs(ball_dir_x);
            }
            if (ball.IntersectsWith(computer_racket))
            {
                ball_dir_x = -Math.Abs(ball_dir_x);
            }

            if (ball.X < 0)
            {
                computer_score++;
                reset_ball();
            }

            if (ball.X > ClientSize.Width - ball.Width)
            {
                player_score++;
                reset_ball();
            }

            if (ball.Y < 0)
            {   
                ball_dir_y = Math.Abs(ball_dir_y);
            }
            if (ball.Y > ClientSize.Height - ball.Height)
            { 
                ball_dir_y = -Math.Abs(ball_dir_y);
            }


            ball.X += ball_speed * ball_dir_x;
            ball.Y += ball_speed * ball_dir_y;

            if (computer_racket.Y > ball.Y)
                computer_racket.Y -= computer_speed;
            if (computer_racket.Y < ball.Y)
                computer_racket.Y += computer_speed;


            Invalidate();
        }

        void reset_ball()
        {
            ball.X = ClientSize.Width / 2;
            ball.Y = ClientSize.Height / 2;

            ball_dir_x = -ball_dir_x;
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        protected void Form1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, player_racket);
            g.FillRectangle(Brushes.White, computer_racket);
            g.FillEllipse(Brushes.White, ball);

            e.Graphics.DrawString($"Gracz: {player_score}", font, Brushes.Red, new PointF(ClientSize.Width * 0.25f, ClientSize.Height * 0.05f));
            e.Graphics.DrawString($"Komputer: {computer_score}", font, Brushes.Red, new PointF(ClientSize.Height * 0.9f, ClientSize.Height * 0.05f));
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
