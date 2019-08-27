using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CROPS.Storage
{
    public interface IStorageProvider
    {
         Task<string> Save(Stream stream, string fileName);
    }
}
