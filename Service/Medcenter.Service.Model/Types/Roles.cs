using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    public class Role
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
    public class RolesCollection
    {
        public RolesCollection(ObservableCollection<string> keys)
        {
            Roles = new ObservableCollection<Role>
            {
                new Role {Key = "Admin", Value = "Администратор", IsChecked = false},
                new Role {Key = "Manager", Value = "Менеджер", IsChecked = false},
                new Role {Key = "Nurse", Value = "Медсестра", IsChecked = false},
                new Role {Key = "Owner", Value = "Владелец", IsChecked = false},
                new Role {Key = "Doctor", Value = "Доктор", IsChecked = false}
            };
            SetCheckedAll(keys);
        }
        public string GetValueByKey(string key)
        {
            return Roles.Single(x => x.Key == key).Value;
        }
        public Role GetRoleByKey(string key)
        {
            return Roles.Single(x => x.Key == key);
        }

        public ObservableCollection<Role> Roles {get; set; }

        public ObservableCollection<string> RolesValues
        {
            get
            {
                var coll = new ObservableCollection<string>();
                foreach (var role in Roles)
                {
                    if (role.IsChecked) coll.Add(role.Value);
                }
                return coll;
            }
        }
        public ObservableCollection<string> RolesKeys
        {
            get
            {
                var coll = new ObservableCollection<string>();
                foreach (var role in Roles)
                {
                    if (role.IsChecked) coll.Add(role.Key);
                }
                return coll;
            }
        }
        public void SetChecked(string key)
        {
            GetRoleByKey(key).IsChecked = true;
        }
        public void SetCheckedAll(ObservableCollection<string> keys)
        {

            UncheckAll();
            if (keys != null)
            {
                foreach (var r in keys)
                {
                    SetChecked(r);
                }
            }
        }
        public void UncheckAll()
        {
            foreach (var r in Roles)
            {
                r.IsChecked = false;
            }
        }
    }
}
