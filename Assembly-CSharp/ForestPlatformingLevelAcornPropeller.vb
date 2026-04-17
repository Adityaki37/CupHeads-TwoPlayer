Imports System
Imports UnityEngine

' Token: 0x02000879 RID: 2169
Public Class ForestPlatformingLevelAcornPropeller
	Inherits AbstractPausableComponent

	' Token: 0x06003262 RID: 12898 RVA: 0x001D57F3 File Offset: 0x001D3BF3
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x06003263 RID: 12899 RVA: 0x001D57FC File Offset: 0x001D3BFC
	Public Function Create(position As Vector2, speed As Single) As ForestPlatformingLevelAcornPropeller
		Dim forestPlatformingLevelAcornPropeller As ForestPlatformingLevelAcornPropeller = Me.InstantiatePrefab(Of ForestPlatformingLevelAcornPropeller)()
		forestPlatformingLevelAcornPropeller.transform.position = position
		forestPlatformingLevelAcornPropeller.speed = speed
		Return forestPlatformingLevelAcornPropeller
	End Function

	' Token: 0x06003264 RID: 12900 RVA: 0x001D582C File Offset: 0x001D3C2C
	Private Sub Update()
		MyBase.transform.AddPosition(0F, Me.speed * CupheadTime.Delta, 0F)
		Me.t += CupheadTime.Delta
		If Me.t > 5F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x04003AC2 RID: 15042
	Private speed As Single

	' Token: 0x04003AC3 RID: 15043
	Private t As Single

	' Token: 0x04003AC4 RID: 15044
	Private Const LIFETIME As Single = 5F
End Class
