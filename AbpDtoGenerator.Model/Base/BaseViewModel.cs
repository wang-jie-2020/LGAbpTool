using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AbpDtoGenerator.Base
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected void InvokePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected T DClone<T>() where T : BaseViewModel
        {
            T t = (T)((object)this);
            T result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, this);
                memoryStream.Seek(0L, SeekOrigin.Begin);
                result = (T)((object)binaryFormatter.Deserialize(memoryStream));
                memoryStream.Close();
            }
            return result;
        }
    }
}