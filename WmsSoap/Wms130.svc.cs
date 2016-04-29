using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OGC.WMS.SOAP.v1_3_0;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace OGC.WMS.SOAP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Wms130 : IWms
    {
        private const string mVersion = "1.3.0";
        private const string mService = "WMS";
        private const string mCapRequest = "GetCapabilities";
        private const string capabilitiesTemplate = "SERVICE={0}&VERSION={1}&REQUEST={2}";

        public WMS_Capabilities GetCapabilities()
        {
            WMS_Capabilities retval = null;
            try
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + string.Format(capabilitiesTemplate, mService, mVersion, mCapRequest));
                Stream responseStream = null;
                try
                {

                    using (var httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                    {
                        responseStream = httpResponse.GetResponseStream();

                        if (responseStream == null)
                        {
                            return null;
                        }

                        using (var streamRdr = new StreamReader(responseStream))
                        {
                            var response = streamRdr.ReadToEnd();

                            httpResponse.Close();

                            return LoadFromString(response);
                        }
                    }

                }
                finally
                {
                    if (responseStream != null)
                    {
                        responseStream.Dispose();
                    }
                }
            }
            catch(System.Exception ex)
            {
                return retval;
            }
            return retval;
        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string GetVersion()
        {
            return mVersion;
        }

        private WMS_Capabilities LoadFromString(string response)
        {
            if (string.IsNullOrEmpty(response))
                return null;


            try
            {
                WMS_Capabilities result = null;
                using (var stringReader = new StringReader(response))
                {
                    using (XmlReader reader = new XmlTextReader(stringReader))
                    {
                        var serializer = new XmlSerializer(typeof(WMS_Capabilities));
                        result = serializer.Deserialize(reader) as WMS_Capabilities;
                    }
                }

                return result;
            }
            catch (System.Exception exception)
            {
                //Log the exception, etc.
                return null;
            }

        }

    }
}
