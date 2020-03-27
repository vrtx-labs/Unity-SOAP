
using System.Xml.Serialization;

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

