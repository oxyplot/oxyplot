// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainWindowViewModel.cs" company="OxyPlot">
//   Copyright (c) 2014 OxyPlot contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleBrowser
{
    using ExampleLibrary;
    using OxyPlot;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Data;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _CanTranspose;
        private bool _CanReverse;
        private string _Code;
        private Renderer _Renderer;
        private PlotModel _PlotModel;
        private bool _Transposed;
        private bool _Reversed;
        private IEnumerable<ExampleInfo> examples;
        private ExampleInfo selectedExample;

        public MainWindowViewModel()
        {
            this.Examples = ExampleLibrary.Examples.GetList();
            this.ExamplesView = CollectionViewSource.GetDefaultView(this.Examples.OrderBy(e => e.Category));
            this.ExamplesView.GroupDescriptions.Add(new PropertyGroupDescription("Category"));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public PlotModel ActiveModel => this.CanvasModel ?? this.SkiaModel;

        public bool CanTranspose
        {
            get => this._CanTranspose;
            private set
            {
                this._CanTranspose = value;
                this.RaisePropertyChanged(nameof(this.CanTranspose));
            }
        }

        public bool CanReverse
        {
            get => this._CanReverse;
            private set
            {
                this._CanReverse = value;
                this.RaisePropertyChanged(nameof(this.CanReverse));
            }
        }

        public PlotModel CanvasModel => this.Renderer == Renderer.Canvas ? this._PlotModel : null;

        public string Code
        {
            get => this._Code;
            set
            {
                this._Code = value;
                this.RaisePropertyChanged(nameof(this.Code));
            }
        }

        public IEnumerable<ExampleInfo> Examples
        {
            get => this.examples;
            set
            {
                this.examples = value;
                this.RaisePropertyChanged(nameof(this.Examples));
            }
        }

        public ICollectionView ExamplesView { get; set; }

        public Renderer Renderer
        {
            get => this._Renderer;
            set
            {
                this._Renderer = value;
                this.CoerceRenderer();
                this.RaisePropertyChanged(nameof(this.Renderer));
            }
        }

        private void CoerceRenderer()
        {
            ((IPlotModel)this._PlotModel)?.AttachPlotView(null);
            this.RaisePropertyChanged(nameof(this.SkiaModel));
            this.RaisePropertyChanged(nameof(this.CanvasXamlModel));
            this.RaisePropertyChanged(nameof(this.CanvasModel));
        }

        public IEnumerable<Renderer> Renderers => Enum.GetValues(typeof(Renderer)).Cast<Renderer>();

        public ExampleInfo SelectedExample
        {
            get => this.selectedExample;
            set
            {
                this.selectedExample = value;
                this.CoerceSelectedExample();
                this.RaisePropertyChanged(nameof(this.SelectedExample));
            }
        }

        public PlotModel SkiaModel => this.Renderer == Renderer.SkiaSharp ? this._PlotModel : null;

        public PlotModel CanvasXamlModel => this.Renderer == Renderer.Canvas_XAML ? this._PlotModel : null;

        public bool Transposed
        {
            get => this._Transposed;
            set
            {
                this._Transposed = value;
                this.UpdatePlotModel();
                this.RaisePropertyChanged(nameof(this.Transposed));
            }
        }

        public bool Reversed
        {
            get => this._Reversed;
            set
            {
                this._Reversed = value;
                this.UpdatePlotModel();
                this.RaisePropertyChanged(nameof(this.Reversed));
            }
        }

        protected void RaisePropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void CoerceSelectedExample()
        {
            this.CanTranspose = this.SelectedExample?.IsTransposable == true;
            this.CanReverse = this.SelectedExample?.IsReversible == true;
            this.UpdatePlotModel();
        }

        private void UpdatePlotModel()
        {
            var flags = ExampleInfo.PrepareFlags(
                this.CanTranspose && this.Transposed,
                this.CanReverse && this.Reversed);

            this._PlotModel = this.SelectedExample?.GetModel(flags);
            this.Code = this.SelectedExample?.GetCode(flags);
            this.CoerceRenderer();
        }
    }
}
