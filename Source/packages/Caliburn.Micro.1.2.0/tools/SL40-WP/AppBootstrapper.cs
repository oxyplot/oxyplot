namespace $safeprojectname$ {
    using System;
    using System.Collections.Generic;
    using System.Windows.Controls;
    using Microsoft.Phone.Controls;
    using Caliburn.Micro;

    public class AppBootstrapper : PhoneBootstrapper
    {
        PhoneContainer container;

        protected override void Configure()
        {
            container = new PhoneContainer(RootFrame);

			container.RegisterPhoneServices();
            container.PerRequest<MainPageViewModel>();

            AddCustomConventions();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        static void AddCustomConventions()
        {
            ConventionManager.AddElementConvention<Pivot>(Pivot.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Pivot.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Pivot.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };

            ConventionManager.AddElementConvention<Panorama>(Panorama.ItemsSourceProperty, "SelectedItem", "SelectionChanged").ApplyBinding =
                (viewModelType, path, property, element, convention) => {
                    if (ConventionManager
                        .GetElementConvention(typeof(ItemsControl))
                        .ApplyBinding(viewModelType, path, property, element, convention))
                    {
                        ConventionManager
                            .ConfigureSelectedItem(element, Panorama.SelectedItemProperty, viewModelType, path);
                        ConventionManager
                            .ApplyHeaderTemplate(element, Panorama.HeaderTemplateProperty, viewModelType);
                        return true;
                    }

                    return false;
                };
        }
    }
}