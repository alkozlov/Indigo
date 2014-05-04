using System;
using Indigo.WinClient.View;

namespace Indigo.WinClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AnalysisViewModel : CommonViewModel
    {
        /// <summary>
        /// Initializes a new instance of the AnalysisViewModel class.
        /// </summary>
        public AnalysisViewModel()
        {
        }

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private String _title = "Проверка текста на уникальность";

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
                RaisePropertyChanged(TitlePropertyName);
            }
        }
    }
}