using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using WpfTest.Commands;
using WpfTest.Data;
using WpfTest.Models;

namespace WpfTest.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        public ObservableCollection<Order> Orders { get; }
        public List<Person> Persons { get; }
        public OrderCommands OrderCommands { get; }
        public Order TemporarySelectedOrder { get; }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set
            {
                _selectedOrder = value;

                if (value != null)
                    value.CopyTo(TemporarySelectedOrder);

                OnPropertyChanged();
            }
        }

        public ICollectionView OrderCollectionView { get; }

        public OrderViewModel()
        {
            OrderCommands = new OrderCommands();

            using (WorkDbContext db = new WorkDbContext())
            {
                Orders = new ObservableCollection<Order>();

                foreach (var Order in db.Orders)
                {
                    Orders.Add(Order);
                }

                Persons = new List<Person>();
                foreach (var person in db.Persons)
                {
                    Persons.Add(person);
                }
            }

            OrderCollectionView = CollectionViewSource.GetDefaultView(Orders);
            TemporarySelectedOrder = new Order();
        }
    }
}


