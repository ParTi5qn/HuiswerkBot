using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Org.BouncyCastle.Asn1.Mozilla;

namespace HuiswerkBot.Modules
{

    /// <summary>
    /// Contains all necessary data to display Homework retrieved from the database.
    /// </summary>
    internal class Huiswerk
    {
        private int _ID;
        private string _Subject;
        private string _Description;
        private DateTime _Deadline;
        private string _Author;
        private string _AuthorGroup;
        private string _AuthorID;
        private string _AuthorAvatar;
        private DateTime _CreationDate;
        private bool _Finished;
        private DateTime _FinishedDate;

        /// <summary>
        /// <b><code>Type = int</code></b>
        /// Holds the data from the <b>hw_id</b> field.
        /// </summary>
        public object ID {
            get => _ID;
            set => _ID = (int)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>subject</b> field.
        /// </summary>
        public object Subject {
            get => _Subject;
            set => _Subject = (string)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>description</b> field.
        /// </summary>
        public object Description {
            get => _Description;
            set => _Description = (string)value;
        }
        /// <summary>
        /// <b><code>Type = DateTimeHelper</code></b>
        /// Holds the data from the <b>deadline</b> field.
        /// </summary>
        public object Deadline {
            get => _Deadline;
            set => _Deadline = (DateTime)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>author</b> field.
        /// </summary>
        public object Author {
            get => _Author;
            set => _Author = (string)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>author_group</b> field.
        /// </summary>
        public object AuthorGroup {
            get => _AuthorGroup;
            set => _AuthorGroup = (string)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>author_id</b> field.
        /// </summary>
        public object AuthorID {
            get => _AuthorID;
            set => _AuthorID = (string)value;
        }
        /// <summary>
        /// <b><code>Type = string</code></b>
        /// Holds the data from the <b>author_avatar</b> field.
        /// </summary>
        public object AuthorAvatar {
            get => _AuthorAvatar;
            set => _AuthorAvatar = (string)value;
        }
        /// <summary>
        /// <b><code>Type = DateTimeHelper</code></b>
        /// Holds the data from the <b>creation_date</b> field.
        /// </summary>
        public object CreationDate {
            get => _CreationDate;
            set => _CreationDate = (DateTime)value;
        }
        /// <summary>
        /// <b><code>Type = bool</code></b>
        /// Holds the data from the <b>finished</b> field.
        /// </summary>
        public object Finished {
            get => _Finished;
            set => _Finished = (bool)value;
        }
        /// <summary>
        /// <b><code>Type = DateTimeHelper</code></b>
        /// Holds the data from the <b>finished_date</b> field.
        /// </summary>
        public object FinishedDate {
            get => _FinishedDate;
            set => _FinishedDate = (DateTime)value;
        }

        
    }

    /// <summary>
    /// Collection of <b>Huiswerk</b> objects.
    /// This class implements <b>IEnumerable</b> so that it can be used with ForEach syntax.
    /// </summary>
    internal class HuiswerkList : IEnumerable
    {
        private readonly Huiswerk[] _huiswerkData;


        /// <summary>
        /// Creates a new <b>HuiswerkList</b> with the specified <b>size</b>
        /// </summary>
        /// <param name="size"></param>
        public HuiswerkList(int size)
        {
            _huiswerkData = new Huiswerk[size];

            for (int i = 0; i < size; i++)
            {
                _huiswerkData[i] = new Huiswerk();
            }
        }

        public HuiswerkList(Huiswerk[] huiswerkData)
        {
            _huiswerkData = new Huiswerk[huiswerkData.Length];

            for (int i = 0; i < huiswerkData.Length; i++)
            {
                _huiswerkData[i] = huiswerkData[i];
            }
        }

        public Huiswerk this[int index] {
            get
            {
                try
                {
                    return _huiswerkData[index];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                try
                {
                    _huiswerkData[index] = value;
                }
                catch (IndexOutOfRangeException)
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }

        // Implementation for the GetEnumerator method.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator)GetEnumerator();
        }

        public HuiswerkEnum GetEnumerator()
        {
            return new HuiswerkEnum(_huiswerkData);
        }
    }

    internal class HuiswerkEnum : IEnumerator
    {
        public Huiswerk[] _huiswerk;

        // Enumerators start at -1 because so the first MoveNext() call start at 0.
        private int position = -1;

        public HuiswerkEnum(Huiswerk[] list)
        {
            _huiswerk = list;
        }

        // Increment enumerator until the length of the array.
        public bool MoveNext()
        {
            position++;
            return (position < _huiswerk.Length);
        }

        // Reset enumerator
        public void Reset()
        {
            position = -1;
        }

        object IEnumerator.Current {
            get => Current;
        }

        // Returns current <b>Huiswerk<b> object
        public Huiswerk Current {
            get
            {
                try
                {
                    return _huiswerk[position];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }
    }
}