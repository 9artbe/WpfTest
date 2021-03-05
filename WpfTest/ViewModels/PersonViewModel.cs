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
    public class PersonViewModel : BaseViewModel
    {
        public ObservableCollection<Person> Persons { get; }
        public ICollectionView PersonCollectionView { get; }
        public List<Department> Departments { get; }
        public PersonCommands PersonCommands { get; }

        public Person TemporarySelectedPerson { get; }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;

                if (value != null)
                    value.CopyTo(TemporarySelectedPerson);

                OnPropertyChanged(nameof(SelectedPerson));
            }
        }
        public PersonViewModel()
        {
            PersonCommands = new PersonCommands();

            using (WorkDbContext db = new WorkDbContext())
            {
                Persons = new ObservableCollection<Person>();
                foreach (var person in db.Persons)
                {
                    Persons.Add(person);
                }

                Departments = new List<Department>();
                foreach (var department in db.Departments)
                {
                    Departments.Add(department);
                }
            }

            PersonCollectionView = CollectionViewSource.GetDefaultView(Persons);
            TemporarySelectedPerson = new Person();

            if (Application.Current.MainWindow != null)
            {
                int id = (Application.Current.MainWindow.DataContext as MainViewModel)?.NavigationId ?? 0;
                PersonCollectionView.MoveCurrentTo(Persons.FirstOrDefault(x => x.Id == id));
            }
        }
    }
}


