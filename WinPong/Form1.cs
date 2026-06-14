using static Utils;

namespace WinPong
{
    public partial class Form1 : Form
    {
        RectangleF player_racket = new(0, 0, 25, 100);
        RectangleF computer_racket = new(0, 0, 25, 100);

        RectangleF ball = new(0, 0, 20, 20);

        int computer_score = 0;
        int player_score = 0;

        bool move_down = false;
        bool move_up = false;

        float player_speed = 6;
        float computer_speed = 2;
        float ball_speed = 4;
        float ball_dir_x = 1;
        float ball_dir_y = 1;

        Font font = new("Arial", 12);

        int frames = 0;
        int monitor_hz = 1000;
        bool closed = false;

        public Form1()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            player_racket.X = player_racket.Width * 2;
            player_racket.Y = Size.Height / 2;

            computer_racket.X = ClientSize.Width - computer_racket.Width * 3;
            computer_racket.Y = Size.Height / 2;

            ball.X = ClientSize.Width / 2;
            ball.Y = ClientSize.Height / 2;
        }

        private void game_loop()
        {
            while (!closed)
            {
                frames++;
                if (frames / 1000 < 1000 / monitor_hz)
                    continue;

                frames = 0;
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
        }

        void reset_ball()
        {
            ball.X = ClientSize.Width / 2;
            ball.Y = ClientSize.Height / 2;

            ball_dir_x = -ball_dir_x;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(() => game_loop());
            DEVMODE mode = new DEVMODE();
            if (EnumDisplaySettings(null, -1, ref mode))
                monitor_hz = mode.dmDisplayFrequency;

            ball_speed *= 1f / (monitor_hz * 4);
            computer_speed *= 1f / (monitor_hz * 4);
            player_speed *= 1f / (monitor_hz * 4);

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.White, player_racket);
            g.FillRectangle(Brushes.White, computer_racket);
            g.FillEllipse(Brushes.White, ball);

            Size computer_score_size = TextRenderer.MeasureText($"Computer: {computer_score}", DefaultFont);
            e.Graphics.DrawString($"Player: {player_score}", font, Brushes.Red, new PointF(ClientSize.Width * 0.25f, ClientSize.Height * 0.05f));
            e.Graphics.DrawString($"Computer: {computer_score}", font, Brushes.Red, new PointF(ClientSize.Width * 0.75f - computer_score_size.Width, ClientSize.Height * 0.05f));

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) move_up = true;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) move_down = true;

            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                player_speed *= 1.1f;
                computer_speed *= 1.1f;
            }

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                player_speed /= 1.1f;
                computer_speed /= 1.1f;
            }

            if (e.KeyCode == Keys.R)
            {
                player_speed = 6f / (monitor_hz * 4);
                computer_speed = 2f / (monitor_hz * 4);
                ball_speed = 4f / (monitor_hz * 4);
            }

            if (e.KeyCode == Keys.Oemplus)
                ball_speed *= 1.1f;

            if (e.KeyCode == Keys.OemMinus)
                ball_speed /= 1.1f;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.W) move_up = false;
            if (e.KeyCode == Keys.Down || e.KeyCode == Keys.S) move_down = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            closed = true;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            player_racket.X = player_racket.Width * 2;
            computer_racket.X = ClientSize.Width - computer_racket.Width * 3;

            computer_racket.Y = Math.Clamp(computer_racket.Y, 0, Math.Abs(ClientSize.Height - computer_racket.Height));
            player_racket.Y = Math.Clamp(player_racket.Y, 0, Math.Abs(ClientSize.Height - player_racket.Height));
        }
    }
}
