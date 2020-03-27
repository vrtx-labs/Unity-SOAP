
using System.Xml.Serialization;

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