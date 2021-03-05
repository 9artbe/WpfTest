using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using WpfTest.Annotations;

namespace WpfTest.Models
{
    public class Person:INotifyPropertyChanged
    {
        private string _firstName;
        private string _secondName;
        private DateTime? _birthDate;
        private GenderType? _gender;
        private string _lastName;
        private int? _departmentId;
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

        public string LastName
        {
            get => _lastName;
            set
            {
                if (value == _lastName) return;
                _lastName = value;
                OnPropertyChanged();
            }
        }
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (value == _firstName) return;
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string SecondName
        {
            get => _secondName;
            set
            {
                if (value == _secondName) return;
                _secondName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? BirthDate
        {
            get => _birthDate;
            set
            {
                if (Nullable.Equals(value, _birthDate)) return;
                _birthDate = value;
                OnPropertyChanged();
            }
        }

        public GenderType? Gender
        {
            get => _gender;
            set
            {
                if (value == _gender) return;
                _gender = value;
                OnPropertyChanged();
            }
        }

        public int? DepartmentId
        {
            get => _departmentId;
            set
            {
                if (value == _departmentId) return;
                _departmentId = value;
                OnPropertyChanged();
            }
        }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public string GetFullName => $"{_lastName} {_firstName} {_secondName}";

        public void CopyTo(Person target)
        {
            if (target == null)
                throw new ArgumentNullException();

            target.FirstName = this.FirstName;
            target.SecondName = this.SecondName;
            target.LastName = this.LastName;
            target.BirthDate = this.BirthDate;
            target.DepartmentId = this.DepartmentId;
            target.Gender = this.Gender;
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