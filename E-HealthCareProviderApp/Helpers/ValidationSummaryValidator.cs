using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace E_HealthCareProviderApp.Helpers
{
    public static class ValidationSummaryValidator  
    {
        public static DependencyProperty AdornerSiteProperty =
            DependencyProperty.RegisterAttached("AdornerSite", typeof(DependencyObject), typeof(ValidationSummaryValidator),
            new PropertyMetadata(null, OnAdornerSiteChanged));

        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static DependencyObject GetAdornerSite(DependencyObject element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            return (element.GetValue(AdornerSiteProperty) as DependencyObject);
        }

        public static void SetAdornerSite(DependencyObject element, DependencyObject value)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }
            element.SetValue(AdornerSiteProperty, value);
        }

        private static void OnAdornerSiteChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            IErrorViewer errorViewer = e.NewValue as IErrorViewer;
            if (errorViewer != null)
            { 
                errorViewer.SetElement(d);
            }
        }
    }
}
