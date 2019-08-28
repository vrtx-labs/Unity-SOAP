using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace VRTX.Net
{
    public class SoapOperation<Req, Res, Self> where Req : SoapRequestType where Res : SoapResponseType where Self : SoapOperation<Req, Res, Self>, new()
    {
        protected static Self _instance = null;
        public static Self Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Self();
                return _instance;
            }
        }
        public string Endpoint { get; private set; } = string.Empty;
        public Req Request { get; set; } = null;
        public Res Response { get; private set; } = null;

        public SoapOperation()
        { this.Endpoint = "Unknown"; }
        protected SoapOperation(string endpoint)
        { this.Endpoint = endpoint; }

        public async Task<Res> Execute(SoapClient client)
        {
            return await Execute(client, this.Endpoint, this.Request);
        }
        public static async Task<Res> Execute(SoapClient client, Req request)
        {
            return await Execute(client, Instance.Endpoint, request);
        }
        private static async Task<Res> Execute(SoapClient client, string endpoint, Req request)
        {
            if (string.IsNullOrEmpty(endpoint))
                throw new ArgumentException("The provided endpoint is empty. Please specify a valid endpoint name to execute the operation.", "endpoint");
            if (client == null)
                throw new ArgumentException("The provided client reference is null. Please specify a client reference to execute the operation.", "client");
            if (request == null)
                throw new ArgumentException("The provided request is null. Please provide a valid request reference to execute the operation.", "request");
            // execute the request as the checks before should ensure all parameters are at least set.
            return await client.RequestAsync<Res, Req>(endpoint, request);
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