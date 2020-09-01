using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using YoyoAbpCodePowerProject.WPF.Enums;

namespace YoyoAbpCodePowerProject.WPF
{
    public partial class PropertySelectorWindow : Window
    {
        public Action<CallBackTypeEnum> CloseCallBack { get; set; }

        public CallBackTypeEnum BackType { get; set; }

        public PropertySelectorWindow(Action<CallBackTypeEnum> closeCallBack)
        {
            this.InitializeComponent();
            this.CloseCallBack = closeCallBack;
            base.DataContext = Global.PropertyViewModel;
            base.ResizeMode = ResizeMode.NoResize;
            base.WindowStyle = WindowStyle.None;
            base.Closing += this.PropertySelectorWindow_Closing;
            this.title.MouseDown += this.Title_MouseDown;
            base.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private void PropertySelectorWindow_Closing(object sender, CancelEventArgs e)
        {
            Action<CallBackTypeEnum> closeCallBack = this.CloseCallBack;
            if (closeCallBack == null)
            {
                return;
            }
            closeCallBack(this.BackType);
        }

        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            this.BackType = CallBackTypeEnum.Prev;
            base.Close();
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            this.BackType = CallBackTypeEnum.Enter;
            base.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.BackType = CallBackTypeEnum.Cancel;
            base.Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
