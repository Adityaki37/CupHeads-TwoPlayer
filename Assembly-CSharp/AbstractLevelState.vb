Imports System
Imports UnityEngine

' Token: 0x0200047B RID: 1147
Public MustInherit Class AbstractLevelState(Of PATTERN, STATE_NAMES)
	' Token: 0x060011AC RID: 4524 RVA: 0x00008AC4 File Offset: 0x00006EC4
	Public Sub New(healthTrigger As Single, patterns As PATTERN()(), stateName As STATE_NAMES)
		Me.healthTrigger = Mathf.Clamp(healthTrigger, 0F, 1F)
		Me.patterns = patterns(Global.UnityEngine.Random.Range(0, patterns.Length))
		Me.patternIndex = Global.UnityEngine.Random.Range(0, Me.patterns.Length)
		Me.stateName = stateName
	End Sub

	' Token: 0x170002C0 RID: 704
	' (get) Token: 0x060011AD RID: 4525 RVA: 0x00008B19 File Offset: 0x00006F19
	Public ReadOnly Property NextPattern As PATTERN
		Get
			Me.patternIndex += 1
			If Me.patternIndex >= Me.patterns.Length Then
				Me.patternIndex = 0
			End If
			Return Me.patterns(Me.patternIndex)
		End Get
	End Property

	' Token: 0x170002C1 RID: 705
	' (get) Token: 0x060011AE RID: 4526 RVA: 0x00008B54 File Offset: 0x00006F54
	Public ReadOnly Property PeekNextPattern As PATTERN
		Get
			Dim num As Integer = Me.patternIndex + 1
			If num >= Me.patterns.Length Then
				num = 0
			End If
			Return Me.patterns(num)
		End Get
	End Property

	' Token: 0x170002C2 RID: 706
	' (get) Token: 0x060011AF RID: 4527 RVA: 0x00008B86 File Offset: 0x00006F86
	Public ReadOnly Property CurrentPattern As PATTERN
		Get
			Return Me.patterns(Me.patternIndex)
		End Get
	End Property

	' Token: 0x04001B30 RID: 6960
	Public healthTrigger As Single

	' Token: 0x04001B31 RID: 6961
	Public patterns As PATTERN()

	' Token: 0x04001B32 RID: 6962
	Public stateName As STATE_NAMES

	' Token: 0x04001B33 RID: 6963
	Private patternIndex As Integer
End Class
