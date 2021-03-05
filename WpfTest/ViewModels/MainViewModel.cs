using System.Windows.Input;
using WpfTest.Commands;
using WpfTest.Enums;

namespace WpfTest.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged();
            }
        }

        private int _navigationId;

        public int NavigationId
        {
            get => _navigationId;
            set
            {
                if (value == _navigationId) return;
                _navigationId = value;
                OnPropertyChanged();
            }
        }
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
            UpdateViewCommand = new UpdateViewCommand(this);
            UpdateViewCommand.Execute(ViewType.Order);
        }

    }
}
