using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace VRTX.Net
{
    public class SoapClientSamples : MonoBehaviour
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

        private BLZServiceSoapClient _blzServiceClient = new BLZServiceSoapClient();
        private CalculatorSoapClient _calculatorClient = new CalculatorSoapClient();
        // Start is called before the first frame update
        public async void ExecuteBLZRequest()
        {
            GetBankRequest request = new GetBankRequest()
            { BLZ = _input.text };

            GetBankResponse response = await _blzServiceClient.RequestAsync<GetBankResponse, GetBankRequest>("getBank", request);
            if (response != null)
            {
                _results.text = "Result:" + Environment.NewLine;
                _results.text += response.Bezeichnung.ToString() + Environment.NewLine;
                _results.text += response.BIC.ToString() + Environment.NewLine;
                _results.text += response.PLZ.ToString() + " " + response.Ort.ToString();
            }
        }

        public async void ExecuteCalculatorAddRequest()
        {
            Dictionary<string, object> endPointParameters = new Dictionary<string, object>();
            endPointParameters.Add("intA", UnityEngine.Random.Range(-short.MaxValue, short.MaxValue));
            endPointParameters.Add("intB", UnityEngine.Random.Range(-short.MaxValue, short.MaxValue));
            AddRequest request = new AddRequest()
            {
                intA = UnityEngine.Random.Range(-short.MaxValue, short.MaxValue),
                intB = UnityEngine.Random.Range(-short.MaxValue, short.MaxValue)
            };
            AddResponse response = await _calculatorClient.RequestAsync<AddResponse, AddRequest>("Add", request);
            if (response != null)
                _results.text = response.Value.ToString() + Environment.NewLine;
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
    public class GetBankResponse : SoapResponseType
    {
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
    [XmlRoot(ElementName = "getBank", Namespace = "http://thomas-bayer.com/blz/")]
    public class GetBankRequest : SoapRequestType
    {
        [XmlElement("blz")]
        public string BLZ { get; set; }
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
    public class AddResponse : SoapResponseType
    {
        [XmlElement("AddResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Add", Namespace = "http://tempuri.org/")]
    public class AddRequest : SoapRequestType
    {
        [XmlElement("intA")]
        public int intA { get; set; }
        [XmlElement("intB")]
        public int intB { get; set; }
    }

    [XmlRoot(ElementName = "SubtractResponse", Namespace = "http://tempuri.org/")]
    public class SubtractResponse : SoapResponseType
    {
        [XmlElement("SubtractResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "MultiplyResponse", Namespace = "http://tempuri.org/")]
    public class MultiplyResponse : SoapResponseType
    {
        [XmlElement("MultiplyResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "DivideResponse", Namespace = "http://tempuri.org/")]
    public class DivideResponse : SoapResponseType
    {
        [XmlElement("DivideResult")]
        public int Value { get; set; }
    }

}