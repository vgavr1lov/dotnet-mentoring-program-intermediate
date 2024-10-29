using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MyCalculatorv1;

public partial class MainWindow : Window, IComponentConnector
{
   public MainWindow()
   {
      InitializeComponent();
   }

   private void Button_Click_1(object sender, RoutedEventArgs e)
   {
      Button button = (Button)sender;
      tb.Text += button.Content.ToString();
   }

   private void Result_click(object sender, RoutedEventArgs e)
   {
      result();
   }

   private void result()
   {
      var num = GetOperationIndex(tb.Text);

      if (num < 0)
         return;

      string text = tb.Text.Substring(num, 1);
      if (!double.TryParse(tb.Text.Substring(0, num), out double num2) ||
          !double.TryParse(tb.Text.Substring(num + 1), out double num3))
      {
         return;
      }

      switch (text)
      {
         case "+":
            {
               TextBox textBox = tb;
               textBox.Text = textBox.Text + "=" + (num2 + num3);
               break;
            }
         case "-":
            {
               TextBox textBox = tb;
               textBox.Text = textBox.Text + "=" + (num2 - num3);
               break;
            }
         case "*":
            {
               TextBox textBox = tb;
               textBox.Text = textBox.Text + "=" + num2 * num3;
               break;
            }
         case "/":
            {
               TextBox textBox = tb;
               textBox.Text = textBox.Text + "=" + num2 / num3;
               break;
            }
      }
   }

   private int GetOperationIndex(string expression)
   {
      if (expression.Contains("+"))
      {
         return expression.IndexOf("+");
      }
      else if (expression.Contains("-"))
      {
         return expression.IndexOf("-");
      }
      else if (expression.Contains("*"))
      {
         return expression.IndexOf("*");
      }
      else if (expression.Contains("/"))
      {
         return expression.IndexOf("/");
      }

      return -1;
   }

   private void RepeatResult()
   {
      var num = tb.Text.IndexOf("=");
   }

   private void Off_Click_1(object sender, RoutedEventArgs e)
   {
      Application.Current.Shutdown();
   }

   private void Del_Click(object sender, RoutedEventArgs e)
   {
      tb.Text = "";
   }

   private void R_Click(object sender, RoutedEventArgs e)
   {
      if (tb.Text.Length > 0)
      {
         tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
      }
   }
}
