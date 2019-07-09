using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace VRTX.Net
{
    public class SoapClientProxy : MonoBehaviour
    {
        [SerializeField()]
        private TMPro.TextMeshProUGUI _results = null;
        [SerializeField()]
        private TMPro.TMP_InputField _input = null;

        // guide on building soap requests:
        // https://long2know.com/2016/07/consuming-a-soap-service-using-httpclient/

        // service url:
        // http://www.thomas-bayer.com/axis2/services/BLZService?wsdl
        // service base url:
        // http://www.thomas-bayer.com/axis2/services/BLZService
        // service namespace:
        // http://thomas-bayer.com/blz/

        BLZServiceSoapClient _blzServiceClient = new BLZServiceSoapClient();
        CalculatorSoapClient _calculatorClient = new CalculatorSoapClient();
        // Start is called before the first frame update
        public async void ExecuteBLZRequest()
        {
            Dictionary<string, object> endPointParameters = new Dictionary<string, object>();
            endPointParameters.Add("blz", _input.text);
            SoapResponse soapResponse = await _blzServiceClient.ProcessRequest(_blzServiceClient.SoapRequest("getBank", endPointParameters));
            if (soapResponse != null)
            {
                _results.text = soapResponse.ReasonPhrase + Environment.NewLine;
                GetBankResult complexType = soapResponse.GetComplexType<GetBankResult>();

                _results.text += complexType.Bezeichnung.ToString() + Environment.NewLine;
                _results.text += complexType.BIC.ToString() + Environment.NewLine;
                _results.text += complexType.PLZ.ToString() + " " + complexType.Ort.ToString();
            }
        }

        public async void ExecuteCalculatorAddRequest()
        {
            Dictionary<string, object> endPointParameters = new Dictionary<string, object>();
            endPointParameters.Add("intA", UnityEngine.Random.Range(-short.MaxValue, short.MaxValue));
            endPointParameters.Add("intB", UnityEngine.Random.Range(-short.MaxValue, short.MaxValue));
            SoapResponse soapResponse = await _calculatorClient.SendRequest("Add", endPointParameters);
            if (soapResponse != null)
            {
                _results.text = soapResponse.ReasonPhrase + Environment.NewLine;
                AddResponse complexType = soapResponse.GetComplexType<AddResponse>();
                _results.text += complexType.Value.ToString() + Environment.NewLine;
            }
        }

    }


    public class BLZServiceSoapClient : SoapClient
    {
        // soap client implementation for BLZService (public sample wsdl service)
        // API-URL:     http://www.thomas-bayer.com/axis2/services/BLZService
        // Namespace:   http://thomas-bayer.com/blz/
        //  List of available commands/endpoints
        //              getBank(string blz)
        public BLZServiceSoapClient() : base("http://www.thomas-bayer.com/axis2/services/BLZService", "http://thomas-bayer.com/blz/")
        { }
    }
    /// <summary>
    /// sample implementation of complexType 'detailsType' for BLZService wsdl service
    /// </summary>
    [XmlRoot(ElementName = "details", Namespace = "http://thomas-bayer.com/blz/")]
    public class GetBankResult : SoapComplexType
    {
        // mandatory 'new' static properties/values to provide correct full qualified xml element name for the complex type
        protected static new XNamespace Namespace
        { get; } = "http://thomas-bayer.com/blz/";
        protected static new string ElementName
        { get; } = "details";
        public static new XName FullQualifiedElementName
        { get; } = Namespace + ElementName;

        // optional member fields mapped using XmlElement and other Xml..-attributes
        //  ->  they are deserialized using the SoapResponse.Deserialize method
        [XmlElement("bezeichnung")]
        public string Bezeichnung { get; set; }
        [XmlElement("bic")]
        public string BIC { get; set; }
        [XmlElement("ort")]
        public string Ort { get; set; }
        [XmlElement("plz")]
        public string PLZ { get; set; }

    }


    public class CalculatorSoapClient : SoapClient
    {
        // soap client implementation for Calculator (public sample wsdl service)
        // API-URL:     http://www.dneonline.com/calculator.asmx
        // Namespace:   http://tempuri.org/
        //  List of available commands/endpoints
        //              Add(int intA, int intB)
        //              Subtract(int intA, int intB)
        //              Multiply(int intA, int intB)
        //              Divide(int intA, int intB)
        public CalculatorSoapClient() : base("http://www.dneonline.com/calculator.asmx", "http://tempuri.org/")
        { }
    }

    [XmlRoot(ElementName = "AddResponse", Namespace = "http://tempuri.org/")]
    public class AddResponse : SoapComplexType
    {
        protected static new XNamespace Namespace
        { get; } = "http://tempuri.org/";
        protected static new string ElementName
        { get; } = "AddResponse";
        public static new XName FullQualifiedElementName
        { get; } = Namespace + ElementName;

        [XmlElement("AddResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "SubtractResponse", Namespace = "http://tempuri.org/")]
    public class SubtractResponse : SoapComplexType
    {
        protected static new XNamespace Namespace
        { get; } = "http://tempuri.org/";
        protected static new string ElementName
        { get; } = "SubtractResponse";
        public static new XName FullQualifiedElementName
        { get; } = Namespace + ElementName;

        [XmlElement("SubtractResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "MultiplyResponse", Namespace = "http://tempuri.org/")]
    public class MultiplyResponse : SoapComplexType
    {
        protected static new XNamespace Namespace
        { get; } = "http://tempuri.org/";
        protected static new string ElementName
        { get; } = "MultiplyResponse";
        public static new XName FullQualifiedElementName
        { get; } = Namespace + ElementName;

        [XmlElement("MultiplyResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "DivideResponse", Namespace = "http://tempuri.org/")]
    public class DivideResponse : SoapComplexType
    {
        protected static new XNamespace Namespace
        { get; } = "http://tempuri.org/";
        protected static new string ElementName
        { get; } = "DivideResponse";
        public static new XName FullQualifiedElementName
        { get; } = Namespace + ElementName;

        [XmlElement("DivideResult")]
        public int Value { get; set; }
    }



}