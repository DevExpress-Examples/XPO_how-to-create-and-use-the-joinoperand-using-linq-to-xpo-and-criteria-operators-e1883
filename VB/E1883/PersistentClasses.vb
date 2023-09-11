Imports DevExpress.Xpo

Namespace DXSample

    Public Class Person
        Inherits XPObject

        Public Sub New(ByVal session As Session)
            MyBase.New(session)
        End Sub

        Private nameField As String

        Public Property Name As String
            Get
                Return nameField
            End Get

            Set(ByVal value As String)
                SetPropertyValue(Of String)("Name", nameField, value)
            End Set
        End Property

        Private ageField As Integer

        Public Property Age As Integer
            Get
                Return ageField
            End Get

            Set(ByVal value As Integer)
                SetPropertyValue(Of Integer)("Age", ageField, value)
            End Set
        End Property
    End Class
End Namespace
