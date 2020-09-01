using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Navigation;
using AbpDtoGenerator;

namespace YoyoAbpCodePowerProject.WPF.Controls
{
    public partial class Copyright : UserControl
    {
        public Copyright()
        {
            this.InitializeComponent();

            this.txtVersion.Text = "3.2.2";
            this.txtTime.Text = AppConsts.StartAppTime;
            this.txtDateYear.Text = DateTime.Now.Year.ToString();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
