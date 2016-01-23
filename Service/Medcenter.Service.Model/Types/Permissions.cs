using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medcenter.Service.Model.Types
{
    public class Permission
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
    public class PermissionsCollection
    {
        public PermissionsCollection(ObservableCollection<string> keys)
        {
            Permissions = new ObservableCollection<Permission>
            {
                new Permission {Key = "CanEditMainLists", Value = "Редактировать списки", IsChecked = false},
                new Permission {Key = "CanGetDailyBalance", Value = "Смотреть баланс текущего дня", IsChecked = false},
                new Permission {Key = "CanGetFinancialInfo", Value = "Получать финансовую информацию", IsChecked = false}
            };
            SetCheckedAll(keys);
        }
        public string GetValueByKey(string key)
        {
            return Permissions.Single(x => x.Key == key).Value;
        }
        public Permission GetPermissionByKey(string key)
        {
            return Permissions.Single(x => x.Key == key);
        }

        public ObservableCollection<Permission> Permissions { get; set; }

        public ObservableCollection<string> PermissionsValues
        {
            get
            {
                var coll = new ObservableCollection<string>();
                foreach (var permission in Permissions)
                {
                    if (permission.IsChecked) coll.Add(permission.Value);
                }
                return coll;
            }
        }
        public ObservableCollection<string> PermissionsKeys
        {
            get
            {
                var coll = new ObservableCollection<string>();
                foreach (var permission in Permissions)
                {
                    if (permission.IsChecked) coll.Add(permission.Key);
                }
                return coll;
            }
        }
        public void SetChecked(string key)
        {
            GetPermissionByKey(key).IsChecked = true;
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
            foreach (var r in Permissions)
            {
                r.IsChecked = false;
            }
        }
    }
}
