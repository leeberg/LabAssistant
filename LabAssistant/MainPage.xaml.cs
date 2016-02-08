using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Xml;
using System.Threading.Tasks;
using Newtonsoft.Json;



// NOT FOR UWP :(
//using System.Security.Cryptography;


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

        public string CurrentRunbook;
        public string CurrentHybridGroup;

        public string CurrentInput1;
        public string CurrentInput2;
        public string CurrentInput3;

        public string UserInput1;
        public string UserInput2;
        public string UserInput3;


        public Dictionary<string, string> ExperimentDictionary = new Dictionary<string, string>();

        class WebHookInput
        {
            public String Input1 { get; set; }
            public String Input3 { get; set; }
            public String Input2 { get; set; }

        }



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

                    
                    String LabAssitantMainString = CurrentRunbook + ';' + CurrentHybridGroup + ';' + UserInput1 + ';' + UserInput2 + ';' + UserInput3;
                    Debug.WriteLine("Starting WebHook:");
                    Debug.WriteLine(WebHookToken);
                    Debug.WriteLine(LabAssitantMainString);

                    response = await client.PostAsJsonAsync("webhooks?token=" + WebHookToken, LabAssitantMainString);

                    if (response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine(DateTime.Now + " WebHook Function Success!");
                        webHookResult = "OK";

                    }
                    else
                    {
                        Debug.WriteLine(DateTime.Now + " WebHook Function Failed!");

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
            {

                // Open the text file using a stream reader.



                /*
                Uri ExperimentsXML = new Uri("https://raw.githubusercontent.com/bergotronic/LabAssistant/blob/master/Experiments/Local.txt");
                
                https://raw.githubusercontent.com/octokit/octokit.rb/master/README.md"
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
                */


                // LOAD THE XML SETTINGS
                Windows.Storage.StorageFolder appFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
                Windows.Storage.StorageFile sampleFile = await appFolder.GetFileAsync("Experiments.xml");

                string text = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                XmlDocument doc = new XmlDocument();

                doc.LoadXml(text);



//                XmlNodeList xnList = doc.DocumentElement.s("/HybridWorkerGroup");


                foreach (XmlNode node in doc.DocumentElement.ChildNodes)
                {

                    if (node.Name == "HybridWorkerGroup")
                    {
                        String HybridWorkerGroupName = node["Name"].InnerText;
                        Debug.WriteLine("Found Hybrid Runbook Worker " + HybridWorkerGroupName);
                        HybridWorkerGroupList.Items.Add(HybridWorkerGroupName);
                    }

                    if (node.Name == "WebHook")
                    {
                        CurrentWebHook = node["Name"].InnerText;

                    }

                    if (node.Name == "Experiment")
                        {
                        string textd = node.InnerText; //or loop through its children as well

                        String experimentName = node["Name"].InnerText;
                        String experimentDescription = node["Description"].InnerText;
                        String experimentRunbook = node["RunbookName"].InnerText;
                       
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

                    
                        ActionListBoxRunbook.Items.Add(experimentName);

                        ExperimentDictionary.Add(experimentName + "_Description", experimentDescription);
                        ExperimentDictionary.Add(experimentName + "_RunbookName", experimentRunbook);


                    }

                    




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
            UserInput1 = "";
            UserInput2 = "";
            UserInput3 = "";

            IMGRunbookOK.Visibility = Visibility.Collapsed;
            IMGRunbookFAIL.Visibility = Visibility.Collapsed;


            string ActionListIndex = ActionListBoxRunbook.SelectedIndex.ToString();

            try
            {
                string SelectedName = ActionListBoxRunbook.SelectedItem.ToString();
                Debug.WriteLine(SelectedName);

                if (ExperimentDictionary.ContainsKey(SelectedName + "_RunbookName"))
                {
                    CurrentRunbook = ExperimentDictionary[SelectedName + "_RunbookName"];
                    TextBoxRunbookName.Text = CurrentRunbook;
                }

                if (ExperimentDictionary.ContainsKey(SelectedName + "_Description"))
                {
                    CurrentDescription = ExperimentDictionary[SelectedName + "_Description"];
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

        private void LabPivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            UserInput1 = TextBoxWebHookInput1.Text;
            UserInput2 = TextBoxWebHookInput2.Text;
            UserInput3 = TextBoxWebHookInput3.Text;

        }

        private void HybridWorkerGroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentHybridGroup = HybridWorkerGroupList.SelectedItem.ToString();
            Debug.WriteLine("Current Hybrid Group is: " + CurrentHybridGroup);
        }
    }
}
