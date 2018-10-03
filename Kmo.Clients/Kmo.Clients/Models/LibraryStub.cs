using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kmo.Clients.Models
{
    [Serializable]
    public class LibraryStub
    {
        public string AssemblyName { get; set; }
        public Guid DesktopLibraryId { get; set; }
        public int DesktopLibraryRevisionId { get; set; }
        public string FileExtension { get; set; }
        public string Md5Hash { get; set; }
        public string RevisionNote { get; set; }
        public string UsersId { get; set; }
        public byte[] BinaryData { get; set; }
    }
}
