using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace TransportApiSharpSample.Models
{
    [XmlRoot("travelinedata")]
    public class TravelineOperatorCodes
    {
        [XmlAttribute("reportName")]
        public string ReportName { get; set; }

        [XmlAttribute("generationDate")]
        public DateTime GenerationDate { get; set; }

        [XmlElement("nocrecord")]
        public List<NocRecord> NocRecords { get; set; }
    }

    public class NocRecord
    {
        [XmlElement("NOCCODE")]
        public string NocCode { get; set; }

        [XmlElement("OpNm")]
        public string OperatorName { get; set; }

        [XmlElement("OperatorPublicName")]
        public string PublicName { get; set; }
    }
}