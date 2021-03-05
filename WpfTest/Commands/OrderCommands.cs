using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfTest.Data;
using WpfTest.Enums;
using WpfTest.Models;
using WpfTest.ViewModels;

namespace WpfTest.Commands
{
    public class OrderCommands
    {
        private ICommand _deleteItemCommand;
        private ICommand _editItemCommand;
        private ICommand _addItemCommand;
        private ICommand _navigatePersonCommand;

        public ICommand DeleteItemCommand => _deleteItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                OrderViewModel orderViewModel = (OrderViewModel)list.DataContext;

                if (MessageBox.Show(orderViewModel.SelectedOrder.GetFullOrderName, "Удалить заказ?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    using (WorkDbContext db = new WorkDbContext())
                    {
                        db.Orders.Remove(orderViewModel.SelectedOrder);
                        db.SaveChanges();
                    }
                    orderViewModel.Orders.Remove(orderViewModel.SelectedOrder);
                }
            }
        });

        public ICommand EditItemCommand => _editItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                OrderViewModel orderViewModel = (OrderViewModel)list.DataContext;

                using (WorkDbContext db = new WorkDbContext())
                {
                    db.Orders.Update(orderViewModel.TemporarySelectedOrder);
                    db.SaveChanges();
                }

                var item = orderViewModel.Orders.FirstOrDefault(value => value.Id == orderViewModel.TemporarySelectedOrder.Id);

                if (item != null)
                {
                    Order oldOrder = orderViewModel.Orders[orderViewModel.Orders.IndexOf(item)];
                    orderViewModel.TemporarySelectedOrder.CopyTo(oldOrder);
                }
                else
                {
                    orderViewModel.Orders.Add(orderViewModel.TemporarySelectedOrder);
                    orderViewModel.OrderCollectionView.MoveCurrentTo(orderViewModel.TemporarySelectedOrder);
                }

                list.ScrollIntoView(list.Items[list.SelectedIndex]);
                list.Focus();
            }
        });
        public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                Order order = new Order() { OrderNumber = "Введите номер", OrderDate = DateTime.Now};

                using (WorkDbContext db = new WorkDbContext())
                {
                   db.Orders.Add(order);
                   db.SaveChanges();
                }

                OrderViewModel orderViewModel = (OrderViewModel)list.DataContext;
                orderViewModel.Orders.Add(order);
                orderViewModel.OrderCollectionView.MoveCurrentTo(order);
                list.ScrollIntoView(list.Items[list.SelectedIndex]);
                list.Focus();
            }
        });

        public ICommand NavigatePersonCommand => _navigatePersonCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is int id)
            {
                (Application.Current.MainWindow.DataContext as MainViewModel)?
                    .UpdateViewCommand.Execute(
                        new UpdateViewParam() { Id = id, ViewType = ViewType.Person });
            }
        });
    }
}
