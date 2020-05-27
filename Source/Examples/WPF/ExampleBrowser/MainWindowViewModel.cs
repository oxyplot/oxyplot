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
        private PlotModel _CanvasModel;
        private string _Code;
        private Renderer _Renderer;
        private PlotModel _SkiaModel;
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

        public PlotModel CanvasModel
        {
            get => this._CanvasModel;
            set
            {
                this._CanvasModel = value;
                this.RaisePropertyChanged(nameof(this.CanvasModel));
            }
        }

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
                this.UpdatePlotModels();
                this.RaisePropertyChanged(nameof(this.Renderer));
            }
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

        public PlotModel SkiaModel
        {
            get => this._SkiaModel;
            set
            {
                this._SkiaModel = value;
                this.RaisePropertyChanged(nameof(this.SkiaModel));
            }
        }

        public bool Transposed
        {
            get => this._Transposed;
            set
            {
                this._Transposed = value;
                this.UpdatePlotModels();
                this.RaisePropertyChanged(nameof(this.Transposed));
            }
        }

        public bool Reversed
        {
            get => this._Reversed;
            set
            {
                this._Reversed = value;
                this.UpdatePlotModels();
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
            this.UpdatePlotModels();
        }

        private void UpdatePlotModels()
        {
            var flags = ExampleInfo.PrepareFlags(
                this.CanTranspose && this.Transposed,
                this.CanReverse && this.Reversed);

            var model = this.SelectedExample?.GetModel(flags);
            this.Code = this.SelectedExample?.GetCode(flags);

            switch (this.Renderer)
            {
                case Renderer.Canvas:
                    this.SkiaModel = null;
                    this.CanvasModel = model;
                    break;
                case Renderer.SkiaSharp:
                    this.CanvasModel = null;
                    this.SkiaModel = model;
                    break;
                default:
                    break;
            }
        }
    }
}
