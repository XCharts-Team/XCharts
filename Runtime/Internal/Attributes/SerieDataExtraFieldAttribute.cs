using System;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class SerieDataExtraFieldAttribute : Attribute
    {
        public readonly List<string> fields = new List<string>();

        public SerieDataExtraFieldAttribute()
        { }
        public SerieDataExtraFieldAttribute(string field1)
        {
            AddFiled(field1);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2)
        {
            AddFiled(field1);
            AddFiled(field2);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2, string field3)
        {
            AddFiled(field1);
            AddFiled(field2);
            AddFiled(field3);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4)
        {
            AddFiled(field1);
            AddFiled(field2);
            AddFiled(field3);
            AddFiled(field4);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5)
        {
            AddFiled(field1);
            AddFiled(field2);
            AddFiled(field3);
            AddFiled(field4);
            AddFiled(field5);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6)
        {
            AddFiled(field1);
            AddFiled(field2);
            AddFiled(field3);
            AddFiled(field4);
            AddFiled(field5);
            AddFiled(field6);
        }
        public SerieDataExtraFieldAttribute(string field1, string field2, string field3, string field4, string field5, string field6, string field7)
        {
            AddFiled(field1);
            AddFiled(field2);
            AddFiled(field3);
            AddFiled(field4);
            AddFiled(field5);
            AddFiled(field6);
            AddFiled(field7);
        }

        private void AddFiled(string field)
        {
            if (!SerieData.extraFieldList.Contains(field))
                throw new ArgumentException("SerieData not support field:" + field);
            fields.Add(field);
        }

        public bool Contains(string field)
        {
            return fields.Contains(field);
        }
    }
}