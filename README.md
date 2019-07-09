# Unity SOAP
### Repository with example code to communicate with SOAP web services within a Unity3D project

### Why?  
A upcoming project required us to cope with a SOAP-based web-service from which we should retrieve complex data into a Unity3D application which itself should run on iOS and Android mobile platforms.  
After searching for an out-of-the-box solution using WCF, the WSDL and SVCUtil-tools and the related .NET assemblies, unfortunately we were incapable of talking to services and all 'guides' we've found so far point to very old versions of Unity or didn't work out on the mobile platforms.  
This repository is the result of our research and should provide a simple way to get started with some examples using public available WSDL services.  
  
### Initial Situation  
We are currently using Unity3D version 2019.1.9f for the project and also does this repository.  
Our target platforms are iOS and Android (and for development the Unity editor running on Windows).  
Due to requirements given by the app stores we went with the IL2CPP scripting backend for both, iOS and Android.  
  
  \
  \
This repository is mostly matching our requirements and have not been tested against other setups. Making it public and available to the public will give others a chance to use it themselves and contribute to it for broader support of platforms and tested functionality.  


## Base Project Settings  
1.	We've set the project's "Scripting Runtime Version" to ".NET 4.x Equivalent"
2.	We've set the project's "Api Compatibility Level" to ".NET 4.x"
3.	We've added a csc.rsp file to the projects root-folder, it provides the .NET 4.x compiler with additional assembly references.
4.	We've added a link.xml file to prevent code stripping to affect the aditional referenced assembly

## Sample Service Calculator
The first service we've added is the Calculator-service available at http://www.dneonline.com/calculator.asmx?wsdl
It provides service endpoints for Add, Subtract, Multiply and Divide operations executed each on a pair of integer values.
  
  
## Sample Service BLZService
The first service we've added is the BLZ-service available at http://www.thomas-bayer.com/axis2/services/BLZService?wsdl
> BLZ are number unique to the German and Austrian banking system, a BLZ is a identifier code assigned to a specific bank which was used for money transfer prior to the introduction of IBAN.

The service provides one endpoint to retrieve bundled information about the bank identified by a given BLZ. This information contains a short description, postal-code, city and BIC (Bank Identifier Code).
  
  
# Contribute & Support
This respository is the product of other people sharing their experience and code and a big thank you belongs to Stephen from https://long2know.com/2016/07/consuming-a-soap-service-using-httpclient/ which led us to implement communication with soap services using the System.Net.Http.HttpClient class in the end.  
  
Feel free to fork this repository or use this project according to your personal needs. We want to share our knowledge we obtained on this topic with everyone else who may need it.

If you find bugs or solutions we do not cover or did wrong, feel free to leave us an issue, pull request or just a kind note. We are happy to get in contact with you and improve our solution.
