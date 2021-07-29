using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoApp.Data.Config
{
    public class TodoStorage
    {
        public const string ConfigKey = "TodoStorage";
        public string BlobStorageConnectionString { get; set; }
    }
}
