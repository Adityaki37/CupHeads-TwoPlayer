Imports System
Imports System.Collections.Generic

' Token: 0x02000447 RID: 1095
<Serializable()>
Public Class GitLevelProperties
	' Token: 0x06001057 RID: 4183 RVA: 0x0009F2BA File Offset: 0x0009D6BA
	Public Sub New()
		Me.levels = New List(Of GitLevelProperties.GitLevel)()
	End Sub

	' Token: 0x040019AB RID: 6571
	Public Const UNITY_PATH As String = "/_CUPHEAD/_Generated/git_data.xml"

	' Token: 0x040019AC RID: 6572
	Public Const GIT_TOOLS_PATH As String = "Assets/_CUPHEAD/_Generated/git_data.xml"

	' Token: 0x040019AD RID: 6573
	Public levels As List(Of GitLevelProperties.GitLevel)

	' Token: 0x02000448 RID: 1096
	<Serializable()>
	Public Class GitLevel
		' Token: 0x040019AE RID: 6574
		Public name As String

		' Token: 0x040019AF RID: 6575
		Public levelClassPath As String

		' Token: 0x040019B0 RID: 6576
		Public levelObjectPath As String
	End Class
End Class
