using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ProResp3.UserControls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProResp3.Views
{
    /// <summary>
    /// Interaction logic for SetupView.xaml
    /// </summary>
    public partial class SetupView : UserControl
    {

        public SetupView()
        {
            InitializeComponent();

            int numCols = 3;
            int numRows = Globals.NumValves/numCols;

            for (int i = 0; i < numCols; i++)
            {
                ColumnDefinition newColumnDefinition = new ColumnDefinition();
                newColumnDefinition.Width = new GridLength(1, GridUnitType.Star);
                this.MainGrid.ColumnDefinitions.Add(newColumnDefinition);
            }
            for (int i = 0; i < numRows; i++)
            {
                RowDefinition newRowDefinition = new RowDefinition();
                newRowDefinition.Height = new GridLength(1, GridUnitType.Star);
                this.MainGrid.RowDefinitions.Add(newRowDefinition);
            }

            int valveNum = 1;

            for (int i = 0; i < numCols; i++)
            {
                for (int j = 0; j < numRows; j++)
                {
                    ValveWeightControl newControl = new ValveWeightControl();
                    newControl.label.Text = "Valve " + valveNum;

                    //Set textBox.Text binding to SetupViewModel.ValveWeights[index]
                    Binding weightBinding = new Binding();
                    weightBinding.Path = new PropertyPath("ValveWeights[" + (valveNum - 1).ToString() + "]");
                    weightBinding.Mode = BindingMode.TwoWay;
                    newControl.textBox.SetBinding(TextBox.TextProperty, weightBinding);

                    //Set textBox.IsEnabled binding to 
                    //Binding checkBoxBinding = new Binding();
                    //checkBoxBinding.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(Grid), 2);
                    //checkBoxBinding.Path = new PropertyPath("IsChecked");
                    ////checkBoxBinding.Source = FindName("valve" + valveNum.ToString() + "CheckBox");
                    ////checkBoxBinding.ElementName = "valve" + valveNum.ToString() + "CheckBox";
                    //newControl.textBox.SetBinding(TextBox.IsEnabledProperty, checkBoxBinding);


                    Grid.SetColumn(newControl, i);
                    Grid.SetRow(newControl, j);
                    this.MainGrid.Children.Add(newControl);
                    valveNum++;
                }
            }
        }
    }
}
