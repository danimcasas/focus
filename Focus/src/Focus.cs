using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

[assembly: AssemblyTitle("Focus")]
[assembly: AssemblyProduct("Focus")]
[assembly: AssemblyDescription("Temporizador local de concentración, estudio y descanso")]

namespace FocusDesktop
{
    internal sealed class FocusForm : Form
    {
        private readonly string[] modeNames = { "Concentración", "Estudio", "Descanso" };
        private readonly int[] modeMinutes = { 25, 50, 10 };
        private readonly Button[] modeButtons = new Button[3];
        private readonly System.Windows.Forms.Timer clockTimer;
        private Label modeTitle;
        private Label clockLabel;
        private Label statusLabel;
        private TableLayoutPanel modePanel;
        private long remainingMilliseconds;
        private DateTime deadlineUtc;
        private int selectedMode;
        private bool isRunning;

        private static readonly Color BackgroundColor = Color.FromArgb(17, 26, 43);
        private static readonly Color CardColor = Color.FromArgb(25, 36, 58);
        private static readonly Color ControlColor = Color.FromArgb(32, 45, 70);
        private static readonly Color MainTextColor = Color.FromArgb(244, 246, 251);
        private static readonly Color MutedTextColor = Color.FromArgb(156, 169, 191);
        private static readonly Color AccentColor = Color.FromArgb(169, 240, 209);
        private static readonly Color AccentHoverColor = Color.FromArgb(109, 224, 178);

        public FocusForm()
        {
            Text = "Focus";
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(760, 730);
            MinimumSize = new Size(570, 620);
            BackColor = BackgroundColor;
            ForeColor = MainTextColor;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            AutoScaleMode = AutoScaleMode.Dpi;
            FormBorderStyle = FormBorderStyle.Sizable;

            BuildInterface();
            remainingMilliseconds = modeMinutes[selectedMode] * 60L * 1000L;
            clockTimer = new System.Windows.Forms.Timer();
            clockTimer.Interval = 100;
            clockTimer.Tick += OnClockTick;
            FormClosed += delegate { clockTimer.Stop(); clockTimer.Dispose(); };
            RenderState();
        }

        private void BuildInterface()
        {
            TableLayoutPanel shell = new TableLayoutPanel();
            shell.Dock = DockStyle.Fill;
            shell.ColumnCount = 1;
            shell.RowCount = 3;
            shell.Padding = new Padding(22);
            shell.BackColor = BackgroundColor;
            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            Controls.Add(shell);

            Label creatorFooter = new Label();
            creatorFooter.Name = "creatorFooter";
            creatorFooter.Dock = DockStyle.Fill;
            creatorFooter.TextAlign = ContentAlignment.MiddleCenter;
            creatorFooter.ForeColor = MutedTextColor;
            creatorFooter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            creatorFooter.Text = "danimcasas.com curso codex 2026" + Environment.NewLine
                + "Versión " + Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            creatorFooter.AccessibleName = "Creadores y versión de Focus";
            shell.Controls.Add(creatorFooter, 0, 2);

            Panel header = new Panel();
            header.Dock = DockStyle.Fill;
            header.BackColor = BackgroundColor;
            Label brand = new Label();
            brand.Dock = DockStyle.Left;
            brand.Width = 150;
            brand.Text = "◷  FOCUS";
            brand.TextAlign = ContentAlignment.MiddleLeft;
            brand.ForeColor = AccentColor;
            brand.Font = new Font("Segoe UI Semibold", 15F, FontStyle.Bold, GraphicsUnit.Point);
            brand.AccessibleName = "Focus";
            Label tagline = new Label();
            tagline.Dock = DockStyle.Fill;
            tagline.Text = "Tu tiempo, a tu ritmo";
            tagline.TextAlign = ContentAlignment.MiddleRight;
            tagline.ForeColor = MutedTextColor;
            tagline.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            header.Controls.Add(tagline);
            header.Controls.Add(brand);
            shell.Controls.Add(header, 0, 0);

            Panel card = new Panel();
            card.Dock = DockStyle.Fill;
            card.BackColor = CardColor;
            card.Padding = new Padding(28);
            card.Paint += delegate(object sender, PaintEventArgs e)
            {
                Rectangle border = card.ClientRectangle;
                border.Width -= 1;
                border.Height -= 1;
                ControlPaint.DrawBorder(e.Graphics, border, Color.FromArgb(49, 62, 84), ButtonBorderStyle.Solid);
            };
            shell.Controls.Add(card, 0, 1);

            TableLayoutPanel layout = new TableLayoutPanel();
            layout.Dock = DockStyle.Fill;
            layout.ColumnCount = 1;
            layout.RowCount = 5;
            layout.BackColor = CardColor;
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 78F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 66F));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            card.Controls.Add(layout);

