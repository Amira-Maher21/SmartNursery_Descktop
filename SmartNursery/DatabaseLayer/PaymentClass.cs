using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNursery.DatabaseLayer
{
    class PaymentClass : IDisposable
    {
        DataClassesDataContext db = new DataClassesDataContext();

        public void Dispose()
        {
            if (db != null)
            {
                db.Dispose();
                db = null;
            }
        }
        
        public object serialAccount(int id)
        {
            db = new DataClassesDataContext();
            return ((db.Payments.Where(u => u.StudentId == id).Select(a => (int?)a.PaymentSerial).Max() ?? 0) + 1);
        }
        public object BalanceAccount(int id)
        {
            db = new DataClassesDataContext();
            return (db.Payments.Where(u => u.StudentId == id).Select(a => (decimal?)a.PaymentBalance).Sum() ?? 0);
        }
        public object serialTreasury(int id)
        {
            db = new DataClassesDataContext();
            return ((db.Treasuries.Where(u => u.SafeId == id).Select(a => (int?)a.SerialNumber).Max() ?? 0) + 1);
        }
        public object BalanceTreasury(int id)
        {
            db = new DataClassesDataContext();
            return (db.Treasuries.Where(u => u.SafeId == id).Select(a => (decimal?)a.TreasuryBalance).Sum() ?? 0);
        }
    }
}
