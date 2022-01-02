using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenzHealth.DAL.Entity
{
    public class Menu
    {
        public Menu() { }
        public Menu(string url, string stringText, string icon, bool isMenu,string claim, List<Menu> childMenus)
        {
            _url = url;
            _stringText = stringText;
            _icon = icon;
            _childMenus = childMenus;
            _isMenu = isMenu;
            _claim = claim;
        }
        public string _url { get; set; }
        public string _stringText { get; set; }
        public string _icon { get; set; }
        public List<Menu> _childMenus { get; set; }
        public bool _isMenu { get; set; }
        public bool isAssigned { get; set; }
        public string _parent { get; set; }
        public string _claim { get; set; }
    }
}
