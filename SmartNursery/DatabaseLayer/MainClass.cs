using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartNursery.DatabaseLayer
{
    class MainClass 
    {
        DataClassesDataContext db = new DataClassesDataContext();


        public int UserLogin(string userName)
        {
            DataClassesDataContext db = new DataClassesDataContext();
            int emp = 0;
            var id = db.UserLogin(userName);
            foreach (var item in id)
            {
                emp = item.EmplyeeId;
            }
            return emp;

        }
        public object classf()
        {
            db = new DataClassesDataContext();
            return from c in db.ClassTablees
                   join e in db.Employees on c.EmplyeeId equals e.EmplyeeId
                   join g in db.Grades on c.GradeId equals g.GradeId
                   join f in db.Floors on c.FloorId equals f.FloorId
                   select new { c.ClassId,c.ClassDisc,g.GradeDec,f.FloorDesc,e.EmplyeeName };
        }

        public object GradeData()
        {
            db = new DataClassesDataContext();
            var data = db.Grades.ToList();
            return data;
        }
        
        public object Student()
        {
            db = new DataClassesDataContext();
            return from St in db.Students
                   join c in db.ClassTablees on St.ClassId equals c.ClassId
                   select new { St.StudentId,St.StudentName, c.ClassDisc, St.StudentJoiningDate,St.StudentBarCode};
        }

        public object Test()
        {
            db = new DataClassesDataContext();
            return from c in db.Tests
                   join g in db.Grades on c.GradeId equals g.GradeId
                   join f in db.Subjects on c.SubjectId equals f.SubjectId
                   select new { c.TestId, c.TestDesc, g.GradeDec, f.SubjectName, c.MajorDegree, c.MinorDegree };
        }

        public object floor()
        {
            db = new DataClassesDataContext();
            return from St in db.Floors
                   join c in db.Employees on St.EmplyeeId equals c.EmplyeeId
                   select new { St.FloorId, St.FloorDesc, c.EmplyeeName };
        }
        public object Subject()
        {
            db = new DataClassesDataContext();
            var data = db.Subjects.ToList();
            return data;
        }



        public object safe()
        {
            db = new DataClassesDataContext();
            return from S in db.Safes
                   join f in db.SafeFlags on S.SafeFlagId equals f.SafeFlagId
                   where f.SafeFlagId==1
                   select new {S.SafeId, S.SafeName, S.FirstPeriodBalance};
        }
        public object bank()
        {
            db = new DataClassesDataContext();
            return from S in db.Safes
                   join f in db.SafeFlags on S.SafeFlagId equals f.SafeFlagId
                   where f.SafeFlagId == 2
                   select new {S.SafeId ,S.SafeName, S.FirstPeriodBalance, S.AccountNumber };
        }
        public object EmployeeFlag()
      {
          
            db = new DataClassesDataContext();
            var data = db.EmployeeFlags.ToList();
            return data;
        }
        public object Employee()
        {
            db = new DataClassesDataContext();
            return from c in db.Employees
                   join g in db.Employees on c.EmplyeeId equals g.EmplyeeId
                   join f in db.EmployeeFlags on c.EmployeeFlagId equals f.EmployeeFlagId
                   select new { c.EmplyeeId, c.EmplyeeName, g.EmplyeeAddress, c.EmplyeeEmail, c.EmplyeelSalary, c.EmployeeHireDate, c.EmployeeUserName, c.EmplyeeNote, c.EmpoyeeIsActive };
        }
        public object ExpenIncome()
        {
            db = new DataClassesDataContext();
            return from ex in db.ExpensesAndIncomeItems
                   join d in db.DiscountAndAdditions on ex.DiscountAndAdditionId equals d.DiscountAndAdditionId
                   select new { ex.ExpensesAndIncomeItemsId, ex.ExpensesAndIncomeItemsDesc, ex.DiscountAndAdditionId, ex.ExpensesAndIncomeItemsIsActive, d.DiscountAndAdditionDesc };
        }
        public object DailyExpenIncome()
        {
            db = new DataClassesDataContext();
            return from da in db.ExpensesAndIncomeDatas
                   join ex in db.ExpensesAndIncomeItems on da.ExpensesAndIncomeItemsId equals ex.ExpensesAndIncomeItemsId
                   join d in db.DiscountAndAdditions on da.DiscountAndAdditionId equals d.DiscountAndAdditionId
                   join em in db.Employees on da.EmployeeId equals em.EmplyeeId
                   join sa in db.Safes on da.SafeId equals sa.SafeId
                   select new {da.ExpensesAndIncomeDataId, ex.ExpensesAndIncomeItemsDesc, da.ExpensesAndIncomeDataValue, d.DiscountAndAdditionDesc, da.Date, em.EmplyeeName, sa.SafeName };
        }
        public object EmpDeductAndAdd()
        {
            db = new DataClassesDataContext();
            return from a in db.EmpAddAndDeductItems
                   join d in db.DiscountAndAdditions on a.DiscountAndAdditionId equals d.DiscountAndAdditionId
                   select new { a.EmpAddAndDeductItemsId, a.EmpAddAndDeductItemsDesc, d.DiscountAndAdditionDesc, a.IsActive };
        }
    }
}
