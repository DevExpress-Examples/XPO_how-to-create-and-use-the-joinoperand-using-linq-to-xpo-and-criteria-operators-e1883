Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.Xpo
Namespace DXSample
	Public Class Person
		Inherits XPObject
		Public Sub New(ByVal session As Session)
			MyBase.New(session)
		End Sub

		Private name_Renamed As String
		Public Property Name() As String
			Get
				Return name_Renamed
			End Get
			Set(ByVal value As String)
				SetPropertyValue(Of String)("Name", name_Renamed, value)
			End Set
		End Property

		Private age_Renamed As Integer
		Public Property Age() As Integer
			Get
				Return age_Renamed
			End Get
			Set(ByVal value As Integer)
				SetPropertyValue(Of Integer)("Age", age_Renamed, value)
			End Set
		End Property
	End Class
End Namespace