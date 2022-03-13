using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BatteryNotification
{
    static class Program
    {
        public static readonly ContextMenu menu = new ContextMenu();
        public static readonly MenuItem mnuExit = new MenuItem("Exit");
        public static readonly Timer timer = new Timer()
        {
            Interval = 60000,
            Enabled = true
        };
        public static readonly NotifyIcon notifyIcon = new NotifyIcon()
        {
            Icon = Properties.Resources.wpdshext_713,
            ContextMenu = menu,
            Text = "Main"
        };

        [STAThread]
        static void Main()
        {
            menu.MenuItems.Add(mnuExit);
            mnuExit.Click += new EventHandler(mnuExit_Click);
            notifyIcon.Visible = true;
            timer.Tick += Timer_Tick;
            Application.Run();
        }

        private static void Timer_Tick(object sender, EventArgs e)
        {            
            BatteryInformation cap = BatteryInfo.GetBatteryInformation();

            if (!cap.PowerState.HasFlag(Win32.POWER_STATE.BATTERY_CHARGING))
                return;

            var percent = (float)cap.CurrentCapacity / cap.FullChargeCapacity;
            if (percent >= 0.8f)
                notifyIcon.ShowBalloonTip(3000, "Battery", "Battery reached 80%", ToolTipIcon.Warning);
        }

        static void mnuExit_Click(object sender, EventArgs e)
        {
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}
