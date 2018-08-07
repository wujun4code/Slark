using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using LeanCloud;
using LeanCloud.Storage.Internal;

namespace TheMessage
{
    public interface INotifyPropertyUpdated
    {
        event SyncPropertyChangedEventHandler OnPropertyUpdated;
    }

    public interface INotifyCollectionPropertyUpdate
    {
        event SyncPropertyChangedEventHandler OnCollectionPropertyUpdated;
    }

    public enum NotifyCollectionUpdatedAction
    {
        
    }

    public class CollectionPropertyUpdatedEventArgs : PropertyChangedEventArgs
    {
        public CollectionPropertyUpdatedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            
        }
    }

    public class PropertyUpdatedEventArgs : PropertyChangedEventArgs
    {
        public PropertyUpdatedEventArgs(string propertyName, object oldValue, object newValue) : base(propertyName)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public object OldValue { get; private set; }
        public object NewValue { get; private set; }
    }

    public delegate void SyncPropertyChangedEventHandler(object sender, PropertyUpdatedEventArgs args);

    public class TMSyncObject : AVObject, INotifyPropertyUpdated
    {
        public TMSyncObject()
        {

        }

        private SynchronizedEventHandler<PropertyUpdatedEventArgs> propertyChanged =
            new SynchronizedEventHandler<PropertyUpdatedEventArgs>();

        public event SyncPropertyChangedEventHandler OnPropertyUpdated
        {
            add
            {
                propertyChanged.Add(value);
            }
            remove
            {
                propertyChanged.Remove(value);
            }
        }

        protected override void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
        {
            var oldValue = base.GetProperty<T>(propertyName);
            base.SetProperty(value, propertyName);
            var newValue = value;

            propertyChanged.Invoke(this, new PropertyUpdatedEventArgs(propertyName, oldValue, newValue));
        }
    }
}
