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
    public class DepartmentCommands
    {
        private ICommand _deleteItemCommand;
        private ICommand _editItemCommand;
        private ICommand _addItemCommand;
        private ICommand _navigatePersonCommand;

        public ICommand DeleteItemCommand => _deleteItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                DepartmentViewModel departmentViewModel = (DepartmentViewModel)list.DataContext;

                if (MessageBox.Show(departmentViewModel.SelectedDepartment.Name, "Удалить подразделение?",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
                {

                    using (WorkDbContext db = new WorkDbContext())
                    {
                        db.Departments.Remove(departmentViewModel.SelectedDepartment);
                        try
                        {
                            db.SaveChanges();
                            departmentViewModel.Departments.Remove(departmentViewModel.SelectedDepartment);
                        }
                        catch (DbUpdateException ex)
                        {
                            var sqlException = ex.GetBaseException() as SqlException;

                            if (sqlException != null)
                            {
                                var number = sqlException.Number;

                                if (number == 547)
                                {
                                    MessageBox.Show("Сначала удалите сотрудников из отдела");
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
                DepartmentViewModel departmentViewModel = (DepartmentViewModel)list.DataContext;

                using (WorkDbContext db = new WorkDbContext())
                {
                    db.Departments.Update(departmentViewModel.TemporarySelectedDepartment);
                    db.SaveChanges();
                }

                var item = departmentViewModel.Departments.FirstOrDefault(value => value.Id == departmentViewModel.TemporarySelectedDepartment.Id);

                if (item != null)
                {
                    Department oldDepartment = departmentViewModel.Departments[departmentViewModel.Departments.IndexOf(item)];
                    departmentViewModel.TemporarySelectedDepartment.CopyTo(oldDepartment);
                }
                else
                {
                    departmentViewModel.Departments.Add(departmentViewModel.TemporarySelectedDepartment);
                    departmentViewModel.DepartmentCollectionView.MoveCurrentTo(departmentViewModel.TemporarySelectedDepartment);
                }

                list.ScrollIntoView(list.Items[list.SelectedIndex]);
                list.Focus();
            }
        });
        public ICommand AddItemCommand => _addItemCommand ??= new RelayCommand(parameter =>
        {
            if (parameter is ListView list)
            {
                Department department = new Department() { Name = "Введите название" };
                
                using (WorkDbContext db = new WorkDbContext())
                {
                    db.Departments.Add(department);
                    db.SaveChanges();
                }

                DepartmentViewModel departmentViewModel = (DepartmentViewModel)list.DataContext;
                departmentViewModel.Departments.Add(department);
                departmentViewModel.DepartmentCollectionView.MoveCurrentTo(department);
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
