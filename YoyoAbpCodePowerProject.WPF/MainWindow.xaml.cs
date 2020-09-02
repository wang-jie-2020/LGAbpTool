using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using AbpDtoGenerator;
using AbpDtoGenerator.Models;
using Newtonsoft.Json;
using YoyoAbpCodePowerProject.WPF.Enums;

namespace YoyoAbpCodePowerProject.WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            base.DataContext = Global.MainViewModel;
            base.Loaded += this.MainWindow_Loaded;
            this.title.MouseDown += this.Title_MouseDown;
            base.Closing += this.MainWindow_Closing;
            base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            string text = Path.Combine(Global.SolutionInfo.SolutionPath, "52abp_code_power");
            if (!Directory.Exists(text))
            {
                Directory.CreateDirectory(text);
            }
            string path = Path.Combine(text, "52ABP_CodePowerExtendedModel.json");
            string content = JsonConvert.SerializeObject(Global.MainViewModel.MainExtendedCfg);
            path.CreateFile(content);
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Global.SolutionInfo.CurrentSelectFilePath);
            string path2 = Path.Combine(text, fileNameWithoutExtension + ".json");
            string content2 = JsonConvert.SerializeObject(Global.Entity);
            path2.CreateFile(content2);

            string lgPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "LG", "Options.json");
            string lgContent = JsonConvert.SerializeObject(Global.MainViewModel.LGOptionCfg);
            lgPath.CreateFile(lgContent);
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            base.ResizeMode = ResizeMode.NoResize;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            BasicOptionCfg option = Global.Option;
            if (!option.HasChecked())
            {
                "请至少选择一项!".WaringMsg();
                return;
            }
            base.Visibility = Visibility.Hidden;
            if (option.DoWithoutSelectProperty())
            {
                this.StartCodeGen();
                return;
            }
            if (option.IsAllGeneratorCode || option.UseApplicationServiceCode)
            {
                Global.PropertyViewModel.OnlyShowEditAndListDtoCheckbox(Global.Option.UseNgZorro);
            }
            new PropertySelectorWindow(delegate (CallBackTypeEnum backType)
            {
                switch (backType)
                {
                    case CallBackTypeEnum.Default:
                        break;
                    case CallBackTypeEnum.Prev:
                        base.Visibility = Visibility.Visible;
                        return;
                    case CallBackTypeEnum.Cancel:
                        base.Close();
                        return;
                    case CallBackTypeEnum.Enter:
                        this.StartCodeGen();
                        break;
                    default:
                        return;
                }
            }).Show();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            base.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void StartCodeGen()
        {
            Global.Entity.EntityDisplayName = Global.PropertyViewModel.EntityDisplayName;
            CodeGen.Gen();
            base.Close();
        }
    }
}
