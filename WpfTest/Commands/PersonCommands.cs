using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WpfTest.Data;
using WpfTest.Enums;
using WpfTest.Models;
using WpfTest.ViewModels;

namespace WpfTest.Commands
{
    public class PersonCommands
    {
        private ICommand _deleteItemCommand;
        private ICommand _editItemCommand;
        private ICommand _addItemCommand;
        private ICommand _navigateDepartmentCommand;
        public ICommand DeleteItemCommand => _deleteItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                PersonViewModel personViewModel = (PersonViewModel)list.DataContext;

                if (MessageBox.Show(personViewModel.SelectedPerson.GetFullName, "Удалить сотрудника?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {
                    using (WorkDbContext db = new WorkDbContext())
                    {
                        db.Persons.Remove(personViewModel.SelectedPerson);
                        try
                        {
                            db.SaveChanges();
                            personViewModel.Persons.Remove(personViewModel.SelectedPerson);
                        }
                        catch (DbUpdateException ex)
                        {
                            var sqlException = ex.GetBaseException() as SqlException;

                            if (sqlException != null)
                            {
                                var number = sqlException.Number;

                                if (number == 547)
                                {
                                    MessageBox.Show("Сначала удалите связанные заказы");
                                }
                                else
                                {
                                    MessageBox.Show(sqlException.ToString());
                                }
                            }
                        }
                    }
                }
            }
        });

        public ICommand EditItemCommand => _editItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                PersonViewModel personViewModel = (PersonViewModel)list.DataContext;

                using (WorkDbContext db = new WorkDbContext())
                {
                    db.Persons.Update(personViewModel.TemporarySelectedPerson);
                    db.SaveChanges();
                }

                var item = personViewModel.Persons.FirstOrDefault(value => value.Id == personViewModel.TemporarySelectedPerson.Id);

                if (item != null)
                {
                    Person oldPerson = personViewModel.Persons[personViewModel.Persons.IndexOf(item)];
                    personViewModel.TemporarySelectedPerson.CopyTo(oldPerson);
                }
                else
                {
                    personViewModel.Persons.Add(personViewModel.TemporarySelectedPerson);
                    personViewModel.PersonCollectionView.MoveCurrentTo(personViewModel.TemporarySelectedPerson);
                }

                list.ScrollIntoView(list.Items[list.SelectedIndex]);
                list.Focus();
            }
        });
        public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                Person person = new Person() { LastName = "Заполните поля" };

                using (WorkDbContext db = new WorkDbContext())
                {
                    db.Persons.Add(person);
                    db.SaveChanges();
                }

                PersonViewModel personViewModel = (PersonViewModel)list.DataContext;
                personViewModel.Persons.Add(person);
                personViewModel.PersonCollectionView.MoveCurrentTo(person);
                list.ScrollIntoView(list.Items[list.SelectedIndex]);
                list.Focus();
            }
        });

        public ICommand NavigateDepartmentCommand => _navigateDepartmentCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is int id)
            {
                (Application.Current.MainWindow.DataContext as MainViewModel)?
                    .UpdateViewCommand.Execute(
                    new UpdateViewParam() { Id = id, ViewType = ViewType.Department });
            }
        });
    }
}
