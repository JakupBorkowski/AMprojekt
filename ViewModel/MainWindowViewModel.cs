using System;
using System.Collections.Generic;
using System.Text;

namespace MultiViewApp.ViewModel
{
    /** 
     * @brief View model for MainWindow.xaml 
     */
    public class MainWindowViewModel : BaseViewModel
    {
        private BaseViewModel _contetnViewModel;
        public BaseViewModel ContentViewModel
        {
            get { return _contetnViewModel; }
            set
            {
                if(_contetnViewModel != value)
                {
                    _contetnViewModel = value;
                    OnPropertyChanged("ContentViewModel");
                }
            }
        }

        public ButtonCommand MenuCommandView1 { get; set; } //!< 'VIEW1' button command
        public ButtonCommand MenuCommandView2 { get; set; } //!< 'VIEW2' button command
        public ButtonCommand MenuCommandView3 { get; set; } //!< 'VIEW3' button command
        public ButtonCommand MenuCommandView4 { get; set; } //!< 'VIEW4' button command
        public ButtonCommand MenuCommandView5 { get; set; } //!< 'VIEW5' button command

        /**
         * @brief MainWindowViewModel constructor.
         */
        public MainWindowViewModel()
        {
            MenuCommandView1 = new ButtonCommand(MenuSetView1);
            MenuCommandView2 = new ButtonCommand(MenuSetView2);
            MenuCommandView3 = new ButtonCommand(MenuSetView3);
            MenuCommandView4 = new ButtonCommand(MenuSetView4);
            MenuCommandView5 = new ButtonCommand(MenuSetView5);

            //ContentViewModel = new View3_ViewModel(); // View1_ViewModel.Instance
        }
        /**
        * @brief Set MenuSetView1 procedure
        */
        private void MenuSetView1()
        {
            ContentViewModel = new View1_ViewModel(); // View1_ViewModel.Instance
        }
        /**
        * @brief Set MenuSetView2 procedure
        */
        private void MenuSetView2()
        {
            ContentViewModel = new View2_ViewModel(); // View2_ViewModel.Instance
        }
        /**
        * @brief Set MenuSetView3 procedure
        */
        private void MenuSetView3()
        {
            ContentViewModel = new View3_ViewModel(); // View3_ViewModel.Instance
        }
        /**
        * @brief Set MenuSetView4 procedure
        */
        private void MenuSetView4()
        {
            ContentViewModel = new View4_ViewModel(); // View4_ViewModel.Instance
        }
        /**
        * @brief Set MenuSetView5 procedure
        */
        private void MenuSetView5()
        {
            ContentViewModel = new View5_ViewModel(); // View5_ViewModel.Instance
        }
    }
}
