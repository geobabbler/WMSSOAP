using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace OGC.WMS.SOAP
{
    [DataContract]
    [KnownType(typeof(MemoryStream))]
    [KnownType(typeof(Stream))]
    public class BinaryResponse
    {
        [DataMember]
        public string ContentType { get; set; }

        [DataMember]
        public Stream BinaryPayload { get; set; }
    }
}