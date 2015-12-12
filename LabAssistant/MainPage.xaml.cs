using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using Windows.UI.Xaml.Media.Imaging;

using System.Net;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.UI.Xaml.Media.Imaging;




// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LabAssistant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static HttpResponseMessage response;
        private string Action;

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

            Debug.WriteLine(ActionListBoxLocal.SelectedIndex.ToString());
            // 0 = EX 1
            // 1 = EX 2 
            // 2 = EX 3

            Debug.WriteLine(LabPivot.SelectedIndex.ToString());
            Action = LabPivot.SelectedIndex.ToString();

            Windows.UI.Popups.MessageDialog messageDialog =
             new Windows.UI.Popups.MessageDialog("Thank you for choosing banana.");
            //await messageDialog.ShowAsync();


            if (Action == "1")
            {
                //WebHook();
            }
    
            // 0 = LOCAL
            // 1 = CLOUDE!





        }

        private async void WebHook()
        {
            Debug.WriteLine(DateTime.Now + " Calling Azure Automation WebHook!");

            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri("https://s1events.azure-automation.net/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP POST
                // response = await client.PostAsJsonAsync("https://s1events.azure-automation.net/webhooks?token=NuX9bfyiWzKM45UAoXp1WQ7mW2PPoDkaMUtxQlL%2f7ps%3d", "");
                //https://s1events.azure-automation.net/webhooks?token=pORbQ527NLVkze%2bf2Kyt%2bNI68%2bz8aiELViZyWp63Cec%3d

                response = await client.PostAsJsonAsync("webhooks?token=pORbQ527NLVkze%2bf2Kyt%2bNI68%2bz8aiELViZyWp63Cec%3d", "");
                if (response.IsSuccessStatusCode)
                {

                    // Debug.WriteLine(response.Content.ToString());
                    Debug.WriteLine(response.StatusCode.ToString());
                }

            }

            Debug.WriteLine(DateTime.Now + " WebHook Function Complete!");
        }


    }
}
