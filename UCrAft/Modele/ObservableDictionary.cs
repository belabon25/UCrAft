using System.Collections.Generic;
using System.Collections.Specialized;

namespace Modele
{
    /// <summary>
    /// Attention ce dictionnaire observable n'est potentielement pas correct et complet (Il fonctionne neanmoins pour cette application)
    /// </summary>
    /// <typeparam name="Key"></typeparam>
    /// <typeparam name="Value"></typeparam>
    public class ObservableDictionary<Key, Value> : Dictionary<Key, Value>, INotifyCollectionChanged
    {
        public ObservableDictionary(IDictionary<Key, Value> dictionary) : base(dictionary)
        {
        }

        public ObservableDictionary()
        {
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public new Value this[Key key]
        {
            get => base[key];
            set
            {
                if (base.ContainsKey(key))
                {
                    Value oldValue = base[key];
                    base[key] = value;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, new KeyValuePair<Key, Value>(key, value), new KeyValuePair<Key, Value>(key, oldValue)));
                }
                else
                {
                    base[key] = value;
                    CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, value));
                }
            }
        }

        public new void Add(Key key, Value value)
        {
            Value oldValue = base[key];
            base.Add(key, value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<Key, Value>(key, value)));
        }

        public new bool TryAdd(Key key, Value value)
        {
            Value oldValue = base[key];
            bool returnValue = base.TryAdd(key, value);
            if (returnValue)
            {
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new KeyValuePair<Key,Value>(key, value)));
            }

            return returnValue;
        }

        public new void Remove(Key key)
        {
            Value deletedValue;
            base.Remove(key, out deletedValue);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<Key,Value>(key, deletedValue)));
        }

        public new void Remove(Key key, out Value value)
        {
            base.Remove(key, out value);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, new KeyValuePair<Key, Value>(key, value)));
        }

        public new void Clear()
        {
            base.Clear();
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
