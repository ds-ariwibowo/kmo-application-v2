using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Kmo.Clients.Models
{
    public class vi_DesktopLibrary
    {
        private Guid _DesktopLibraryId;

        private string _DesktopLibraryName;

        private bool _GroupedLibs;

        private bool _StartingPoint;

        private string _AssemblyName;

        private string _FileExtension;

        private int _DesktopLibraryRevisionId;

        private string _RevisionNote;

        private string _UsersId;
        
        private Guid _BinaryDataId;

        public vi_DesktopLibrary()
        {
        }

        public Guid DesktopLibraryId
        {
            get
            {
                return this._DesktopLibraryId;
            }
            set
            {
                if ((this._DesktopLibraryId != value))
                {
                    this._DesktopLibraryId = value;
                }
            }
        }

        public string DesktopLibraryName
        {
            get
            {
                return this._DesktopLibraryName;
            }
            set
            {
                if ((this._DesktopLibraryName != value))
                {
                    this._DesktopLibraryName = value;
                }
            }
        }

        public bool GroupedLibs
        {
            get
            {
                return this._GroupedLibs;
            }
            set
            {
                if ((this._GroupedLibs != value))
                {
                    this._GroupedLibs = value;
                }
            }
        }

        public bool StartingPoint
        {
            get
            {
                return this._StartingPoint;
            }
            set
            {
                if ((this._StartingPoint != value))
                {
                    this._StartingPoint = value;
                }
            }
        }

        public string AssemblyName
        {
            get
            {
                return this._AssemblyName;
            }
            set
            {
                if ((this._AssemblyName != value))
                {
                    this._AssemblyName = value;
                }
            }
        }

        public string FileExtension
        {
            get
            {
                return this._FileExtension;
            }
            set
            {
                if ((this._FileExtension != value))
                {
                    this._FileExtension = value;
                }
            }
        }

        public int DesktopLibraryRevisionId
        {
            get
            {
                return this._DesktopLibraryRevisionId;
            }
            set
            {
                if ((this._DesktopLibraryRevisionId != value))
                {
                    this._DesktopLibraryRevisionId = value;
                }
            }
        }

        public string RevisionNote
        {
            get
            {
                return this._RevisionNote;
            }
            set
            {
                if ((this._RevisionNote != value))
                {
                    this._RevisionNote = value;
                }
            }
        }

        public string UsersId
        {
            get
            {
                return this._UsersId;
            }
            set
            {
                if ((this._UsersId != value))
                {
                    this._UsersId = value;
                }
            }
        }

        public Guid BinaryDataId
        {
            get
            {
                return this._BinaryDataId;
            }
            set
            {
                if ((this._BinaryDataId != value))
                {
                    this._BinaryDataId = value;
                }
            }
        }


    }
}
