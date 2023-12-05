using InternetShopMobileApp.DTOs;
using ReactiveUI;
using SukiUI.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace InternetShopMobileApp.ViewModels
{
    public class AddProductContentViewModel : ReactiveObject, IRoutableViewModel
    {
        private int _stepperIndex = 0;
        public int StepperIndex
        {
            get => _stepperIndex;
            set => this.RaiseAndSetIfChanged(ref _stepperIndex, value);
        }
        public ReactiveCommand<Unit, Unit> AddStepperIndexCommand { get; private set; }
        public ReactiveCommand<Unit, Unit> DecStepperIndexCommand { get; private set; }
        // Reference to IScreen that owns the routable view model.
        public IScreen HostScreen { get; }

        // Unique identifier for the routable view model.
        public string UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        public AddProductContentViewModel(IScreen screen)
        {
            HostScreen = screen;

            AddStepperIndexCommand = ReactiveCommand.Create(AddIndex);
            DecStepperIndexCommand = ReactiveCommand.Create(DecIndex);
        }

        private void AddIndex()
        {
            if (StepperIndex < 4)
                StepperIndex++;
        }

        private void DecIndex()
        {
            if (StepperIndex > 0)
                StepperIndex--;
        }

    }
}
