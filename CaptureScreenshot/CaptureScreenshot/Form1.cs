using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using AForge.Video.FFMPEG;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.Net;
using System.IO;
using Click2Connect;

namespace CaptureScreenshot
{
    public partial class Form1 : Form
    {
        int temp;
        int h, m, s;
        Bitmap bmp, cursorBMP;
        Graphics gr;
        Rectangle screen = new Rectangle();
        VideoFileWriter write = new VideoFileWriter();
        int cursorX = 0;
        int cursorY = 0;
        int height = Screen.PrimaryScreen.Bounds.Height;
        int width = Screen.PrimaryScreen.Bounds.Width; 
        static string path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Click2Connect");
        string path2 = path+ "\\Captured Images";
        string path1 = path + "\\Recorded Video";   

        public Form1()
        {
            InitializeComponent();
            temp = 0;
            h = m = s = 0;
            btnEnd.Enabled = false;
            SetFormPosition();
            if (!Directory.Exists(path1))
            {
                System.IO.Directory.CreateDirectory(path1);
            }
            if (!Directory.Exists(path2))
            {
                System.IO.Directory.CreateDirectory(path2);
            }
            lblh.ForeColor = Color.SlateGray;
            lblm.ForeColor = Color.SlateGray;
            lbls.ForeColor = Color.SlateGray;
            label1.ForeColor = Color.SlateGray;
            label2.ForeColor = Color.SlateGray;
            label3.ForeColor = Color.SlateGray;
        }

        private void SetFormPosition()
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            this.Top = Screen.PrimaryScreen.WorkingArea.Bottom - this.Height;
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {
            
            try
            {
                this.WindowState = FormWindowState.Minimized;
                System.Threading.Thread.Sleep(500);
                string d = null;
                d = DateTime.Now.ToString(".yyyy.MM.dd,HH.mm.ss");
                string name = "CAPIMG" + d;
                Bitmap bm = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                bm.Save(path2 +"\\"+ name + ".png", ImageFormat.Png);

                this.WindowState = FormWindowState.Normal;
                MessageBox.Show("Image Captured" );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Capturing");
                MessageBox.Show(ex.Message);
            }
        }
             
       
        private void btnRecord_Click(object sender, EventArgs e)
        {
           if (btnRecord.Text == "Record")
           {

                lblh.ForeColor = Color.Black;
                lblm.ForeColor = Color.Black;
                lbls.ForeColor = Color.Black;
                label1.ForeColor = Color.Black;
                label2.ForeColor = Color.Black;
                label3.ForeColor = Color.Black;
                if (temp == 0)
                {
                    string d = null;
                    d = DateTime.Now.ToString(".yyyy.MM.dd,HH.mm.ss");
                    string name = "CAPVID" + d;
                    write.Open(path1 + "\\" + name + ".wmv", width, height, 25, VideoCodec.WMV1, 50000000);
                }
                screen.Height = height;
                screen.Width = width;
                screen.Location = new Point(0, 0);
                t1.Enabled = true;
                t1.Start();
                btnRecord.Text = "Pause";
                btnEnd.Enabled = true;
            }
            else
            {
                temp = 1;
                t1.Stop();
                btnRecord.Text = "Record";
                btnEnd.Enabled = false;
                lblh.ForeColor = Color.SlateGray;
                lblm.ForeColor = Color.SlateGray;
                lbls.ForeColor = Color.SlateGray;
                label1.ForeColor = Color.SlateGray;
                label2.ForeColor = Color.SlateGray;
                label3.ForeColor = Color.SlateGray;
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            t1.Stop();
            write.Close();
            Click2Connect.Selector ent = new Click2Connect.Selector();
            //Click2Connect.Click2Connect cl = new Click2Connect.Click2Connect(ent);
            this.Hide();
            ent.ShowDialog();
            this.Close();
            this.Dispose();
        }

        private void btnEnd_Click(object sender, EventArgs e)
        {
            t1.Stop();
            write.Close();
            btnRecord.Text = "Record";
            MessageBox.Show("Saved");
            temp = 0;
            btnEnd.Enabled = false;
            s = m = h = 0;
            lbls.Text = az(s);
            lblm.Text = az(m);
            lblh.Text = az(h);
            t1.Enabled = false;
            lblh.ForeColor = Color.SlateGray;
            lblm.ForeColor = Color.SlateGray;
            lbls.ForeColor = Color.SlateGray;
            label1.ForeColor = Color.SlateGray;
            label2.ForeColor = Color.SlateGray;
            label3.ForeColor = Color.SlateGray;
        }

        private void t1_Tick(object sender, EventArgs e)
        {
            try
            {   
                s++;
                time(s);
                bmp = new Bitmap(width,height);
                cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                screen = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                gr.DrawImage(cursorBMP, screen);
                gr.Flush();
                for (int i = 0; i <= 4; i++)
                {
                    write.WriteVideoFrame(bmp);
                }
                bmp = new Bitmap(width, height);
                cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                screen = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                gr.DrawImage(cursorBMP, screen);
                gr.Flush();
                for (int i = 0; i <= 4; i++)
                {
                    write.WriteVideoFrame(bmp);
                }
                bmp = new Bitmap(width, height);
                cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                screen = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                gr.DrawImage(cursorBMP, screen);
                gr.Flush();
                for (int i = 0; i <= 4; i++)
                {
                    write.WriteVideoFrame(bmp);
                }
                bmp = new Bitmap(width, height);
                cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                screen = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                gr.DrawImage(cursorBMP, screen);
                gr.Flush();
                for (int i = 0; i <= 4; i++)
                {
                    write.WriteVideoFrame(bmp);
                }
                bmp = new Bitmap(width, height);
                cursorBMP = CaptureCursor(ref cursorX, ref cursorY);
                screen = new Rectangle(cursorX, cursorY, cursorBMP.Width, cursorBMP.Height);
                gr = Graphics.FromImage(bmp);
                gr.CopyFromScreen(0, 0, 0, 0, bmp.Size);
                gr.DrawImage(cursorBMP, screen);
                gr.Flush();
                for (int i = 0; i <= 4; i++)
                {
                    write.WriteVideoFrame(bmp);
                }              
            }
            catch (Exception ex)
            {
                t1.Stop();
                t1.Start();
            }
        }

        private string az(double str)
        {
            if(str<=9)
            {
                return "0" + str;
            }
            else
            {
                return str.ToString();
            }
        }
        void time(int c)
        {
            
            if(s>59)
            {
                m++;
                s = 0;
            }
            if(m>59)
            {
                h++;
                m = 0;
            }

            lbls.Text = az(s);
            lblm.Text = az(m);
            lblh.Text = az(h);
        }

        static Bitmap CaptureCursor(ref int x, ref int y)
        {
            Bitmap bmp;
            IntPtr hicon;
            Win32Stuff.CURSORINFO ci = new Win32Stuff.CURSORINFO();
            Win32Stuff.ICONINFO icInfo;
            ci.cbSize = Marshal.SizeOf(ci);
            if (Win32Stuff.GetCursorInfo(out ci))
            {
                if (ci.flags == Win32Stuff.CURSOR_SHOWING)
                {
                    hicon = Win32Stuff.CopyIcon(ci.hCursor);
                    if (Win32Stuff.GetIconInfo(hicon, out icInfo))
                    {
                        x = ci.ptScreenPos.x - ((int)icInfo.xHotspot);
                        y = ci.ptScreenPos.y - ((int)icInfo.yHotspot);
                        Icon ic = Icon.FromHandle(hicon);
                        bmp = ic.ToBitmap();

                        return bmp;
                    }
                }
            }
            return null;
        }
    }