            TableLayoutPanel titlePanel = new TableLayoutPanel();
            titlePanel.Dock = DockStyle.Fill;
            titlePanel.ColumnCount = 1;
            titlePanel.RowCount = 2;
            titlePanel.BackColor = CardColor;
            titlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 24F));
            titlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            Label eyebrow = new Label();
            eyebrow.Dock = DockStyle.Fill;
            eyebrow.Text = "UN PASO CADA VEZ";
            eyebrow.TextAlign = ContentAlignment.MiddleCenter;
            eyebrow.ForeColor = MutedTextColor;
            eyebrow.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold, GraphicsUnit.Point);
            modeTitle = new Label();
            modeTitle.Dock = DockStyle.Fill;
            modeTitle.TextAlign = ContentAlignment.MiddleCenter;
            modeTitle.ForeColor = MainTextColor;
            modeTitle.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point);
            modeTitle.AccessibleName = "Modo seleccionado";
            titlePanel.Controls.Add(eyebrow, 0, 0);
            titlePanel.Controls.Add(modeTitle, 0, 1);
            layout.Controls.Add(titlePanel, 0, 0);

            modePanel = new TableLayoutPanel();
            modePanel.Dock = DockStyle.Fill;
            modePanel.BackColor = CardColor;
            modePanel.Padding = new Padding(0, 5, 0, 5);
            modePanel.ColumnCount = 3;
            modePanel.RowCount = 1;
            modePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            modePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            modePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            string[] modeAccessibleNames = { "Modo Concentración, 25 minutos", "Modo Estudio, 50 minutos", "Modo Descanso, 10 minutos" };
            for (int i = 0; i < modeNames.Length; i++)
            {
                int modeIndex = i;
                Button button = CreateButton(modeNames[i], false);
                button.Name = "modeButton" + i;
                button.AccessibleName = modeAccessibleNames[i];
                button.Margin = new Padding(4, 0, 4, 0);
                button.Click += delegate { SelectMode(modeIndex); };
                modeButtons[i] = button;
                modePanel.Controls.Add(button, i, 0);
            }
            layout.Controls.Add(modePanel, 0, 1);

            TableLayoutPanel timerPanel = new TableLayoutPanel();
            timerPanel.Dock = DockStyle.Fill;
            timerPanel.ColumnCount = 1;
            timerPanel.RowCount = 2;
            timerPanel.BackColor = CardColor;
            timerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 78F));
            timerPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 22F));
            clockLabel = new Label();
            clockLabel.Dock = DockStyle.Fill;
            clockLabel.TextAlign = ContentAlignment.MiddleCenter;
            clockLabel.ForeColor = MainTextColor;
            clockLabel.Font = new Font("Segoe UI Light", 78F, FontStyle.Regular, GraphicsUnit.Point);
            clockLabel.UseCompatibleTextRendering = true;
            clockLabel.AccessibleName = "Tiempo restante";
            statusLabel = new Label();
            statusLabel.Dock = DockStyle.Fill;
            statusLabel.TextAlign = ContentAlignment.TopCenter;
            statusLabel.ForeColor = MutedTextColor;
            statusLabel.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            statusLabel.AccessibleName = "Estado del temporizador";
            timerPanel.Controls.Add(clockLabel, 0, 0);
            timerPanel.Controls.Add(statusLabel, 0, 1);
            layout.Controls.Add(timerPanel, 0, 2);
            timerPanel.Resize += delegate { ResizeClockFont(); };

            TableLayoutPanel actions = new TableLayoutPanel();
            actions.Dock = DockStyle.Fill;
            actions.ColumnCount = 3;
            actions.RowCount = 1;
            actions.BackColor = CardColor;
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.333F));
            actions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.334F));
            Button startButton = CreateButton("Iniciar", true);
            startButton.Name = "startButton";
            startButton.AccessibleName = "Iniciar o reanudar";
            startButton.Click += delegate { StartSession(); };
            Button pauseButton = CreateButton("Pausar", false);
            pauseButton.Name = "pauseButton";
            pauseButton.AccessibleName = "Pausar y conservar el tiempo restante";
            pauseButton.Click += delegate { PauseSession(); };
            Button resetButton = CreateButton("Reiniciar", false);
            resetButton.Name = "resetButton";
            resetButton.AccessibleName = "Reiniciar el modo seleccionado";
            resetButton.Click += delegate { ResetSession(); };
            actions.Controls.Add(startButton, 0, 0);
            actions.Controls.Add(pauseButton, 1, 0);
            actions.Controls.Add(resetButton, 2, 0);
            layout.Controls.Add(actions, 0, 3);
            this.startButton = startButton;
            this.pauseButton = pauseButton;

            Label footnote = new Label();
            footnote.Dock = DockStyle.Fill;
            footnote.Text = "Concentración · 25 min     Estudio · 50 min     Descanso · 10 min";
            footnote.TextAlign = ContentAlignment.MiddleCenter;
            footnote.ForeColor = MutedTextColor;
            footnote.Font = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
            layout.Controls.Add(footnote, 0, 4);
        }

        private Button startButton;
        private Button pauseButton;

        private Button CreateButton(string text, bool primary)
        {
            Button button = new Button();
            button.Text = text;
            button.Dock = DockStyle.Fill;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = primary ? AccentColor : Color.FromArgb(61, 76, 100);
            button.FlatAppearance.MouseOverBackColor = primary ? AccentHoverColor : Color.FromArgb(41, 57, 85);
            button.BackColor = primary ? AccentColor : ControlColor;
            button.ForeColor = primary ? Color.FromArgb(16, 36, 29) : MainTextColor;
            button.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold, GraphicsUnit.Point);
            button.Cursor = Cursors.Hand;
            button.Margin = new Padding(7, 7, 7, 7);
            return button;
        }

        private void ResizeClockFont()
        {
            if (clockLabel == null || clockLabel.ClientSize.Width <= 0) return;
            float size = Math.Max(44F, Math.Min(78F, clockLabel.ClientSize.Width / 7.8F));
            clockLabel.Font = new Font("Segoe UI Light", size, FontStyle.Regular, GraphicsUnit.Point);
        }

        private void SelectMode(int mode)
        {
            clockTimer.Stop();
            isRunning = false;
            selectedMode = mode;
            remainingMilliseconds = modeMinutes[selectedMode] * 60L * 1000L;
            UpdateModeButtons();
            RenderState();
        }

        private void StartSession()
        {
            if (isRunning || remainingMilliseconds <= 0) return;
            deadlineUtc = DateTime.UtcNow.AddMilliseconds(remainingMilliseconds);
            isRunning = true;
            clockTimer.Start();
            RenderState();
        }

        private void PauseSession()
        {
            if (!isRunning) return;
            remainingMilliseconds = Math.Max(0L, (long)(deadlineUtc - DateTime.UtcNow).TotalMilliseconds);
            clockTimer.Stop();
            isRunning = false;
            RenderState();
        }

        private void ResetSession()
        {
            clockTimer.Stop();
            isRunning = false;
            remainingMilliseconds = modeMinutes[selectedMode] * 60L * 1000L;
            RenderState();
        }

        private void OnClockTick(object sender, EventArgs e)
        {
            remainingMilliseconds = Math.Max(0L, (long)(deadlineUtc - DateTime.UtcNow).TotalMilliseconds);
            if (remainingMilliseconds == 0L)
            {
                clockTimer.Stop();
                isRunning = false;
            }
            RenderState();
        }

        private void UpdateModeButtons()
        {
            for (int i = 0; i < modeButtons.Length; i++)
            {
                bool active = i == selectedMode;
                modeButtons[i].BackColor = active ? Color.FromArgb(37, 64, 62) : ControlColor;
                modeButtons[i].ForeColor = active ? AccentColor : MutedTextColor;
                modeButtons[i].FlatAppearance.BorderColor = active ? Color.FromArgb(84, 136, 116) : Color.FromArgb(61, 76, 100);
            }
        }

        private void RenderState()
        {
            if (modeTitle == null || clockLabel == null || statusLabel == null) return;
            modeTitle.Text = modeNames[selectedMode];
            clockLabel.Text = FormatTime(remainingMilliseconds);
            if (remainingMilliseconds == 0L && !isRunning) statusLabel.Text = "Sesión completada";
            else if (isRunning) statusLabel.Text = "Sesión en marcha";
            else if (remainingMilliseconds < modeMinutes[selectedMode] * 60L * 1000L) statusLabel.Text = "Sesión en pausa";
            else statusLabel.Text = "Listo para empezar";
            if (startButton != null) startButton.Enabled = !isRunning && remainingMilliseconds > 0L;
            if (pauseButton != null) pauseButton.Enabled = isRunning;
            UpdateModeButtons();
        }

        private static string FormatTime(long milliseconds)
        {
            long seconds = (long)Math.Ceiling(Math.Max(0L, milliseconds) / 1000.0);
            long minutes = seconds / 60L;
            long remainder = seconds % 60L;
            return minutes.ToString("00") + ":" + remainder.ToString("00");
        }
    }

    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FocusForm());
        }
    }
}
