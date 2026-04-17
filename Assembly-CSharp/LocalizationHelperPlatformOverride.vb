Imports System
Imports UnityEngine

' Token: 0x02000921 RID: 2337
Public Class LocalizationHelperPlatformOverride
	Inherits MonoBehaviour

	' Token: 0x060036A7 RID: 13991 RVA: 0x001FA094 File Offset: 0x001F8494
	Public Function HasOverrideForCurrentPlatform(<System.Runtime.InteropServices.OutAttribute()> ByRef newID As Integer) As Boolean
		Dim platform As RuntimePlatform = Application.platform
		For i As Integer = 0 To Me.[overrides].Length - 1
			Dim overrideInfo As LocalizationHelperPlatformOverride.OverrideInfo = Me.[overrides](i)
			If overrideInfo.platform = platform Then
				newID = overrideInfo.id
				Return True
			End If
		Next
		newID = -1
		Return False
	End Function

	' Token: 0x04003ED9 RID: 16089
	Public [overrides] As LocalizationHelperPlatformOverride.OverrideInfo()

	' Token: 0x02000922 RID: 2338
	<Serializable()>
	Public Class OverrideInfo
		' Token: 0x04003EDA RID: 16090
		Public platform As RuntimePlatform

		' Token: 0x04003EDB RID: 16091
		Public id As Integer
	End Class
End Class
