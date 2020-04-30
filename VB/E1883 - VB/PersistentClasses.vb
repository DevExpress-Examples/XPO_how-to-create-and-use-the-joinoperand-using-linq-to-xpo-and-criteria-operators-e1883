Imports DevExpress.Xpo
Namespace DXSample
	Public Class Person
		Inherits XPObject

		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

'INSTANT VB NOTE: The field name was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private name_Conflict As String
		Public Property Name() As String
			Get
				Return name_Conflict
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("Name", name_Conflict, value)
			End Set
		End Property

'INSTANT VB NOTE: The field age was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private age_Conflict As Integer
		Public Property Age() As Integer
			Get
				Return age_Conflict
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue(Of Integer)("Age", age_Conflict, value)
			End Set
		End Property
	End Class
End Namespace