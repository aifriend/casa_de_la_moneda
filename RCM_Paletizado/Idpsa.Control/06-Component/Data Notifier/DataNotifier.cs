using System;
using System.Collections.Generic;
using Idpsa.Control.Manuals;

namespace Idpsa.Control.Component
{         
    public class DataNotifier<T> : IDataNotifier<T>, IManualFormatProvider, IManualsProvider
    {
        private readonly string _notifierName;
        private readonly int _dataFontSize;
        private readonly bool _hasFormatDirectives;
        private readonly int _senderNameFontSize;

        #region Miembros de IStringDataNotofier

        public DataNotifier(string notifierName)
        {
            if (String.IsNullOrEmpty(notifierName))
                throw new ArgumentException("senderName");
            _notifierName = notifierName;
        }

        public DataNotifier(string notifierName, int senderNameFontSize, int dataFontSize)
        {
            if (String.IsNullOrEmpty(notifierName))
                throw new ArgumentException("senderName");
            _notifierName = notifierName;

            _senderNameFontSize = senderNameFontSize;
            _dataFontSize = dataFontSize;
            _hasFormatDirectives = true;
        }
        
        public string NotifierName
        {
            get { return _notifierName; }
        }

        public void Subscribe(EventHandler<DataEventArgs<T>> newDataHander)
        {
            _newData += newDataHander;
        }

        public bool HasFormatDirectives
        {
            get { return _hasFormatDirectives; }
        }

        public int SenderNameFontSize
        {
            get { return _senderNameFontSize; }
        }

        public int DataFontSize
        {
            get { return _dataFontSize; }
        }

        public void Notify(T data)
        {
            if (_newData != null)
                _newData(this, new DataEventArgs<T>(data));
        }

        #endregion

        private event EventHandler<DataEventArgs<T>> _newData;

        #region Miembros de IManualsProvider

        IEnumerable<Manual> IManualsProvider.GetManualsRepresentations()
        {
            var manual = new Manual(this) { Description = NotifierName };
            return new[] { manual };
        }

        #endregion
    }
}