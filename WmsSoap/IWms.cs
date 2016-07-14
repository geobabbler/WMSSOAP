using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using OGC.WMS.SOAP.v1_3_0;

namespace OGC.WMS.SOAP
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceKnownType("GetKnownTypes", typeof(Helper))]
    [ServiceContract (Namespace = "http://schemas.opengis.net/wms/soap")]
    public interface IWms
    {

        [OperationContract]
        string GetVersion();

        [OperationContract]
        WMS_Capabilities GetCapabilities();

        [OperationContract]
        BinaryResponse GetMap(string[] layers, string[] styles, BoundingBox bounds, int width, int height, string crs, string format);

        [OperationContract]
        BinaryResponse PostMap(OGC.WMS.SOAP.SLD.GetMapType getMapSld);

     }

    static class Helper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            System.Collections.Generic.List<System.Type> knownTypes =
                new System.Collections.Generic.List<System.Type>();
            // Add any types to include here.
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.NamedLayer));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.UserLayer));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.NamedStyle));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.EnvelopeType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.ItemChoiceType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.VersionType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.OutputType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.CoordType));
            return knownTypes;
        }
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

}
