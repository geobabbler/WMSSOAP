using System;
using System.Collections.Generic;
using System.Drawing;
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
        private const string mMapRequest = "GetMap";
        private const string capabilitiesTemplate = "SERVICE={0}&VERSION={1}&REQUEST={2}";
        private const string mapTemplate = "SERVICE={0}&VERSION={1}&REQUEST={2}&layers={3}&styles={4}&bbox={5}&width={6}&height={7}&srs={8}&format={9}";

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

        public BinaryResponse GetMap(string[] layers, string[] styles, BoundingBox bounds, int width, int height, string crs, string format)
        {
            BinaryResponse retval = new BinaryResponse();
            string lyrs = string.Join(",", layers);
            string styls = styles.Length > 0 ? string.Join(",", styles) : "";
            string bbox = bounds.minx.ToString() + "," + bounds.miny.ToString() + "," + bounds.maxx.ToString() + "," + bounds.maxy.ToString();
            HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(System.Configuration.ConfigurationManager.AppSettings["baseUrl"] + string.Format(mapTemplate, mService, mVersion, mMapRequest, lyrs, styls, bbox, width.ToString(), height.ToString(), crs, format));
            Stream responseStream = null;
            MemoryStream ms;

            try
            {

                using (var httpResponse = (HttpWebResponse)httpRequest.GetResponse())
                {
                    using (responseStream = httpResponse.GetResponseStream())
                    {
                        ms = new MemoryStream();

                        byte[] buffer = new byte[1024];
                        int byteCount;
                        do
                        {
                            byteCount = responseStream.Read(buffer, 0, buffer.Length);
                            ms.Write(buffer, 0, byteCount);
                        } while (byteCount > 0);
                        ms.Position = 0;
                        retval.ContentType = format;
                        retval.BinaryPayload = ms;
                        return retval;
                    }


                    //using (var streamRdr = new StreamReader(responseStream))
                    //{
                    //    var response = streamRdr.ReadToEnd();

                    //    httpResponse.Close();

                    //    return LoadFromString(response);
                    //}
                }

            }
            catch(System.Exception ex)
            {
                return null;
            }
            finally
            {
                //if (responseStream != null)
                //{
                //    responseStream.Dispose();
                //}
            }





            //Bitmap bitmap = new Bitmap(width, height);
            //for (int i = 0; i < bitmap.Width; i++)
            //{
            //    for (int j = 0; j < bitmap.Height; j++)
            //    {
            //        bitmap.SetPixel(i, j, (Math.Abs(i - j) < 2) ? Color.Blue : Color.Yellow);
            //    }
            //}
            //MemoryStream ms = new MemoryStream();
            //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //ms.Position = 0;
            //WebOperationContext.Current.OutgoingResponse.ContentType = format;
            //return ms;
        }
    }
}
