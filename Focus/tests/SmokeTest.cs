using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace FocusDesktop
{
    internal static class SmokeTest
    {
        [STAThread]
        private static int Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (FocusForm form = new FocusForm())
            {
                try
                {
                    form.ShowInTaskbar = false;
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(-30000, -30000);
                    form.Opacity = 0;
                    form.Show();
                    Application.DoEvents();
                    form.PerformLayout();
                    Control[] footer = form.Controls.Find("creatorFooter", true);
                    string version = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
                    Check(footer.Length == 1, "No se encontró el pie de la app.");
                    Check(footer[0].Text == "danimcasas.com curso codex 2026" + Environment.NewLine + "Versión " + version, "Crédito o versión incorrectos.");
                    Check(!footer[0].Bounds.IsEmpty && footer[0].Height >= 40, "El pie no tiene espacio visible.");
                    CheckModeLayout(form);
                    if (args.Length > 0)
                    {
                        Render(form, Path.Combine(args[0], "Focus-normal.png"));
                        form.Size = form.MinimumSize;
                        form.PerformLayout();
                        Check(footer[0].Bottom <= footer[0].Parent.ClientSize.Height, "El pie queda fuera de la ventana mínima.");
                        CheckModeLayout(form);
                        Render(form, Path.Combine(args[0], "Focus-minimo.png"));
                    }

                    int[] minutes = { 25, 50, 10 };
                    for (int i = 0; i < minutes.Length; i++)
                    {
                        Invoke(form, "SelectMode", i);
                        Check(Text(form, "clockLabel") == minutes[i].ToString("00") + ":00", "Duración de modo incorrecta.");
                    }
                    Invoke(form, "SelectMode", 0);
                    Invoke(form, "StartSession");
                    Pump(1200);
                    Invoke(form, "PauseSession");
                    string paused = Text(form, "clockLabel");
                    Check(paused != "25:00", "El reloj no avanzó.");
                    Check(Text(form, "statusLabel") == "Sesión en pausa", "Estado de pausa incorrecto.");
                    Check(((Button)Field(form, "startButton")).Enabled, "No se permite reanudar.");
                    Check(!((Button)Field(form, "pauseButton")).Enabled, "Pausar sigue activado.");
                    Pump(1200);
                    Check(Text(form, "clockLabel") == paused, "El tiempo cambió en pausa.");
                    Invoke(form, "StartSession");
                    Check(Text(form, "statusLabel") == "Sesión en marcha", "No reanuda.");
                    Invoke(form, "ResetSession");
                    Check(Text(form, "clockLabel") == "25:00", "Reinicio incorrecto.");
                    Check(Text(form, "statusLabel") == "Listo para empezar", "Estado de reinicio incorrecto.");
                    Invoke(form, "StartSession");
                    Invoke(form, "SelectMode", 2);
                    Check(Text(form, "clockLabel") == "10:00" && !((Button)Field(form, "pauseButton")).Enabled, "Cambiar de modo no detuvo la sesión.");
                    Invoke(form, "StartSession");
                    typeof(FocusForm).GetField("deadlineUtc", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(form, DateTime.UtcNow.AddMilliseconds(-1));
                    Invoke(form, "OnClockTick", null, EventArgs.Empty);
                    Check(Text(form, "clockLabel") == "00:00" && Text(form, "statusLabel") == "Sesión completada", "Finalización incorrecta.");
                    Check(!((Button)Field(form, "startButton")).Enabled, "Iniciar sigue activado tras finalizar.");
                    Console.WriteLine("PASS: crédito, versión " + version + ", pie a tamaño normal y mínimo; modos, cuenta real, pausa estable, reanudación, reinicio, cambio de modo y finalización.");
                    return 0;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex);
                    return 1;
                }
            }
        }

        private static void Render(Form form, string path)
        {
            using (Bitmap bitmap = new Bitmap(form.Width, form.Height))
            {
                form.DrawToBitmap(bitmap, new Rectangle(Point.Empty, form.Size));
                bitmap.Save(path, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private static void Pump(int milliseconds)
        {
            DateTime until = DateTime.UtcNow.AddMilliseconds(milliseconds);
            while (DateTime.UtcNow < until) { Application.DoEvents(); Thread.Sleep(10); }
        }

        private static void CheckModeLayout(FocusForm form)
        {
            Button[] buttons = (Button[])Field(form, "modeButtons");
            for (int i = 0; i < buttons.Length; i++)
            {
                Check(buttons[i].Visible && buttons[i].Width > 0, "Hay un modo sin control visible.");
                for (int j = i + 1; j < buttons.Length; j++)
                    Check(!buttons[i].Bounds.IntersectsWith(buttons[j].Bounds), "Los botones de modo se solapan.");
            }
        }

        private static object Field(FocusForm form, string name)
        {
            return typeof(FocusForm).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance).GetValue(form);
        }

        private static string Text(FocusForm form, string name) { return ((Label)Field(form, name)).Text; }

        private static void Invoke(FocusForm form, string name, params object[] args)
        {
            typeof(FocusForm).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Instance).Invoke(form, args);
        }

        private static void Check(bool condition, string message) { if (!condition) throw new Exception(message); }
    }
}
