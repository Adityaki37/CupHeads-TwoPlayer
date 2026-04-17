Imports System
Imports UnityEngine

' Token: 0x0200088D RID: 2189
Public Class TreePlatformingLevelDragonflyShot
	Inherits PlatformingLevelPathMovementEnemy

	' Token: 0x17000440 RID: 1088
	' (get) Token: 0x060032E6 RID: 13030 RVA: 0x001D97BB File Offset: 0x001D7BBB
	' (set) Token: 0x060032E7 RID: 13031 RVA: 0x001D97C3 File Offset: 0x001D7BC3
	Public Property isActivated As Boolean

	' Token: 0x060032E8 RID: 13032 RVA: 0x001D97CC File Offset: 0x001D7BCC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isActivated = False
	End Sub

	' Token: 0x060032E9 RID: 13033 RVA: 0x001D97DB File Offset: 0x001D7BDB
	Protected Overrides Sub Die()
		Me.Deactivate()
	End Sub

	' Token: 0x060032EA RID: 13034 RVA: 0x001D97E3 File Offset: 0x001D7BE3
	Public Sub Activate()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Me.isActivated = True
		Me.PrepareShot()
	End Sub

	' Token: 0x060032EB RID: 13035 RVA: 0x001D980A File Offset: 0x001D7C0A
	Public Sub Deactivate()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.isActivated = False
		MyBase.ResetStartingCondition()
	End Sub

	' Token: 0x060032EC RID: 13036 RVA: 0x001D9831 File Offset: 0x001D7C31
	Private Sub PrepareShot()
		If Rand.Bool() Then
			Me.startPosition = 0F
			Me._direction = PlatformingLevelPathMovementEnemy.Direction.Forward
		Else
			Me.startPosition = 1F
			Me._direction = PlatformingLevelPathMovementEnemy.Direction.Back
		End If
		MyBase.StartFromCustom()
	End Sub
End Class
