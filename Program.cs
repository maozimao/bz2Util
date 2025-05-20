using System;
using System.Windows.Forms;
using MyExeApp.pages.Test; // 添加命名空间引用

namespace MyExeApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new TestForm()); // 类名不变，但命名空间已更新
        }
    }
}