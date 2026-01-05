using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OxyPlotControls.Controls.Base;

/// <summary>
/// A control that provides visual validation feedback for user input.
/// Displays error messages and visual indicators (red border, icon) when validation fails.
/// </summary>
public class ValidationControl : ContentControl
{
    #region Dependency Properties

    /// <summary>
    /// Identifies the <see cref="HasError"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty HasErrorProperty =
        DependencyProperty.Register(
            nameof(HasError),
            typeof(bool),
            typeof(ValidationControl),
            new PropertyMetadata(false, OnHasErrorChanged));

    /// <summary>
    /// Identifies the <see cref="ErrorMessage"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ErrorMessageProperty =
        DependencyProperty.Register(
            nameof(ErrorMessage),
            typeof(string),
            typeof(ValidationControl),
            new PropertyMetadata(string.Empty));

    /// <summary>
    /// Identifies the <see cref="ErrorBorderBrush"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ErrorBorderBrushProperty =
        DependencyProperty.Register(
            nameof(ErrorBorderBrush),
            typeof(Brush),
            typeof(ValidationControl),
            new PropertyMetadata(Brushes.Red));

    /// <summary>
    /// Identifies the <see cref="ErrorBorderThickness"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty ErrorBorderThicknessProperty =
        DependencyProperty.Register(
            nameof(ErrorBorderThickness),
            typeof(Thickness),
            typeof(ValidationControl),
            new PropertyMetadata(new Thickness(2)));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether the control has a validation error.
    /// </summary>
    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    /// <summary>
    /// Gets or sets the validation error message to display.
    /// </summary>
    public string ErrorMessage
    {
        get => (string)GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    /// <summary>
    /// Gets or sets the brush used for the error border.
    /// </summary>
    public Brush ErrorBorderBrush
    {
        get => (Brush)GetValue(ErrorBorderBrushProperty);
        set => SetValue(ErrorBorderBrushProperty, value);
    }

    /// <summary>
    /// Gets or sets the thickness of the error border.
    /// </summary>
    public Thickness ErrorBorderThickness
    {
        get => (Thickness)GetValue(ErrorBorderThicknessProperty);
        set => SetValue(ErrorBorderThicknessProperty, value);
    }

    #endregion

    #region Constructor

    static ValidationControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            typeof(ValidationControl),
            new FrameworkPropertyMetadata(typeof(ValidationControl)));
    }

    #endregion

    #region Property Change Handlers

    private static void OnHasErrorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ValidationControl control && e.NewValue is bool hasError)
        {
            control.UpdateVisualState(hasError);
        }
    }

    private void UpdateVisualState(bool hasError)
    {
        if (hasError)
        {
            BorderBrush = ErrorBorderBrush;
            BorderThickness = ErrorBorderThickness;
            ToolTip = ErrorMessage;
        }
        else
        {
            BorderBrush = Brushes.Transparent;
            BorderThickness = new Thickness(0);
            ToolTip = null;
        }
    }

    #endregion
}
