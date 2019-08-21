using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jstris_AUTO
{
    public partial class Form1 : Form
    {
        #region DllImport
        [DllImport("user32.dll")]
        private static extern int RegisterHotKey(IntPtr hwnd, int id, int fsModifiers, Keys vk);

        [DllImport("kernel32.dll")]
        public static extern bool Beep(int n, int m);

        [DllImport("user32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool PrintWindow(IntPtr hwnd, IntPtr hDC, uint nFlags);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);

        [DllImport("gdi32.dll")]
        static extern IntPtr CreateRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("user32.dll")]
        public static extern void keybd_event(uint vk, uint scan, uint flags, uint extraInfo);
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private Bitmap CaptureForm()
        {
            #region Capture
            Bitmap bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.CopyFromScreen(PointToScreen(new Point(pictureBox1.Location.X, pictureBox1.Location.Y)), new Point(0, 0), pictureBox1.Size);
            return bitmap;
            #endregion
        }

        private void Run()
        {
            while (running)
            {
                try
                {
                    #region Capture
                    Bitmap bitmap = CaptureForm();
                    if (bitmap == null)
                    {
                        continue;
                    }
                    int[,] array = new int[20, 10];
                    int[,] _array = new int[4, 4];
                    int k = 0;
                    for (int i = 1; i < 20; i++)
                    {
                        bool b = false;
                        for (int j = 0; j < 10; j++)
                        {
                            if (bitmap.GetPixel(24 * j + 12, 24 * i + 12).GetBrightness() * 100 > 29)
                            {
                                array[i, j] = 100;
                            }
                            else if (bitmap.GetPixel(24 * j + 12, 24 * i + 12).GetBrightness() * 100 > 5)
                            {
                                b = true;
                                _array[k, j - 3] = 100;
                            }
                        }
                        if (b)
                        {
                            k++;
                        }
                    }
                    #endregion
                    #region Detect
                    if (_array[0, 1] >= 100 && _array[0, 2] >= 100 && _array[1, 2] >= 100)
                    {
                        Bruteforce(1, array);
                    }
                    else if (_array[0, 3] >= 100)
                    {
                        Bruteforce(2, array);
                    }
                    else if (_array[0, 0] >= 100 && _array[0, 1] < 100)
                    {
                        Bruteforce(3, array);
                    }
                    else if (_array[0, 1] < 100 && _array[0, 2] >= 100)
                    {
                        Bruteforce(4, array);
                    }
                    else if (_array[0, 0] < 100 && _array[0, 2] < 100)
                    {
                        Bruteforce(5, array);
                    }
                    else if (_array[0, 0] < 100)
                    {
                        Bruteforce(6, array);
                    }
                    else
                    {
                        Bruteforce(7, array);
                    }
                    #endregion
                }
                catch
                {

                }

            }
        }

        private void Bruteforce(int num, int[,] array)
        {
            int cnt = 0, min = int.MaxValue, move = 0, rotate = 0;
            int[] Value = new int[4];
            #region SetShape
            int[,,] _array = new int[4, 4, 4];
            switch (num)
            {
                case 1:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 1, 0 }
                        }
                    };
                    break;
                case 2:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {1, 1, 1, 1 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 1, 0 },
                            {0, 0, 1, 0 },
                            {0, 0, 1, 0 },
                            {0, 0, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {1, 1, 1, 1 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 }
                        }
                    };
                    break;
                case 3:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {1, 0, 0, 0 },
                            {1, 1, 1, 0 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {1, 1, 1, 0 },
                            {0, 0, 1, 0 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 },
                            {1, 1, 0, 0 }
                        }
                    };
                    break;
                case 4:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 1, 0 },
                            {1, 1, 1, 0 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {1, 1, 1, 0 },
                            {1, 0, 0, 0 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {1, 1, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 0, 0 }
                        }
                    };
                    break;
                case 5:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {1, 1, 1, 0 },
                            {0, 0, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {1, 1, 1, 0 },
                            {0, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {1, 1, 0, 0 },
                            {0, 1, 0, 0 }
                        }
                    };
                    break;
                case 6:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {1, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {0, 1, 1, 0 },
                            {0, 0, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {0, 1, 1, 0 },
                            {1, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {1, 0, 0, 0 },
                            {1, 1, 0, 0 },
                            {0, 1, 0, 0 }
                        }
                    };
                    break;
                case 7:
                    _array = new int[4, 4, 4]
                    {
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {1, 1, 0, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 1, 0 },
                            {0, 1, 1, 0 },
                            {0, 1, 0, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 0, 0, 0 },
                            {1, 1, 0, 0 },
                            {0, 1, 1, 0 }
                        },
                        {
                            {0, 0, 0, 0 },
                            {0, 1, 0, 0 },
                            {1, 1, 0, 0 },
                            {1, 0, 0, 0 }
                        }
                    };
                    break;
            }
            #endregion
            for(int i = 0;i < 4;i++)
            {
                int[,] map_tmp = new int[20, 10];
                MapCopy(map_tmp, array);
                for (int j = 0;j < 4;j++)
                {
                    for(int k = 0;k < 4;k++)
                    {
                        map_tmp[j, k + 3] = (_array[i, j, k] == 1 ? 50 : 0);
                    }
                }
                int tmp = Left(map_tmp, i);
                if(tmp < min)
                {
                    min = tmp;
                    move = min_move[i];
                    rotate = i;
                }
            }
            for(int i = 0;i < rotate;i++)
            {
                keybd_event((byte)Keys.Up, 0, 0x00, 0);
                Thread.Sleep(1);
                keybd_event((byte)Keys.Up, 0, 0x02, 0);
                Thread.Sleep(1);
            }
            for(int i = 0;i < 5;i++)
            {
                keybd_event((byte)Keys.Left, 0, 0x00, 0);
                Thread.Sleep(1);
                keybd_event((byte)Keys.Left, 0, 0x02, 0);
                Thread.Sleep(1);
            }
            for (int i = 0; i < move; i++)
            {
                keybd_event((byte)Keys.Right, 0, 0x00, 0);
                Thread.Sleep(1);
                keybd_event((byte)Keys.Right, 0, 0x02, 0);
                Thread.Sleep(1);
            }
            keybd_event((byte)Keys.Space, 0, 0x00, 0);
            Thread.Sleep(1);
            keybd_event((byte)Keys.Space, 0, 0x02, 0);
            Thread.Sleep(50);
            Application.DoEvents();
        }

        private void MapCopy(int[,] a, int[,] b)
        {
            for(int i = 0;i < 20;i++)
            {
                for(int j = 0;j < 10;j++)
                {
                    a[i, j] = b[i, j];
                }
            }
        }

        private int Left(int[,] array, int now)
        {
            bool b;
            string ss = "";
            b = true;
            while (b)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (array[i, 0] == 50)
                    {
                        b = false;
                        break;
                    }
                }
                int[,] map_tmp = new int[20, 10];
                MapCopy(map_tmp, array);
                int errcnt = 0;
                for (int i = 1; i < 10 && b; i++)
                {
                    for (int j = 0; j < 20 && b; j++)
                    {
                        if (map_tmp[j, i] == 50)
                        {
                            if (map_tmp[j, i - 1] == 0)
                            {
                                map_tmp[j, i - 1] = 50;
                                map_tmp[j, i] = 0;
                            }
                            else
                            {
                                errcnt++;
                                b = false;
                            }
                        }
                    }
                }
                if (errcnt == 0)
                {
                    array = map_tmp;
                }
            }
            return func(array, now);
        }

        private bool Right(int[,] _array, int[,] dest)
        {
            int[,] array = new int[20, 10];
            MapCopy(array, _array);
            for(int i = 0;i < 20;i++)
            {
                if(array[i, 9] == 50)
                {
                    return false;
                }
            }
            for(int i = 0;i < 20;i++)
            {
                for(int j = 8;j >= 0;j--)
                {
                    if(array[i, j] == 50)
                    {
                        if(array[i, j + 1] == 0)
                        {
                            array[i, j] = 0;
                            array[i, j + 1] = 50;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            MapCopy(dest, array);
            return true;
        }

        private bool Down(int[,] _array)
        {
            int[,] array = new int[20, 10];
            MapCopy(array, _array);
            for(int i = 0;i < 10;i++)
            {
                if(array[19, i] == 50)
                {
                    return false;
                }
            }
            for(int i = 18;i >= 0;i--)
            {
                for(int j = 0;j < 10;j++)
                {
                    if(array[i, j] == 50)
                    {
                        if(array[i + 1, j] == 0)
                        {
                            array[i, j] = 0;
                            array[i + 1, j] = 50;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            MapCopy(_array, array);
            return true;
        }

        int[] min_move = new int[4];
        private int func(int[,] _array, int now)
        {
            int cnt = int.MaxValue;
            int move_count = 0;
            int[,] array = new int[20, 10];
            int[,] backup = new int[20, 10];
            MapCopy(array, _array);
            bool b = true;
            while(b)
            {
                MapCopy(backup, array);
                //
                //cnt하고 등등
                //
                while(Down(array))
                {

                }
                /*
                string s = "";
                for(int i = 0;i < 20;i++)
                {
                    for(int j = 0;j < 10;j++)
                    {
                        s += (array[i, j] >= 50 ? " ■" : " □");
                    }
                    s += "\n";
                }
                MessageBox.Show(s);
                */
                #region Count
                for (int i = 0;i < 20;i++)
                {
                    for(int j = 0;j < 10;j++)
                    {
                        if(array[i, j] >= 50)
                        {
                            continue;
                        }
                        array[i, j] += i;
                        if(i - 1 >= 0)
                        {
                            if(j + 1 < 10)
                            {
                                if(array[i - 1, j + 1] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                            if(true)
                            {
                                if(array[i - 1, j] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                            if(j - 1 >= 0)
                            {
                                if(array[i - 1, j - 1] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                        }
                        if(i + 1 < 20)
                        {
                            if (j + 1 < 10)
                            {
                                if (array[i + 1, j + 1] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                            if (true)
                            {
                                if (array[i + 1, j] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                            if (j - 1 >= 0)
                            {
                                if (array[i + 1, j - 1] >= 50)
                                {
                                    array[i, j]++;
                                }
                            }
                        }
                        if(j - 1 >= 0)
                        {
                            if(array[i, j - 1] >= 50)
                            {
                                array[i, j]++;
                            }
                        }
                        if(j + 1 < 10)
                        {
                            if(array[i, j + 1] >= 50)
                            {
                                array[i, j]++;
                            }
                        }
                    }
                }
                int tmp = 0;
                for(int i = 0;i < 20;i++)
                {
                    for(int j = 0;j < 10;j++)
                    {
                        if(array[i, j] < 50)
                        {
                            tmp += array[i, j];
                        }
                    }
                }
                #region NoBlank
                bool Blank = false;
                int Blank_cnt = 0;
                int Blank_ALL = 0;
                for(int i = 0;i < 10;i++)
                {
                    Blank = false;
                    for(int j = 0;j < 20;j++)
                    {
                        if(array[j, i] >= 50)
                        {
                            Blank = true;
                        }
                        if(array[j, i] < 50)
                        {
                            if(Blank)
                            {
                                Blank_cnt++;
                            }
                        }
                    }
                    Blank_ALL += (Blank_cnt * 10) * (Blank_cnt * 10);
                    Blank_cnt = 0;
                    Blank = false;
                }
                tmp += Blank_ALL;
                #endregion
                #region NoHeight
                /*
                for(int i = 0;i < 10;i++)
                {
                    int j;
                    for(j = 0;j < 20;j++)
                    {
                        if(array[j, i] >= 50)
                        {
                            break;
                        }
                    }
                    tmp += (19 - j) * (19 - j) * 2;
                }
                */
                #endregion
                if (cnt > tmp)
                {
                    cnt = tmp;
                    min_move[now] = move_count;
                }
                #endregion
                if (!Right(backup, array))
                {
                    b = false;
                }
                move_count++;
            }
            return cnt;
        }
        #region RegisterHotKey
        Thread thread;
        bool running = false;
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x312:
                    var key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    if (key == Keys.F1)
                    {
                        running = !running;
                        if (running)
                        {
                            new Thread(Delegate => Beep(2000, 200)).Start();
                            Run();
                        }
                        else
                        {
                            new Thread(Delegate => Beep(1000, 200)).Start();
                        }
                    }
                    if (key == Keys.F2)
                    {

                    }
                    break;
                default:
                    break;
            }
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(Handle, 0, 0, Keys.F1);
            RegisterHotKey(Handle, 1, 0, Keys.F2);
        }
        #endregion
    }
}

/*
□※※□ ■■■※ ※□□□ □□※□
□■※□ □□□□ ■■■□ ■■■□
□□□□ □□□□ □□□□ □□□□
□□□□ □□□□ □□□□ □□□□

□■□□ □■■□ ■■□□ □□□□
■■■□ ■■□□ □■■□ □□□□
□□□□ □□□□ □□□□ □□□□
□□□□ □□□□ □□□□ □□□□
*/
