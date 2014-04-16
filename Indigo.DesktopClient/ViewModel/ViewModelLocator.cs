/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocatorTemplate xmlns:vm="clr-namespace:Indigo.DesktopClient.ViewModel"
                                   x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
*/

using System.Linq;

namespace Indigo.DesktopClient.ViewModel
{
    using Microsoft.Practices.ServiceLocation;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;

    using Indigo.DesktopClient.Model;
    using Indigo.DesktopClient.ViewModel.Partial;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<IDataService, Design.DesignDataService>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
            }

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AnalysisViewModel>();
            SimpleIoc.Default.Register<SignInViewModel>();
            SimpleIoc.Default.Register<PenthouseViewModel>();
            SimpleIoc.Default.Register<UnauthorizedViewModel>();
            SimpleIoc.Default.Register<AuthorizedViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<ReferencesViewModel>();
            SimpleIoc.Default.Register<DocumentsViewModel>();
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<ReportsViewModel>();
        }

        /// <summary>
        /// Gets the MainViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the AnalysisViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AnalysisViewModel AnalysisViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AnalysisViewModel>();
            }
        }

        /// <summary>
        /// Gets the SignInViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SignInViewModel SignInViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SignInViewModel>();
            }
        }

        /// <summary>
        /// Gets the PenthouseViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public PenthouseViewModel PenthouseViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PenthouseViewModel>();
            }
        }

        /// <summary>
        /// Gets the UnauthorizedViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public UnauthorizedViewModel UnauthorizedViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UnauthorizedViewModel>();
            }
        }

        /// <summary>
        /// Gets the AuthorizedViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AuthorizedViewModel AuthorizedViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AuthorizedViewModel>();
            }
        }

        /// <summary>
        /// Gets the ProfileViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ProfileViewModel ProfileViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ProfileViewModel>();
            }
        }

        /// <summary>
        /// Gets the ReferencesViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ReferencesViewModel ReferencesViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReferencesViewModel>();
            }
        }

        /// <summary>
        /// Gets the DocumentsViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public DocumentsViewModel DocumentsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DocumentsViewModel>();
            }
        }

        /// <summary>
        /// Gets the UsersViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public UsersViewModel UsersViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UsersViewModel>();
            }
        }

        /// <summary>
        /// Gets the ReportsViewModel property.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public ReportsViewModel ReportsViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReportsViewModel>();
            }
        }

        /// <summary>
        /// Cleans up all the resources.
        /// </summary>
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<SignInViewModel>();
            SimpleIoc.Default.Unregister<PenthouseViewModel>();
            SimpleIoc.Default.Unregister<UnauthorizedViewModel>();
            SimpleIoc.Default.Unregister<AuthorizedViewModel>();
            SimpleIoc.Default.Unregister<ProfileViewModel>();
            SimpleIoc.Default.Unregister<ReferencesViewModel>();
            SimpleIoc.Default.Unregister<DocumentsViewModel>();
            SimpleIoc.Default.Unregister<UsersViewModel>();
            SimpleIoc.Default.Unregister<ReportsViewModel>();

            SimpleIoc.Default.Register<SignInViewModel>();
            SimpleIoc.Default.Register<PenthouseViewModel>();
            SimpleIoc.Default.Register<UnauthorizedViewModel>();
            SimpleIoc.Default.Register<AuthorizedViewModel>();
            SimpleIoc.Default.Register<ProfileViewModel>();
            SimpleIoc.Default.Register<ReferencesViewModel>();
            SimpleIoc.Default.Register<DocumentsViewModel>();
            SimpleIoc.Default.Register<UsersViewModel>();
            SimpleIoc.Default.Register<ReportsViewModel>();
        }
    }
}