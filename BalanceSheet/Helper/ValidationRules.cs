using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BalanceSheet.Helper
{
    public static class ValidationBehavior
    {
        #region Attached Properties

        public static readonly DependencyProperty HasErrorProperty = DependencyProperty.RegisterAttached(
            "HasError",
            typeof(bool),
            typeof(ValidationBehavior),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CoerceHasError));

        private static readonly DependencyProperty HasErrorDescriptorProperty = DependencyProperty.RegisterAttached(
            "HasErrorDescriptor",
            typeof(DependencyPropertyDescriptor),
            typeof(ValidationBehavior));

        #endregion

        private static DependencyPropertyDescriptor GetHasErrorDescriptor(DependencyObject d)
        {
            return (DependencyPropertyDescriptor)d.GetValue(HasErrorDescriptorProperty);
        }

        private static void SetHasErrorDescriptor(DependencyObject d, DependencyPropertyDescriptor value)
        {
            d.SetValue(HasErrorDescriptorProperty, value);
        }

        #region Attached Property Getters and setters

        public static bool GetHasError(DependencyObject d)
        {
            return (bool)d.GetValue(HasErrorProperty);
        }

        public static void SetHasError(DependencyObject d, bool value)
        {
            d.SetValue(HasErrorProperty, value);
        }

        #endregion

        #region CallBacks

        private static object CoerceHasError(DependencyObject d, object baseValue)
        {
            var result = (bool)baseValue;
            if (BindingOperations.IsDataBound(d, HasErrorProperty))
            {
                if (GetHasErrorDescriptor(d) == null)
                {
                    var desc = DependencyPropertyDescriptor.FromProperty(System.Windows.Controls.Validation.HasErrorProperty, d.GetType());
                    desc.AddValueChanged(d, OnHasErrorChanged);
                    SetHasErrorDescriptor(d, desc);
                    result = System.Windows.Controls.Validation.GetHasError(d);
                }
            }
            else
            {
                if (GetHasErrorDescriptor(d) != null)
                {
                    var desc = GetHasErrorDescriptor(d);
                    desc.RemoveValueChanged(d, OnHasErrorChanged);
                    SetHasErrorDescriptor(d, null);
                }
            }
            return result;
        }
        private static void OnHasErrorChanged(object sender, EventArgs e)
        {
            var d = sender as DependencyObject;
            if (d != null)
            {
                d.SetValue(HasErrorProperty, d.GetValue(System.Windows.Controls.Validation.HasErrorProperty));
            }
        }

        #endregion
    }

    public class NotEmptyValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return string.IsNullOrWhiteSpace((value ?? "").ToString())
                ? new ValidationResult(false, "Field is required.")
                : ValidationResult.ValidResult;
        }
    }

    public class NumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double result = 0;
            return !double.TryParse((value ?? "").ToString(), out result)
                ? new ValidationResult(false, "Invalid Entry")
                : ValidationResult.ValidResult;
        }
    }
}
