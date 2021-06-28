/**
 ******************************************************************************
 * @file    LED Display Control Example/View/MainWindow.cs
 * @author  Adrian Wojcik  adrian.wojcik@put.poznan.pl
 * @version V2.0
 * @date    10-May-2021
 * @brief   LED display controller: Main window View
 ******************************************************************************
 */

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MultiViewApp.View
{
    using ViewModel;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class View3 : UserControl
    {
        private readonly View3_ViewModel _viewModel;
        public View3()
        {
            InitializeComponent();

            _viewModel = new View3_ViewModel();
            DataContext = _viewModel;

            /* Button matrix grid definition */
            for (int i = 0; i < _viewModel.DisplaySizeX; i++)
            {
                ButtonMatrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
                ButtonMatrixGrid.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);
            }

            for (int i = 0; i < _viewModel.DisplaySizeY; i++)
            {
                ButtonMatrixGrid.RowDefinitions.Add(new RowDefinition());
                ButtonMatrixGrid.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Star);
            }

            for (int i = 0; i < _viewModel.DisplaySizeX; i++)
            {
                for (int j = 0; j < _viewModel.DisplaySizeY; j++)
                {
                    // <Button
                    Button led = new Button()
                    {
                        // Name = "LEDij"
                        Name = "LED" + i.ToString() + j.ToString(),
                        // Style="{StaticResource LedButtonStyle}"
                        Style = (Style)FindResource("LedIndicatorStyle"),
                        // BorderThicness="2"
                        BorderThickness = new Thickness(2),
                    };
                    // Command="{Binding LedCommands[i][j]}" 
                    led.SetBinding(Button.CommandProperty, _viewModel.GetCommandBinding(i,j));
                    // Color="{Binding Leds[i][j].ViewColor}" 
                    led.SetBinding(Button.BackgroundProperty, _viewModel.GetColordBinding(i, j));
                    // Grid.Column="i" 
                    Grid.SetColumn(led, i);
                    // Grid.Row="j"
                    Grid.SetRow(led, j);
                    // />

                    ButtonMatrixGrid.Children.Add(led);
                    ButtonMatrixGrid.RegisterName(led.Name, led);
                }
            }
        }
    }
}
