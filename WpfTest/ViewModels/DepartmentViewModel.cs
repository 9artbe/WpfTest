using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using WpfTest.Commands;
using WpfTest.Data;
using WpfTest.Models;

namespace WpfTest.ViewModels
{
    public class DepartmentViewModel : BaseViewModel
    {
        public ObservableCollection<Department> Departments { get; }
        public List<Person> Persons { get; }
        public DepartmentCommands DepartmentCommands { get; }
        public Department TemporarySelectedDepartment { get; }

        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get { return _selectedDepartment; }
            set
            {
                _selectedDepartment = value;

                if (value != null)
                    value.CopyTo(TemporarySelectedDepartment);

                OnPropertyChanged();
            }
        }

        public ICollectionView DepartmentCollectionView { get; }

        public DepartmentViewModel()
        {
            DepartmentCommands = new DepartmentCommands();

            using (WorkDbContext db = new WorkDbContext())
            {
                Departments = new ObservableCollection<Department>();

                foreach (var department in db.Departments)
                {
                    Departments.Add(department);
                }

                Persons = new List<Person>();
                foreach (var person in db.Persons)
                {
                    Persons.Add(person);
                }
            }

            DepartmentCollectionView = CollectionViewSource.GetDefaultView(Departments);
            TemporarySelectedDepartment = new Department();

            if (Application.Current.MainWindow != null)
            {
                int id = (Application.Current.MainWindow.DataContext as MainViewModel)?.NavigationId ?? 0;
                DepartmentCollectionView.MoveCurrentTo(Departments.FirstOrDefault(x => x.Id == id));
            }
        }
    }
}


