# Lab Assistant
This is an app for the MMS 2016 session: IT PRO to IT SCIENTIST.

######DONE
* Change XML to utilize a master "lab assistant" runbook
* Support for 3 Inputs
* Support for Various Hybrid Worker Groups or Azure

######TODO
* Readmeon how to Implement this on your own
* Add Azure Authenication to Retrieve Runbooks & Information Instead of XML File
* More Attractive UI/UX + COLOR
* "Refactoring"
* "Step builder" for sequencing runbooks?
  
This is a Windows 10 Universal Application designed to load an xml file contining information with Azure Automation webhooks you would like to control from you phone. You will define exisiting runbook names and inputs in your own Azure Automation Environment. This app is designed to call "control runbook" using a single Azure Automation Webhook. This control runbook will then launch the other runbooks using the varibles you provided in the phone app.


####Select your xml file defined Azure Automation Runbook
![alt tag](https://raw.githubusercontent.com/bergotronic/LabAssistant/master/ReadMe/LabAssistant1.png)

####View and Configure Runbook Settings (XML Defineable)
![alt tag](https://raw.githubusercontent.com/bergotronic/LabAssistant/master/ReadMe/LabAssistant2.png)

####Execute and View Results of Webhook call
![alt tag](https://raw.githubusercontent.com/bergotronic/LabAssistant/master/ReadMe/LabAssistant3.png)

###An About Page
![alt tag](https://raw.githubusercontent.com/bergotronic/LabAssistant/master/ReadMe/LabAssistant4.png)

###Everything is Configured via XML config file
![alt tag](https://raw.githubusercontent.com/bergotronic/LabAssistant/master/ReadMe/LabAssistant5.png)

