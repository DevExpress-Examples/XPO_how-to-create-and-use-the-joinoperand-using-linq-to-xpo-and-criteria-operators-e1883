Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.Xpo
Imports DXSample
Imports DevExpress.Xpo.DB
Imports DevExpress.Data.Filtering
Imports System.Diagnostics

Namespace E1883

    Friend Class Program

        Shared Sub Main(ByVal args As String())
            XpoDefault.ConnectionString = MSSqlConnectionProvider.GetConnectionString("(local)", "E1883")
            Call CreateData()
            ' Using constructors
            Dim op1 As CriteriaOperator = New BinaryOperator(CreateExpression("^.Age"), CreateExpression("Age"), BinaryOperatorType.Equal)
            op1 = New JoinOperand("Person", op1, Aggregate.Max, New OperandProperty("Age"))
            op1 = New BinaryOperator(op1, New OperandProperty("Age"), BinaryOperatorType.Equal)
            ' Using CriteriaOperator.Parse method
            Dim op2 As CriteriaOperator = CriteriaOperator.Parse("[<Person>][floor(^.Age / 10.0) * 10 = floor(Age / 10.0) * 10].max(Age) = Age")
            Using session As Session = New Session()
                Console.WriteLine("Testing the CriteriaOperator created using constructors")
                Using result1 As XPCollection(Of Person) = New XPCollection(Of Person)(session, op1, New SortProperty("Name", SortingDirection.Ascending))
                    CheckResult(result1)
                End Using

                Console.WriteLine("OK")
                Console.WriteLine("Testing the CriteriaOperator created using the CriteriaOperator.Parse method")
                Using result2 As XPCollection(Of Person) = New XPCollection(Of Person)(session, op2, New SortProperty("Name", SortingDirection.Ascending))
                    CheckResult(result2)
                End Using

                Console.WriteLine("OK")
                Console.WriteLine("Testing the CriteriaOperator created using LINQ to XPO")
                ' Using LINQ to XPO
                Dim persons As XPQuery(Of Person) = New XPQuery(Of Person)(session)
                Dim result = From p In persons Group Join pc In persons On Math.Floor(p.Age / 10.0) * 10 Equals Math.Floor(pc.Age / 10.0) * 10 Into pg = Group Where pg.Max(Function(a) a.Age) Is p.Age Order By p.Name Select p
                CheckResult(result.ToList())
                Console.WriteLine("OK")
            End Using
        End Sub

        Private Shared Sub CreateData()
            Using uow As UnitOfWork = New UnitOfWork()
                If uow.FindObject(Of Person)(Nothing) IsNot Nothing Then Return
                Call New Person(uow) With {.Name = "Maria Anders", .Age = 29}.Save()
                Call New Person(uow) With {.Name = "Ana Trujillo", .Age = 43}.Save()
                Call New Person(uow) With {.Name = "Antonio Moreno", .Age = 22}.Save()
                Call New Person(uow) With {.Name = "Thomas Hardy", .Age = 41}.Save()
                Call New Person(uow) With {.Name = "Christina Berglund", .Age = 26}.Save()
                Call New Person(uow) With {.Name = "Hanna Moos", .Age = 27}.Save()
                Call New Person(uow) With {.Name = "Frederiquee Citeaux", .Age = 28}.Save()
                Call New Person(uow) With {.Name = "Martin Sommer", .Age = 29}.Save()
                Call New Person(uow) With {.Name = "Laurence Lebihan", .Age = 31}.Save()
                Call New Person(uow) With {.Name = "Elizabeth Linkoln", .Age = 47}.Save()
                uow.CommitChanges()
            End Using
        End Sub

        Private Shared Sub CheckResult(ByVal data As IList(Of Person))
            Call Debug.Assert(data.Count = 4, "Count")
            Call Debug.Assert(Equals(data(0).Name, "Elizabeth Linkoln"), "Elizabeth Linkoln")
            Call Debug.Assert(Equals(data(1).Name, "Laurence Lebihan"), "Laurence Lebihan")
            Call Debug.Assert(Equals(data(2).Name, "Maria Anders"), "Maria Anders")
            Call Debug.Assert(Equals(data(3).Name, "Martin Sommer"), "Martin Sommer")
        End Sub

        Private Shared Function CreateExpression(ByVal propertyName As String) As CriteriaOperator
            Dim op As CriteriaOperator = New BinaryOperator(propertyName, 10, BinaryOperatorType.Divide)
            op = New FunctionOperator(FunctionOperatorType.Floor, op)
            Return New BinaryOperator(op, New OperandValue(10), BinaryOperatorType.Multiply)
        End Function
    End Class
End Namespace
