using System.ComponentModel;

namespace wpf_template.ViewModels
{
    /// <summary>
    /// It's always a good idea to have a ViewModelBase class and inherit all the ViewModels from that. 
    /// Hence we can reuse the code for implementing INotifyPropertyChanged.
    /// The main purpose of using INotifyPropertyChanged is to get notification whenever the property value is changed.
    /// </summary>
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
