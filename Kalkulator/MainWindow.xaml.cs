﻿using Kalkulator.Models;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Kalkulator.Kalkulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Operation m_eLastOperationSelected = Operation.None;
        private Operation m_eOperationSelected = Operation.None;
        private double m_eMemoryValue = 0d;
        private string DisplayDefault;
        private string DisplayBlank;

        /// <summary>
        /// Initializes new instance of <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DisplayDefault = Button0.Content.ToString();
            DisplayBlank = string.Empty;
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button oButton = (Button)sender;
            if (Operation.Result == m_eLastOperationSelected)
            {
                txtDisplay.Clear();
                m_eLastOperationSelected = Operation.None;
            }
            else if (m_eLastOperationSelected != Operation.None)
            {
                txtDisplay.Text = oButton.Content.ToString();
                m_eLastOperationSelected = Operation.None;
                return;
            }
            if (txtDisplay.Text.Contains(DisplayDefault)
                && txtDisplay.Text.Length == 1)
            {
                txtDisplay.Clear();
            }
            txtDisplay.Text += oButton.Content;
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            Button oButton = (Button)sender;
            if (Operation.Result == m_eLastOperationSelected)
            {
                txtDisplay.Text = oButton.Content.ToString();
                m_eLastOperationSelected = Operation.None;
                return;
            }
            else
            {
                if (m_eLastOperationSelected != Operation.None)
                {
                    txtDisplay.Text = DisplayDefault;
                    m_eLastOperationSelected = Operation.None;
                }
                if (txtDisplay.Text.Length > 1
                    || !txtDisplay.Text.Contains(oButton.Content.ToString()))
                {
                    txtDisplay.Text += oButton.Content;
                }
            }
        }

        private void ButtonComma_Click(object sender, RoutedEventArgs e)
        {
            Button oButton = (Button)sender;
            if ((txtDisplay.Text.Contains(oButton.Content.ToString())) ||
                (0 == txtDisplay.Text.Length))
            {
                return;
            }
            txtDisplay.Text += oButton.Content.ToString();
        }

        private void ButtonBackspace_Click(object sender, RoutedEventArgs e)
        {
            if (Operation.Result == m_eLastOperationSelected)
            {
                txtDisplay.Text = DisplayDefault;
                m_eLastOperationSelected = Operation.None;
                return;
            }
            else if (m_eLastOperationSelected != Operation.None)
            {
                txtDisplay.Text = DisplayDefault;
                m_eLastOperationSelected = Operation.None;
                return;
            }
            if (txtDisplay.Text.Length == 1 &&
                !txtDisplay.Text.Contains(DisplayDefault)){
                txtDisplay.Text = DisplayDefault;
                return;
            }
            if (txtDisplay.Text.Length > 1)
            {
                string temp = string.Empty;
                char[] array = txtDisplay.Text.ToArray();
                for (int i = 1; i < txtDisplay.Text.Length; i++)
                    temp += array[i - 1];
                txtDisplay.Text = temp;
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            txtDisplay.Text = DisplayDefault;
            txtDisplayMemory.Clear();
            txtDisplayOperation.Clear();
            m_eLastOperationSelected = Operation.None;
            m_eOperationSelected = Operation.None;
            m_eMemoryValue = 0d;
        }

        private void ButtonOperation_Click(object sender, RoutedEventArgs e)
        {
            Button oButton = (Button)sender;
            switch (oButton.Content.ToString())
            {
                case "+":
                    m_eLastOperationSelected = Operation.Addition;
                    m_eOperationSelected = Operation.Addition;
                    break;
                case "-":
                    m_eLastOperationSelected = Operation.Subtraction;
                    m_eOperationSelected = Operation.Subtraction;
                    break;
                case "*":
                    m_eLastOperationSelected = Operation.Multiplication;
                    m_eOperationSelected = Operation.Multiplication;
                    break;
                case "/":
                    m_eLastOperationSelected = Operation.Division;
                    m_eOperationSelected = Operation.Division;
                    break;
            }
            txtDisplayOperation.Text = oButton.Content.ToString();
            if (txtDisplayMemory.Text.Equals(DisplayBlank))
            {
                txtDisplayMemory.Text = txtDisplay.Text;
            }
        }

        private void ButtonEquals_Click(object sender, RoutedEventArgs e)
        {
            if (m_eOperationSelected != Operation.None)
            {
                double a = 0d, b = 0d, result = 0d;
                MessageBoxResult error;
                if (m_eLastOperationSelected == Operation.Result)
                {
                    a = double.Parse(txtDisplay.Text);
                    b = m_eMemoryValue;
                }
                else
                {
                    a = double.Parse(txtDisplayMemory.Text);
                    b = double.Parse(txtDisplay.Text);
                    m_eMemoryValue = b;
                }
                switch (m_eOperationSelected)
                {
                    case Operation.Addition:
                        result = a + b;
                        break;
                    case Operation.Subtraction:
                        result = a - b;
                        break;
                    case Operation.Multiplication:
                        result = a * b;
                        break;
                    case Operation.Division:
                        if (b != 0d)
                            result = a / b;
                        else
                        {
                            error = MessageBox.Show(
                                this,
                                "Nie da się dzielić przez zero!",
                                "BŁĄD!",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                            ButtonClear_Click(sender, e);
                            return;
                        }
                        break;
                }
                txtDisplay.Text = result.ToString();
                txtDisplayMemory.Clear();
                txtDisplayOperation.Clear();
                m_eLastOperationSelected = Operation.Result;
            }
        }
    }
}
