using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Services.Protocols;
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

        [OperationContract, XmlSerializerFormat]
        [SoapDocumentMethod(Use =System.Web.Services.Description.SoapBindingUse.Literal)]
        System.Xml.XmlDocument GetCapabilitiesRaw();

        [OperationContract]
        BinaryResponse GetMap(string[] layers, string[] styles, BoundingBox bounds, int width, int height, string crs, string format);

        [OperationContract]
        BinaryResponse PostMap(OGC.WMS.SOAP.SLD.GetMapType getMapSld);

        [OperationContract]
        [SoapDocumentMethod(Use = System.Web.Services.Description.SoapBindingUse.Literal)]
        BinaryResponse PostMapRaw(System.Xml.XmlElement getMapSld);

    }

    static class Helper
    {
        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            System.Collections.Generic.List<System.Type> knownTypes =
                new System.Collections.Generic.List<System.Type>();
            string nmspc = "OGC.WMS.SOAP.SLD";
            var q = (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.Namespace == nmspc
                    select t).ToList();
            knownTypes.AddRange(q);
            // Add any types to include here.
            /*knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.NamedLayer));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.UserLayer));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.NamedStyle));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.EnvelopeType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.ItemChoiceType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.VersionType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.OutputType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.CoordType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AffinePlacementType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AbsoluteExternalPositionalAccuracyType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.actuateType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AesheticCriteriaType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AnchorPointType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AngleChoiceType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.AngleType));
            knownTypes.Add(typeof(OGC.WMS.SOAP.SLD.animateColorPrototype));*/
            return knownTypes;
        }
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.

}
