using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kmo.Web.Models
{
    public class MenuViewModels
    {
        public string Name { get; set; }

        public string Route { get; set; }
        
        public string MenuPath { get; set; }
    }

    public class MenuNodes
    {
        List<MenuNodes> _childNodes;

        public MenuNodes()
        {
            _childNodes = new List<MenuNodes>();
        }

        public string Name { get; set; }

        public bool IsFolder { get; set; }

        public MenuNodes ParentNode { get; set; }

        public int Level { get; set; }

        public string Icon { get; set; }

        public string Path { get; set; }

        public List<MenuNodes> ChildNodes
        {
            get
            {
                return _childNodes;
            }
        }



    }
}