using BusinessLogic.Models;
using DataAccess.ApplicationContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class AuditService : IAuditService
    {
        private readonly ApplicationDBContext DBContext;

        public AuditService(ApplicationDBContext context)
        {
            DBContext = context;
        }

        public void CreateAudit(AuditModel model)
        {
            Audit audit = new Audit()
            {

                TableName = model.TableName,
                Action = model.Action,
                RecordId = model.RecordId,
                OldValue = model.OldValue,
                NewValue = model.NewValue,
                AuditDate = DateTime.Now
            };

            DBContext.Audits.Add(audit);
            DBContext.SaveChanges();
        }
    }
}
