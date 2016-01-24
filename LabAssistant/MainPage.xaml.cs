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
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text;




// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LabAssistant
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static HttpResponseMessage response;
      
        ObservableCollection<String> Actions = new ObservableCollection<String>();

        public MainPage()
        {

            this.InitializeComponent();
         

        }

   
        private async void button_Click(object sender, RoutedEventArgs e)
        {
            // GetActionSelection
            // GetLabSelection

            // RunActions
            ExperimentInProgressRing.IsActive = true;
            // WHICH HEADER DID THEY PICK!

            Load_Experiments();


            string PivotHeaderIndex = LabPivot.SelectedIndex.ToString();

            if (PivotHeaderIndex == "0")
            {
                Debug.WriteLine("Lab Location is Local");

                if (ActionListBoxLocal.SelectedIndex.ToString() == "0")
                {
                    Debug.WriteLine("Single Nano Server Experiment");
                }

                if (ActionListBoxLocal.SelectedIndex.ToString() == "1")
                {
                    Debug.WriteLine("Deploy Many IIS Experiment");
                }

                if (ActionListBoxLocal.SelectedIndex.ToString() == "2")
                {
                    Debug.WriteLine("Deploy SIngle Nano Experiment");
                }

                
            }

            if (PivotHeaderIndex == "1")
            {
                Debug.WriteLine("Lab Location is Cloud");

                if (ActionListBoxCloud.SelectedIndex.ToString() == "0")
                {
                    Debug.WriteLine("Deploy DNS Experiment");
                }

                if (ActionListBoxCloud.SelectedIndex.ToString() == "1")
                {
                    Debug.WriteLine("OMS Lab Deployment Experiment");
                }

                if (ActionListBoxCloud.SelectedIndex.ToString() == "2")
                {
                    Debug.WriteLine("Cloud Linux Text Experiment");
                }

            }

            Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Thank you for choosing an experiment.");
            await messageDialog.ShowAsync();

            ExperimentInProgressRing.IsActive = false;


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

                response = await client.PostAsJsonAsync("webhooks=?tokenshere :)", "");
                if (response.IsSuccessStatusCode)
                {

                    // Debug.WriteLine(response.Content.ToString());
                    Debug.WriteLine(response.StatusCode.ToString());
                }

            }

            Debug.WriteLine(DateTime.Now + " WebHook Function Complete!");
        }



        private async void Load_Experiments()
        {
            try
            {   // Open the text file using a stream reader.

                StringBuilder outputText = new StringBuilder();

                var webRequest = WebRequest.Create(@"https://onedrive.live.com/redir?resid=67cbed2f7b43bc24!24480&authkey=!AEZuhp_6Bmuyhbk&ithint=file%2ctxt");

                //using (var response = webRequest.GetResponse())
                using (var content = response.Content) ;
                Debug.WriteLine(Content.ToString());
               


            }
            catch (Exception e)
            {
                Debug.WriteLine("The file could not be read:");
                Debug.WriteLine(e.Message);
            }

        }



        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {


        



            ActionListBoxLocal.Items.Add("Experiement1");
            ActionListBoxLocal.Items.Add("Experiement2");
            ActionListBoxLocal.Items.Add("Experiement3");


            ActionListBoxCloud.Items.Add("Experiement1");
            ActionListBoxCloud.Items.Add("Experiement2");
            ActionListBoxCloud.Items.Add("Experiement3");


        }
    }
}
