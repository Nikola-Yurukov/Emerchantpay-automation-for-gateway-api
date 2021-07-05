Introduciton
=============
This is a short framework that helps you test payment APIs. It is easily expandable, thus it can easily be adapted to test any kind of api. This framework implements a BDD testing approach with Specflow.

Prerequisites
==============
- Download and install Visual Studio(community) - https://visualstudio.microsoft.com/downloads/
- Download the latest .NET framework - https://dotnet.microsoft.com/download/dotnet-framework
- Once the IDE and .NET are set up you will need to install the following NuGet packages:
  - SpecFlow
  - SpecFlow.NUnit
  - RestSharp
  - NUnit
  - NUnit3TestAdapter
  - Newtonsoft.Json
 - Follow the tutorial on how to setup the following payment gateway project - https://github.com/eMerchantPay/codemonsters_api_full
 
 Setup
 =====
 - Open the the project with Visual studio
 - Build the solution
 - Load the test runner from Test --> Test Explorer
 
 Structure
 =========
 - **Features** folder which contains all the different features which contain all the scenarios associated with the different functional requirements.
 - **Files** folder that can be used for reading request bodies, which are sent by the framework.
 - **Steps** folder - Here can be added all the different steps files, which by definition should be strictly associated with the corresponding requirement to have more consistency.
  - **Base** class - This is the class which takes care of the API setup, loading the configuration files and other reusable methods.
  - **PaymentGatewaySteps** - This file is an abstract layer of the base class implementation, which only calls the methods from Base.cs, and does some additional checks. Maps the steps in the feature file with Binding
  - **Extension** - Utility class - Method functionality described in the file.
  - **Context** - Used for passing objects through different methods, by storing different values and re-using them later on.
