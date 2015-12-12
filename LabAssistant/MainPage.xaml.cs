using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LabAssistant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        ObservableCollection<String> Actions = new ObservableCollection<String>();

        public MainPage()
        {

            this.InitializeComponent();
            
            Actions.Add("Arial");
            Actions.Add("Courier New");
            Actions.Add("Times New Roman");


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            // GetActionSelection
            // GetLabSelection

            // RunActions


        }
    }
}
