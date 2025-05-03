using KarateSystem.Misc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KarateSystem.Misc.Helper;

namespace KarateSystem.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public List<GenderOption> GenderOptions => Helper.GenderOptions;
        public List<GenderOption2> GenderOptions2 => Helper.GenderOptions2;
        public List<StatusOption> StatusOptions => Helper.StatusOptionsList;
        public List<RoleOption> RoleOptions => Helper.RoleOptionsList;
        public List<OvertimePlaceOption> OvertimeOptions => Helper.OvertimePlaceList;
        public List<WalkoverOption> WalkoverOptions => Helper.WalkoverOptionsList;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
