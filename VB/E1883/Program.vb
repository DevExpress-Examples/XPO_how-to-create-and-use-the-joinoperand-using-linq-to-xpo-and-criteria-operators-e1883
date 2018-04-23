Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo
Imports DXSample
Imports DevExpress.Xpo.DB
Imports DevExpress.Data.Filtering
Imports System.Diagnostics
Imports DevExpress.Data.Linq

Namespace E1883
	Friend Class Program
		Shared Sub Main(ByVal args() As String)
			XpoDefault.ConnectionString = MSSqlConnectionProvider.GetConnectionString("(local)", "E1883")
			CreateData()

			' Using constructors
			Dim op1 As CriteriaOperator = New BinaryOperator(CreateExpression("^.Age"), CreateExpression("Age"), BinaryOperatorType.Equal)
			op1 = New JoinOperand("Person", op1, Aggregate.Max, New OperandProperty("Age"))
			op1 = New BinaryOperator(op1, New OperandProperty("Age"), BinaryOperatorType.Equal)

			' Using CriteriaOperator.Parse method
			Dim op2 As CriteriaOperator = CriteriaOperator.Parse("[<Person>][floor(^.Age / 10.0) * 10 = floor(Age / 10.0) * 10].max(Age) = Age")

			Using session As New Session()
				Console.WriteLine("Testing the CriteriaOperator created using constructors")
				Using result1 As New XPCollection(Of Person)(session, op1, New SortProperty("Name", SortingDirection.Ascending))
					CheckResult(result1)
				End Using

				Console.WriteLine("OK")
				Console.WriteLine("Testing the CriteriaOperator created using the CriteriaOperator.Parse method")

				Using result2 As New XPCollection(Of Person)(session, op2, New SortProperty("Name", SortingDirection.Ascending))
					CheckResult(result2)
				End Using

				Console.WriteLine("OK")
				Console.WriteLine("Testing the CriteriaOperator created using LINQ to XPO")

				' Using LINQ to XPO
				Dim persons As New XPQuery(Of Person)(session)
				Dim result = _
					From p In persons _
					Group Join pc In persons On Math.Floor(p.Age / 10.0) * 10 Equals Math.Floor(pc.Age / 10.0) * 10 Into pg = Group _
					Where pg.Max(Function(a) a.Age) = p.Age _
					Order By p.Name _
					Select p
				CheckResult(result.ToList())

				Console.WriteLine("OK")
			End Using
		End Sub

		Private Shared Sub CreateData()
			Using uow As New UnitOfWork()
				If uow.FindObject(Of Person)(Nothing) IsNot Nothing Then
					Return
				End If
                Dim TempPerson As New Person(uow)
                TempPerson.Name = "Maria Anders"
                TempPerson.Age = 29
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Ana Trujillo"
                TempPerson.Age = 43
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Antonio Moreno"
                TempPerson.Age = 22
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Thomas Hardy"
                TempPerson.Age = 41
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Christina Berglund"
                TempPerson.Age = 26
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Hanna Moos"
                TempPerson.Age = 27
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Frederiquee Citeaux"
                TempPerson.Age = 28
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Martin Sommer"
                TempPerson.Age = 29
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Laurence Lebihan"
                TempPerson.Age = 31
                TempPerson.Save()

                TempPerson = New Person(uow)
                TempPerson.Name = "Elizabeth Linkoln"
                TempPerson.Age = 47
                TempPerson.Save()
                uow.CommitChanges()
			End Using
		End Sub

		Private Shared Sub CheckResult(ByVal data As IList(Of Person))
			Debug.Assert(data.Count = 4, "Count")
			Debug.Assert(data(0).Name = "Elizabeth Linkoln", "Elizabeth Linkoln")
			Debug.Assert(data(1).Name = "Laurence Lebihan", "Laurence Lebihan")
			Debug.Assert(data(2).Name = "Maria Anders", "Maria Anders")
			Debug.Assert(data(3).Name = "Martin Sommer", "Martin Sommer")
		End Sub

		Private Shared Function CreateExpression(ByVal propertyName As String) As CriteriaOperator
			Dim op As CriteriaOperator = New BinaryOperator(propertyName, 10, BinaryOperatorType.Divide)
			op = New FunctionOperator(FunctionOperatorType.Floor, op)
			Return New BinaryOperator(op, New OperandValue(10), BinaryOperatorType.Multiply)
		End Function
	End Class
End Namespace