    class Win32Stuff
    {

        #region Class Variables

        public const int SM_CXSCREEN = 0;
        public const int SM_CYSCREEN = 1;

        public const Int32 CURSOR_SHOWING = 0x00000001;

        [StructLayout(LayoutKind.Sequential)]
        public struct ICONINFO
        {
            public bool fIcon;         // Specifies whether this structure defines an icon or a cursor. A value of TRUE specifies 
            public Int32 xHotspot;     // Specifies the x-coordinate of a cursor's hot spot. If this structure defines an icon, the hot 
            public Int32 yHotspot;     // Specifies the y-coordinate of the cursor's hot spot. If this structure defines an icon, the hot 
            public IntPtr hbmMask;     // (HBITMAP) Specifies the icon bitmask bitmap. If this structure defines a black and white icon, 
            public IntPtr hbmColor;    // (HBITMAP) Handle to the icon color bitmap. This member can be optional if this 
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public Int32 x;
            public Int32 y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CURSORINFO
        {
            public Int32 cbSize;        // Specifies the size, in bytes, of the structure. 
            public Int32 flags;         // Specifies the cursor state. This parameter can be one of the following values:
            public IntPtr hCursor;          // Handle to the cursor. 
            public POINT ptScreenPos;       // A POINT structure that receives the screen coordinates of the cursor. 
        }

        #endregion


        #region Class Functions

        [DllImport("user32.dll", EntryPoint = "GetDesktopWindow")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", EntryPoint = "GetDC")]
        public static extern IntPtr GetDC(IntPtr ptr);

        [DllImport("user32.dll", EntryPoint = "GetSystemMetrics")]
        public static extern int GetSystemMetrics(int abc);

        [DllImport("user32.dll", EntryPoint = "GetWindowDC")]
        public static extern IntPtr GetWindowDC(Int32 ptr);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDc);


        [DllImport("user32.dll", EntryPoint = "GetCursorInfo")]
        public static extern bool GetCursorInfo(out CURSORINFO pci);

        [DllImport("user32.dll", EntryPoint = "CopyIcon")]
        public static extern IntPtr CopyIcon(IntPtr hIcon);

        [DllImport("user32.dll", EntryPoint = "GetIconInfo")]
        public static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO piconinfo);


        #endregion
    }
}
