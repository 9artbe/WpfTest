using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfTest.Annotations;

namespace WpfTest.Models
{
    public class Order : INotifyPropertyChanged
    {
        private string _orderNumber;
        private string _contractorName;
        private DateTime? _orderDate;
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

        public string OrderNumber
        {
            get => _orderNumber;
            set
            {
                if (value == _orderNumber) return;
                _orderNumber = value;
                OnPropertyChanged();
            }
        }

        public string ContractorName
        {
            get => _contractorName;
            set
            {
                if (value == _contractorName) return;
                _contractorName = value;
                OnPropertyChanged();
            }
        }

        public DateTime? OrderDate
        {
            get => _orderDate;
            set
            {
                if (Nullable.Equals(value, _orderDate)) return;
                _orderDate = value;
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

        public virtual Person Person { get; set; }


        public string GetFullOrderName =>
            $"Заказ №{OrderNumber} {(OrderDate == null ? "" : " от " + OrderDate.Value.ToShortDateString())}";


         public void CopyTo(Order target)
        {
            if (target == null)
                throw new ArgumentNullException();

            target.ContractorName = this.ContractorName;
            target.OrderDate = this.OrderDate;
            target.OrderNumber = this.OrderNumber;
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
