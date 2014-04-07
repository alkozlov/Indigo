using System;
using GalaSoft.MvvmLight;

namespace Indigo.DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PenthouseViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the PenthouseViewModel class.
        /// </summary>
        public PenthouseViewModel()
        {
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private String _title = "Персональный кабинет";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String Title
        {
            get
            {
                return this._title;
            }

            set
            {
                if (this._title == value)
                {
                    return;
                }

                this._title = value;
                base.RaisePropertyChanged(TitlePropertyName);
            }
        }
    }
}