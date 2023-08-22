using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProResp3
{
    using ProResp3.CustomEventArgs;
    using ProResp3.ViewModels;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
            this.selectValveListBox.ItemsSource = MakeValveCheckBoxes(Globals.NumValves);
            (this.DataContext as MainViewModel).MessageBoxRequest += new EventHandler<MvvmMessageBoxEventArgs>(MyView_MessageBoxRequest);
        }

        void MyView_MessageBoxRequest(object sender, MvvmMessageBoxEventArgs e)
        {
            e.Show();
        }

        private List<CheckBox> MakeValveCheckBoxes(int newAmount)
        {
            List<CheckBox> checkBoxes = new List<CheckBox>();

            for (int i = 0; i < newAmount; i++)
            {
                CheckBox newCheckBox = new CheckBox();
                newCheckBox.Name = "valve" + (i + 1).ToString() + "CheckBox";
                newCheckBox.Content = "Valve " + (i + 1).ToString();
                newCheckBox.DataContext = DataContext;
                

                Binding valveCheckBinding = new Binding();
                valveCheckBinding.Path = new PropertyPath("CheckedValves[" + i + "]");
                valveCheckBinding.Mode = BindingMode.TwoWay;
                valveCheckBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
                newCheckBox.SetBinding(CheckBox.IsCheckedProperty, valveCheckBinding);
                BindingOperations.SetBinding(newCheckBox, CheckBox.IsCheckedProperty, valveCheckBinding);

                Binding isEnabledBinding = new Binding();
                isEnabledBinding.Path = new PropertyPath("NotExperimentRunning");
                BindingOperations.SetBinding(newCheckBox, CheckBox.IsEnabledProperty, isEnabledBinding);

                checkBoxes.Add(newCheckBox);
            }

            return checkBoxes;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window test = (Window)sender;
            MainViewModel test1 = test.DataContext as MainViewModel;
            test1.CloseButtonClick.Execute("CloseButton");
            return;
        }

        private void valveSwitchTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
