using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kmo.Clients.Models
{
    [Serializable]
    public partial class vi_UsersModule
    {

        private string _UsersId;

        private string _FullName;

        private char _UserType;

        private bool _Admin;

        private bool _Active;

        private int _DesktopModulesId;

        private string _ModuleName;

        private bool _ModuleAllowed;

        private int _ModuleRevisionId;

        private System.Guid _BinaryDataId;

        private string _FileName;

        private string _FileExtension;

        private string _ModuleStructure;

        private string _DisplayName;


        public vi_UsersModule()
        {
        }

        public string UsersId
        {
            get
            {
                return this._UsersId;
            }
            set
            {
                this._UsersId = value;
            }
        }

        public string FullName
        {
            get
            {
                return this._FullName;
            }
            set
            {
                this._FullName = value;
            }
        }

        public char UserType
        {
            get
            {
                return this._UserType;
            }
            set
            {
                this._UserType = value;
            }
        }

        public bool Admin
        {
            get
            {
                return this._Admin;
            }
            set
            {
                this._Admin = value;
            }
        }

        public bool Active
        {
            get
            {
                return this._Active;
            }
            set
            {
                this._Active = value;
            }
        }

        public int DesktopModulesId
        {
            get
            {
                return this._DesktopModulesId;
            }
            set
            {
                this._DesktopModulesId = value;
            }
        }

        public string ModuleName
        {
            get
            {
                return this._ModuleName;
            }
            set
            {
                this._ModuleName = value;
            }
        }

        public bool ModuleAllowed
        {
            get
            {
                return this._ModuleAllowed;
            }
            set
            {
                this._ModuleAllowed = value;
            }
        }

        public int ModuleRevisionId
        {
            get
            {
                return this._ModuleRevisionId;
            }
            set
            {
                this._ModuleRevisionId = value;
            }
        }

        public System.Guid BinaryDataId
        {
            get
            {
                return this._BinaryDataId;
            }
            set
            {
                this._BinaryDataId = value;
            }
        }

        public string FileName
        {
            get
            {
                return _FileName;
            }
            set
            {
                _FileName = value;
            }
        }

        public string FileExtension
        {
            get
            {
                return _FileExtension;
            }
            set
            {
                _FileExtension = value;
            }
        }

        public string ModuleStructure
        {
            get
            {
                return _ModuleStructure;
            }
            set
            {
                _ModuleStructure = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
            }
        }

        
    }
}
