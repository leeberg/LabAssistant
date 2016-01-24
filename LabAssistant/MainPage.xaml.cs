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
using Windows.Networking.Sockets;
using Windows.Networking;
using System.Xml;
using System.Collections;
using System.Threading.Tasks;




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

        public string CurrentWebHook;
        public string CurrentDescription;

        public string CurrentInput1;
        public string CurrentInput2;
        public string CurrentInput3;

        public Dictionary<string, string> ExperimentDictionary = new Dictionary<string, string>();

        public MainPage()
        {

            this.InitializeComponent();
         
        }




        private async Task<string> WebHook(string WebHookToken)
        {
            Debug.WriteLine(DateTime.Now + " Calling Azure Automation WebHook!");
            string webHookResult = "FAIL";

            try
            {
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri("https://s1events.azure-automation.net/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    response = await client.PostAsJsonAsync("webhooks?token=" + WebHookToken, "");

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(DateTime.Now + " WebHook Function Success!");
                        webHookResult = "OK";

                    }
                    else
                    {
                        Debug.WriteLine(DateTime.Now + " WebHook Function Failed!");


                        ;
                    }

                }
            }
            
            catch
            {
                Debug.WriteLine("shit's on fire yo.");
            }

            Debug.WriteLine(DateTime.Now + " WebHook Function Complete!");
            return webHookResult;

        }





        private async void button_Click(object sender, RoutedEventArgs e)
        {

            string webhookresult;

 
            // TODO 
            // ADD THE INPUTS TO THE WEBHOOK

            // RunActions
            ExperimentInProgressRing.IsActive = true;
                       

            //Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Thank you for choosing an experiment.");
            //await messageDialog.ShowAsync();

            //AWAIT WEBHOOK
            webhookresult = await WebHook(CurrentWebHook);


            if (webhookresult == "OK")
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Thank you for choosing an experiment, the experiment was called sucessfully.");
                await messageDialog.ShowAsync();
                IMGRunbookOK.Visibility = Visibility.Visible;
                IMGRunbookFAIL.Visibility = Visibility.Collapsed;

            }
            else
            {
                Windows.UI.Popups.MessageDialog messageDialog = new Windows.UI.Popups.MessageDialog("Thank you for choosing an experiment, the experiment could NOT be stared.");
                await messageDialog.ShowAsync();
                IMGRunbookOK.Visibility = Visibility.Collapsed;
                IMGRunbookFAIL.Visibility = Visibility.Visible;

            }




            //present Results

            ExperimentInProgressRing.IsActive = false;


        }



        private async void Load_Experiments()
        {
            try
            {   // Open the text file using a stream reader.



                Uri LocalExperiments = new Uri("https://github.com/bergotronic/LabAssistant/blob/master/Experiments/Local.txt");
         
        
                
                //Create an HTTP client object
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();

                //Add a user-agent header to the GET request. 
                var headers = httpClient.DefaultRequestHeaders;


                //Send the GET request asynchronously and retrieve the response as a string.
                Windows.Web.Http.HttpResponseMessage httpResponse = new Windows.Web.Http.HttpResponseMessage();
                string httpResponseBody = "";

                try
                {
                    //Send the GET request
                    httpResponse = await httpClient.GetAsync(LocalExperiments);
                    httpResponse.EnsureSuccessStatusCode();
                    httpResponseBody = await httpResponse.Content.ReadAsStringAsync();

               //     Debug.WriteLine(httpResponseBody.ToString());

                }
                catch (Exception ex)
                {
                    httpResponseBody = "Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message;
                }



                // LOAD THE XML SETTINGS
                Windows.Storage.StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                Windows.Storage.StorageFile sampleFile = await appFolder.GetFileAsync("test.xml");

                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(text);


                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {
                    string textd = node.InnerText; //or loop through its children as well

                    String experimentName = node["Name"].InnerText;
                    String experimentDescription = node["Description"].InnerText;
                    String experimentWebHook = node["Webhook"].InnerText;
                    String experimentType = node["Type"].InnerText;


                    try
                    {
                        string ExperimentInput1 = node["Input1"].InnerText;
                        ExperimentDictionary.Add(experimentName + "_Input1", ExperimentInput1);

                        string ExperimentInput2 = node["Input2"].InnerText;
                        ExperimentDictionary.Add(experimentName + "_Input2", ExperimentInput2);

                        string ExperimentInput3 = node["Input3"].InnerText;
                        ExperimentDictionary.Add(experimentName + "_Input3", ExperimentInput3);
                    }

                    catch
                    {
                        Debug.WriteLine("experiment is missing some inputs");
                    }


                    Debug.WriteLine("Name: " + experimentName + "Desc: " + experimentDescription);
                    //Hello World ExpLocalBLows Up your Datacenterhttp://Webhook.com
                                       
                    ActionListBoxRunbook.Items.Add(experimentName);
                 
                    ExperimentDictionary.Add(experimentName + "_Description", experimentDescription);
                    ExperimentDictionary.Add(experimentName + "_WebHook", experimentWebHook);




                }


            }
            catch (Exception e)
            {
                Debug.WriteLine("The file could not be read:");
                Debug.WriteLine(e.Message);
            }

        }







        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {

            Load_Experiments();

        }



        private void ActionListBoxRunbook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBoxWebHookInput1.Text = "";
            TextBoxWebHookInput2.Text = "";
            TextBoxWebHookInput3.Text = "";
            IMGRunbookOK.Visibility = Visibility.Collapsed;
            IMGRunbookFAIL.Visibility = Visibility.Collapsed;


            string ActionListIndex = ActionListBoxRunbook.SelectedIndex.ToString();

            try
            {
                string SelectedName = ActionListBoxRunbook.SelectedItem.ToString();
                Debug.WriteLine(SelectedName);

                if (ExperimentDictionary.ContainsKey(SelectedName + "_WebHook"))
                {
                    CurrentWebHook = ExperimentDictionary[SelectedName + "_WebHook"];
                    Debug.WriteLine(CurrentWebHook);

                    TextBoxWebHookUrl.Text = CurrentWebHook;
                }

                if (ExperimentDictionary.ContainsKey(SelectedName + "_Description"))
                {
                    CurrentDescription = ExperimentDictionary[SelectedName + "_Description"];
                    Debug.WriteLine(CurrentWebHook);

                    TextBoxDescription.Text = CurrentDescription;
                }


                //Handle Inputs

                if (ExperimentDictionary.ContainsKey(SelectedName + "_Input1"))
                {
                    CurrentInput1 = ExperimentDictionary[SelectedName + "_Input1"];
                    Debug.WriteLine(CurrentInput1);

                    TextBlockWebHookInput1.Visibility = Visibility.Visible;
                    TextBoxWebHookInput1.Visibility = Visibility.Visible;
                    TextBlockWebHookInput1.Text = CurrentInput1;
                }
                else
                {
                    TextBlockWebHookInput1.Visibility = Visibility.Collapsed;
                    TextBoxWebHookInput1.Visibility = Visibility.Collapsed;
                    CurrentInput1 = "";
                }


                if (ExperimentDictionary.ContainsKey(SelectedName + "_Input2"))
                {
                    CurrentInput2 = ExperimentDictionary[SelectedName + "_Input2"];
                    Debug.WriteLine(CurrentInput2);

                    TextBlockWebHookInput2.Visibility = Visibility.Visible;
                    TextBoxWebHookInput2.Visibility = Visibility.Visible;
                    TextBlockWebHookInput2.Text = CurrentInput2;
                }
                else
                {
                    TextBlockWebHookInput2.Visibility = Visibility.Collapsed;
                    TextBoxWebHookInput2.Visibility = Visibility.Collapsed;
                    CurrentInput2 = "";
                }



                if (ExperimentDictionary.ContainsKey(SelectedName + "_Input3"))
                {
                    CurrentInput3 = ExperimentDictionary[SelectedName + "_Input3"];
                    Debug.WriteLine(CurrentInput3);

                    TextBlockWebHookInput3.Visibility = Visibility.Visible;
                    TextBoxWebHookInput3.Visibility = Visibility.Visible;
                    TextBlockWebHookInput3.Text = CurrentInput3;
                }
                else
                {
                    TextBlockWebHookInput3.Visibility = Visibility.Collapsed;
                    TextBoxWebHookInput3.Visibility = Visibility.Collapsed;
                    CurrentInput3 = "";
                }



            }
            catch (Exception ex)
            {
                //
            }


        }

        private void ActionListBoxRunbook_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
