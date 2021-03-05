using System;
using System.Windows.Input;
using WpfTest.Enums;
using WpfTest.Models;
using WpfTest.ViewModels;

namespace WpfTest.Commands
{
    public class UpdateViewCommand : ICommand
    {

        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewType viewType = ViewType.None;
            int? id = (parameter as UpdateViewParam)?.Id;

            if (parameter is ViewType paramViewType)
            {
                viewType = paramViewType;
            }

            if (parameter is UpdateViewParam command)
            {
                viewType = command.ViewType;
            }

            if (id != null)
            {
                viewModel.NavigationId = (int)id;
            }
            else
            {
                viewModel.NavigationId = 0;
            }

            switch (viewType)
            {
                case ViewType.Person:
                    viewModel.SelectedViewModel = new PersonViewModel();
                    break;
                case ViewType.Department:
                    viewModel.SelectedViewModel = new DepartmentViewModel();
                    break;
                case ViewType.Order:
                    viewModel.SelectedViewModel = new OrderViewModel();
                    break;
            }
        }
    }
}
