using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SetAlwaysOnTopApp
{
    
    public partial class Form1 : Form
    {

        NotifyIcon trayIcon = new NotifyIcon();

        public string popup_name = "QuickOrder";
        private bool allowVisible;     // ContextMenu's Show command used

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        enum KeyModifier
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            WinKey = 8
        }

        public Form1()
        {

            if (Methods.IsProcessOpen())
            {
                // If ther is more than one, than it is already running.
                MessageBox.Show("Application is already running.");
                Application.Exit();
                Environment.Exit(0);
                return;
            }

            InitializeComponent();
            
            Methods.AoT_on(popup_name);
            Assembly _assembly = Assembly.GetExecutingAssembly();
            
            // Initialize Tray Icon
            TrayIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                    new MenuItem("Quick Order v2.0"),
                    new MenuItem("Set AOT", SetAOT),
                    new MenuItem("About", AboutBox),
                    new MenuItem("Exit", Xit)
            });

            TrayIcon.ShowBalloonTip(5000, "AlwaysOnTop", "AlwaysOnTop is running in the background.", ToolTipIcon.Info);
            TrayIcon.Visible = true;

            //Hot Key
            int id = 0;     // The id of the hotkey. 
            RegisterHotKey(this.Handle, id, 0x0000, Keys.F7.GetHashCode());       // Register Shift + F7 as global hotkey. 
            int id2 = 1;     // The id of the hotkey. 
            RegisterHotKey(this.Handle, id2, 0x0000, Keys.F8.GetHashCode());       // Register Shift + F8 as global hotkey. 
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x0312)
            {
                /* Note that the three lines below are not needed if you only want to register one hotkey.
                 * The below lines are useful in case you want to register multiple keys, which you can use a switch with the id as argument, or if you want to know which key/modifier was pressed for some particular reason. */

                Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);                  // The key of the hotkey that was pressed.
                KeyModifier modifier = (KeyModifier)((int)m.LParam & 0xFFFF);       // The modifier of the hotkey that was pressed.
                int id = m.WParam.ToInt32();                                        // The id of the hotkey that was pressed.

                if (id == 0)
                {
                    Methods.AoT_on(popup_name);
                    
                    //MessageBox.Show("Hotkey F7 has been pressed!");
                }


                if (id == 1)
                {
                    //Methods.AoT_off(popup_name);
                    Methods.SetView(popup_name);
                    //MessageBox.Show("Hotkey F8 has been pressed!");
                }
                // do something
            }
        }

        // An form chinh
        protected override void SetVisibleCore(bool value)
        {
            if (!allowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        //private void TrayIcon_Click(object sender, EventArgs e) //let left click behave the same as right click
        //{
        //    MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
        //    mi.Invoke(trayIcon, null);
        //}

        public NotifyIcon TrayIcon
        {
            get { return trayIcon; }
            set { trayIcon = value; }
        }

        void AboutBox(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        } // AboutBox()

        void SetAOT(object sender, EventArgs e)
        {
            Methods.AoT_on(popup_name);
        } // AboutBox()

        public void Xit(object sender, EventArgs e)
        {
            Application.Exit();
            Environment.Exit(0);
        } // Xit()

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey(this.Handle, 0);
            UnregisterHotKey(this.Handle, 1);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(trayIcon, null);
            }
            else//left or middle click
            {
                Methods.AoT_on(popup_name);
            }
        }
    }
}
