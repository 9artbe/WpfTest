using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WpfTest.Annotations;

namespace WpfTest.Models
{
    public class Department : INotifyPropertyChanged
    {
        private string _name;
        private int? _personId;
        private int _id;

        public int Id
        {
            get => _id;
            set
            {
                if (value == _id) return;
                _id = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public int? PersonId
        {
            get => _personId;
            set
            {
                if (value == _personId) return;
                _personId = value;
                OnPropertyChanged();
            }
        }

        [ForeignKey("PersonId")]
        public virtual Person Person { get; set; }

        public void CopyTo(Department target)
        {
            if (target == null)
                throw new ArgumentNullException();

            target.Name = this.Name;
            target.PersonId = this.PersonId;
            target.Id = this.Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
