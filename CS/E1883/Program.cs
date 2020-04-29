using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DXSample;
using DevExpress.Xpo.DB;
using DevExpress.Data.Filtering;
using System.Diagnostics;
using DevExpress.Data.Linq;

namespace E1883 {
    class Program {
        static void Main(string[] args) {
            XpoDefault.ConnectionString = MSSqlConnectionProvider.GetConnectionString("(local)", "E1883");
            CreateData();

            // Using constructors
            CriteriaOperator op1 = new BinaryOperator(CreateExpression("^.Age"), CreateExpression("Age"), BinaryOperatorType.Equal);
            op1 = new JoinOperand("Person", op1, Aggregate.Max, new OperandProperty("Age"));
            op1 = new BinaryOperator(op1, new OperandProperty("Age"), BinaryOperatorType.Equal);

            // Using CriteriaOperator.Parse method
            CriteriaOperator op2 =
                CriteriaOperator.Parse("[<Person>][floor(^.Age / 10.0) * 10 = floor(Age / 10.0) * 10].max(Age) = Age");

            using (Session session = new Session()) {
                Console.WriteLine("Testing the CriteriaOperator created using constructors");
                using (XPCollection<Person> result1 = 
                    new XPCollection<Person>(session, op1, new SortProperty("Name", SortingDirection.Ascending))) {
                    CheckResult(result1);
                }

                Console.WriteLine("OK");
                Console.WriteLine("Testing the CriteriaOperator created using the CriteriaOperator.Parse method");

                using (XPCollection<Person> result2 =
                    new XPCollection<Person>(session, op2, new SortProperty("Name", SortingDirection.Ascending))) {
                    CheckResult(result2);
                }

                Console.WriteLine("OK");
                Console.WriteLine("Testing the CriteriaOperator created using LINQ to XPO");

                // Using LINQ to XPO
                XPQuery<Person> persons = new XPQuery<Person>(session);
                var result = from p in persons
                             join pc in persons
                             on Math.Floor(p.Age / 10.0) * 10 equals Math.Floor(pc.Age / 10.0) * 10 into pg
                             where pg.Max(a => a.Age) == p.Age
                             orderby p.Name
                             select p;
                CheckResult(result.ToList());

                Console.WriteLine("OK");
            }
        }

        private static void CreateData() {
            using (UnitOfWork uow = new UnitOfWork()) {
                if (uow.FindObject<Person>(null) != null) return;
                new Person(uow) { Name = "Maria Anders", Age = 29 }.Save();
                new Person(uow) { Name = "Ana Trujillo", Age = 43 }.Save();
                new Person(uow) { Name = "Antonio Moreno", Age = 22 }.Save();
                new Person(uow) { Name = "Thomas Hardy", Age = 41 }.Save();
                new Person(uow) { Name = "Christina Berglund", Age = 26 }.Save();
                new Person(uow) { Name = "Hanna Moos", Age = 27 }.Save();
                new Person(uow) { Name = "Frederiquee Citeaux", Age = 28 }.Save();
                new Person(uow) { Name = "Martin Sommer", Age = 29 }.Save();
                new Person(uow) { Name = "Laurence Lebihan", Age = 31 }.Save();
                new Person(uow) { Name = "Elizabeth Linkoln", Age = 47 }.Save();
                uow.CommitChanges();
            }
        }

        private static void CheckResult(IList<Person> data) {
            Debug.Assert(data.Count == 4, "Count");
            Debug.Assert(data[0].Name == "Elizabeth Linkoln", "Elizabeth Linkoln");
            Debug.Assert(data[1].Name == "Laurence Lebihan", "Laurence Lebihan");
            Debug.Assert(data[2].Name == "Maria Anders", "Maria Anders");
            Debug.Assert(data[3].Name == "Martin Sommer", "Martin Sommer");
        }

        private static CriteriaOperator CreateExpression(string propertyName) {
            CriteriaOperator op = new BinaryOperator(propertyName, 10, BinaryOperatorType.Divide);
            op = new FunctionOperator(FunctionOperatorType.Floor, op);
            return new BinaryOperator(op, new OperandValue(10), BinaryOperatorType.Multiply);
        }
    }
}
