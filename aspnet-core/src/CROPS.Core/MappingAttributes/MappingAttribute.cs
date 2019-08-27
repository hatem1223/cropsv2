using System;
using System.Collections.Generic;
using System.Text;

namespace CROPS.MappingAttributes
{
    [System.AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public sealed class MappingAttribute : Attribute
    {
        private readonly string modelName;

        // This is a positional argument
        public MappingAttribute(string modelName)
        {
            this.modelName = modelName;
        }

        public string ModelName
        {
            get { return this.modelName; }
        }
    }
}
