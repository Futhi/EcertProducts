using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ApplicationContext
{
    public class Audit
    {
        public int Id { get; set; }
        public string? TableName { get; set; }
        public string? Action { get; set; }
        public string RecordId { get; set; }
        public  string?   OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime AuditDate { get; set; }
    }
}
