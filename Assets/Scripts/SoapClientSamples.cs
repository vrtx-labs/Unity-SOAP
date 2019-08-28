using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using VRTX.Net.Samples.BLZService;
using VRTX.Net.Samples.Calculator;

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

            // verbose way of sending a request
            //GetBankResponse response = await _blzServiceClient.RequestAsync<GetBankResponse, GetBankRequest>("getBank", request);
            // concise way of sending request using the SoapOperation implementation
            GetBankResponse response = await GetBank.Execute(_blzServiceClient, request);
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
            AddRequest request = new AddRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            //AddResponse response = await _calculatorClient.RequestAsync<AddResponse, AddRequest>("Add", request);
            AddResponse response = await Add.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} + {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorSubtractRequest()
        {
            SubtractRequest request = new SubtractRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            SubtractResponse response = await Subtract.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} - {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorMultiplyRequest()
        {
            MultiplyRequest request = new MultiplyRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue)
            };
            MultiplyResponse response = await Multiply.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} * {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }
        public async void ExecuteCalculatorDivideRequest()
        {
            DivideRequest request = new DivideRequest()
            {
                intA = UnityEngine.Random.Range(-byte.MaxValue, byte.MaxValue),
                intB = UnityEngine.Random.Range(1, byte.MaxValue / 16)
            };
            DivideResponse response = await Divide.Execute(_calculatorClient, request);
            if (response != null)
                _results.text = string.Format("{0} / {1} = {2}", request.intA, request.intB, response.Value) + Environment.NewLine;
        }

    }

}

namespace VRTX.Net.Samples.BLZService
{
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
    public class GetBank : SoapOperation<GetBankRequest, GetBankResponse, GetBank>
    {
        public GetBank() : base("getBank") { }
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
}


namespace VRTX.Net.Samples.Calculator
{

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

    #region Add Operation
    public class Add : SoapOperation<AddRequest, AddResponse, Add>
    {
        public Add() : base("Add")
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
    #endregion

    #region Subtract Operation
    public class Subtract : SoapOperation<SubtractRequest, SubtractResponse, Subtract>
    {
        public Subtract() : base("Subtract")
        { }
    }
    [XmlRoot(ElementName = "SubtractResponse", Namespace = "http://tempuri.org/")]
    public class SubtractResponse : SoapResponseType
    {
        [XmlElement("SubtractResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Subtract", Namespace = "http://tempuri.org/")]
    public class SubtractRequest : AddRequest
    { }
    #endregion

    #region Multiply Operation
    public class Multiply : SoapOperation<MultiplyRequest, MultiplyResponse, Multiply>
    {
        public Multiply() : base("Multiply")
        { }
    }
    [XmlRoot(ElementName = "MultiplyResponse", Namespace = "http://tempuri.org/")]
    public class MultiplyResponse : SoapResponseType
    {
        [XmlElement("MultiplyResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Multiply", Namespace = "http://tempuri.org/")]
    public class MultiplyRequest : AddRequest
    { }
    #endregion

    #region Divide Operation
    public class Divide : SoapOperation<DivideRequest, DivideResponse, Divide>
    {
        public Divide() : base("Divide")
        { }
    }
    [XmlRoot(ElementName = "DivideResponse", Namespace = "http://tempuri.org/")]
    public class DivideResponse : SoapResponseType
    {
        [XmlElement("DivideResult")]
        public int Value { get; set; }
    }
    [XmlRoot(ElementName = "Divide", Namespace = "http://tempuri.org/")]
    public class DivideRequest : AddRequest
    { }
    #endregion
}