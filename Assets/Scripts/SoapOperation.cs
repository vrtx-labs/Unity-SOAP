using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;


namespace VRTX.Net
{
    /// <summary>
    /// generic class which provides the base for implementation of specific SOAP operations using request and response types
    /// </summary>
    /// <typeparam name="Req">The <see cref="SoapRequestType>"/> type the operation uses to send to the service endpoint.</typeparam>
    /// <typeparam name="Res">The <see cref="SoapResponseType>"/> type the operation tries to retrieve from service endpoint</typeparam>
    /// <typeparam name="Self">The <see cref="SoapOperation{Req, Res, Self}"/> type used to create the derived types singleton instance. This should usually the derived type itself.</typeparam>
    public abstract class SoapOperation<Req, Res, Self> where Req : SoapRequestType where Res : SoapResponseType where Self : SoapOperation<Req, Res, Self>, new()
    {
        #region Singleton
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
        #endregion

        /// <summary>
        /// gets the endpoint used by the <see cref="SoapOperation{Req, Res, Self}"/> derived type.
        /// </summary>
        public string Endpoint { get; private set; } = string.Empty;
        /// <summary>
        /// gets or sets the <see cref="Req"/> instance used by the current instance of the <see cref="SoapOperation{Req, Res, Self}"/>. This is null for the singleton as it does not store request or response references.
        /// </summary>
        public Req Request { get; set; } = null;
        /// <summary>
        /// gets the <see cref="Res"/> instance retrieved by the current instance of the <see cref="SoapOperation{Req, Res, Self}"/>. This is null for the singleton as it does not store request or response references.
        /// </summary>
        public Res Response { get; private set; } = null;

        public SoapOperation()
        { this.Endpoint = "Unknown"; }
        protected SoapOperation(string endpoint)
        { this.Endpoint = endpoint; }


        /// <summary>
        /// executes the <see cref="SoapOperation{Req, Res, Self}"/> using the <see cref="Req"/> reference set by the property using the given <see cref="SoapClient"/>
        /// </summary>
        /// <param name="client">The soap client the operation is executed on.</param>
        /// <returns>The <see cref="Res"/> instance received from the web services' response.</returns>
        public async Task<Res> Execute(SoapClient client)
        {
            this.Response = await Execute(client, this.Endpoint, this.Request);
            return this.Response;
        }

        /// <summary>
        /// executes the <see cref="SoapOperation{Req, Res, Self}"/> using the <see cref="Req"/> reference provided as a parameter using the given <see cref="SoapClient"/>
        /// </summary>
        /// <param name="client">The soap client the operation is executed on.</param>
        /// <param name="request">The <see cref="Req"/> used for this operations execution.</param>       
        /// <returns>The <see cref="Res"/> instance received from the web services' response.</returns>
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