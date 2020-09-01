using System;
using System.IO;
using System.Windows;
using AbpDtoGenerator;

namespace YoyoAbpCodePowerProject.WPF
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            if (args == null || args == null || args.Length == 0)
            {
                MessageBox.Show("初始化失败！初始化参数不正确！");
                return;
            }
            string text = args[0];
            if (!Directory.Exists(text))
            {
                MessageBox.Show("初始化失败！初始化数据不正确！");
                return;
            }
            Global.SolutionPath = Path.Combine(text, "solutionInfo.json");
            if (!File.Exists(Global.SolutionPath))
            {
                MessageBox.Show("初始化失败！未能成功创建启动数据！");
                return;
            }
            try
            {
                Global.InitApplication(Global.SolutionPath);
                new App().Run(new MainWindow());
            }
            catch (Exception ex)
            {
                ("初始化UI错误，错误信息:" + ex.ToString()).ErrorMsg();
                MessageBox.Show("初始化UI错误");
            }
        }
    }
}