using System;
using System.Windows;

namespace AbpDtoGenerator
{
    public static class MsgEx
    {
        public static void ErrorMsg(this string msg)
        {
            MessageBox.Show(msg, "错误", MessageBoxButton.OK, MessageBoxImage.Hand);
        }

        public static bool WaringMsg(this string msg)
        {
            return MessageBox.Show(msg, "警告", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes;
        }

        public static void InfoMsg(this string msg)
        {
            MessageBox.Show(msg, "信息", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}