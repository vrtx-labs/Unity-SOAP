using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace VRTX.Net
{
    public class SoapClient
    {
        protected XNamespace _xns = "http://schemas.xmlsoap.org/soap/envelope/"; // should never be subject to change
        protected XNamespace _xsi = "http://www.w3.org/2001/XMLSchema-instance"; // should never be subject to change
        protected XNamespace _xsd = "http://www.w3.org/2001/XMLSchema"; // should never be subject to change
        protected XNamespace _xsvc = string.Empty; // "http://thomas-bayer.com/blz/"; // needs to be set for the soap service which should be used
        protected TimeSpan _timeout = new TimeSpan(0, 0, 60);
        protected string _apiURL = string.Empty;

        private HttpClient _httpClient = null;
        protected HttpClient HttpClient
        {
            get
            {
                if (_httpClient == null)
                {
                    _httpClient = new HttpClient(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip }) { Timeout = _timeout };
                }
                return _httpClient;
            }
        }

        // ctor
        public SoapClient(string svcApiUrl, XNamespace svcNamespace)
        {
            this._apiURL = svcApiUrl;
            this._xsvc = svcNamespace;
        }


        public virtual async Task<T> RequestAsync<T, U>(string endPoint, U request) where T : SoapResponseType where U : SoapRequestType
        {
            SoapResponse soapResponse = await this.SendRequest(endPoint, request);
            if (soapResponse != null)
            {
                int status = (int)soapResponse.StatusCode;
                if (status >= 200 && status < 300)
                {
                    T response = soapResponse.GetResponseType<T>();
                    return response;
                }
            }
            return default(T);
        }

        public virtual XDocument SoapRequest<T>(string endPoint, T endPointParameters)
        {
            XDocument soapRequest = new XDocument(
                new XDeclaration("1.0", "UTF-8", "no"),
                new XElement(_xns + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "xsi", _xsi),
                    new XAttribute(XNamespace.Xmlns + "xsd", _xsd),
                    new XAttribute(XNamespace.Xmlns + "soap", _xns),
                    new XElement(_xns + "Body",
                        SoapUtilities.Serialize(endPointParameters)
                    )
                ));
            return soapRequest;
        }

        public virtual async Task<SoapResponse> SendRequest<T>(string endPoint, T endPointParameters)
        {
            XDocument soapRequest = this.SoapRequest(endPoint, endPointParameters);

            try
            {
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(_apiURL),
                    Method = HttpMethod.Post
                };

                request.Content = new StringContent(soapRequest.ToString(), Encoding.UTF8, "text/xml");
                request.Headers.Clear();
                this.HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("text/xml");
                request.Headers.Add("SOAPAction", _xsvc.NamespaceName + endPoint); // can this one really be left empty? contained: "http://mynamespace.com/GetStuff";

                HttpResponseMessage response = await this.HttpClient.SendAsync(request);
                return await SoapResponse.Retrieve(response);
            }
            catch (AggregateException ex)
            {
                if (ex.InnerException is TaskCanceledException)
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }


    public class SoapResponse
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.OK;
        public string ReasonPhrase { get; } = string.Empty;
        public XDocument XmlDocument { get; private set; } = null;

        private SoapResponse(HttpStatusCode statusCode, string reasonPhrase, XDocument xmlDocument)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            XmlDocument = xmlDocument;
        }

        public T GetResponseType<T>() where T : SoapResponseType
        {
            if (this.XmlDocument != null)
            {
                XName fullQualifiedElementName = SoapResponseType.GetFullyQualifiedElementName<T>();
                //XElement complexTypeElement = this.XmlDocument.Descendants(myns + "details").FirstOrDefault();
                XElement complexTypeElement = this.XmlDocument.Descendants(fullQualifiedElementName).FirstOrDefault();
                T result = SoapUtilities.Deserialize<T>(complexTypeElement.ToString());
                return result;
            }
            return default(T);
        }


        public static async Task<SoapResponse> Retrieve(HttpResponseMessage response)
        {
            SoapResponse result = new SoapResponse(response.StatusCode, response.ReasonPhrase, null);
            if (response.IsSuccessStatusCode)
            {
                Stream stream = await response.Content.ReadAsStreamAsync();
                using (StreamReader sr = new StreamReader(stream))
                {
                    result.XmlDocument = XDocument.Load(sr);
                }
            }
            return result;
        }
    }

    public class SoapResponseType
    {
        public static XName GetFullyQualifiedElementName<T>() where T : SoapResponseType
        {
            Type t = typeof(T);
            object[] value = t.GetCustomAttributes(typeof(XmlRootAttribute), true);
            if (value.Length > 0)
            {
                XmlRootAttribute xmlRootAttr = value[0] as XmlRootAttribute;
                if (xmlRootAttr != null)
                {
                    XNamespace ns = (XNamespace)xmlRootAttr.Namespace;
                    return XName.Get(xmlRootAttr.ElementName, xmlRootAttr.Namespace);
                }
            }
            return string.Empty;

        }
    }
    public class SoapRequestType
    {
        public static XName GetFullyQualifiedElementName<T>() where T : SoapRequestType
        {
            Type t = typeof(T);
            object[] value = t.GetCustomAttributes(typeof(XmlRootAttribute), true);
            if (value.Length > 0)
            {
                XmlRootAttribute xmlRootAttr = value[0] as XmlRootAttribute;
                if (xmlRootAttr != null)
                {
                    XNamespace ns = (XNamespace)xmlRootAttr.Namespace;
                    return XName.Get(xmlRootAttr.ElementName, xmlRootAttr.Namespace);
                }
            }
            return string.Empty;

        }
    }


}