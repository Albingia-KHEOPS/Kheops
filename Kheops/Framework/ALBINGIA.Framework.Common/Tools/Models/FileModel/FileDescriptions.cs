using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;

namespace ALBINGIA.Framework.Common.Models.FileModel
{
    [DataContract]
    public class FileDescription
    {
        [DataMember]
        [Column(Name="LIBDOC")]
        public string LibDoc { get; set; }
        [DataMember]
        [Column(Name="NOMDOC")]        
        public string Name { get; set; }
        [DataMember]
        [Column(Name = "CHEMINDOC")]
        public string FullName { get; set; }
        public string WebPath { get; set; }
        //[DataMember]
        //[Column(Name = "IDDOC")]
        //public Int64 IdDoc { get; set; }
        [DataMember]
        public long Size { get; set; }
        public DateTime DateCreated { get; set; }
        public bool ReturnHtmlTable { get; set; }
        [DataMember]
        public string FilePath { get; set; }
        [DataMember]
        public string Extension { get; set; }
    }

    public class FileDescriptions
    {
        public List<FileDescription> ListFileDescription { get; set; }
    }
}
