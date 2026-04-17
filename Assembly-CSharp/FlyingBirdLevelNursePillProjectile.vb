Imports System
Imports UnityEngine

' Token: 0x02000623 RID: 1571
Public Class FlyingBirdLevelNursePillProjectile
	Inherits BasicProjectile

	' Token: 0x06001FF7 RID: 8183 RVA: 0x00125A88 File Offset: 0x00123E88
	Public Sub SetPillColor(color As FlyingBirdLevelNursePillProjectile.PillColor)
		If color = FlyingBirdLevelNursePillProjectile.PillColor.Yellow Then
			Me.yellowPill.SetActive(True)
		ElseIf color = FlyingBirdLevelNursePillProjectile.PillColor.Blue Then
			Me.bluePill.SetActive(True)
		ElseIf color = FlyingBirdLevelNursePillProjectile.PillColor.LightPink Then
			Me.lightPinkPill.SetActive(True)
		Else
			Me.darkPinkPill.SetActive(True)
		End If
	End Sub

	' Token: 0x06001FF8 RID: 8184 RVA: 0x00125AE8 File Offset: 0x00123EE8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002873 RID: 10355
	<SerializeField()>
	Private yellowPill As GameObject

	' Token: 0x04002874 RID: 10356
	<SerializeField()>
	Private bluePill As GameObject

	' Token: 0x04002875 RID: 10357
	<SerializeField()>
	Private lightPinkPill As GameObject

	' Token: 0x04002876 RID: 10358
	<SerializeField()>
	Private darkPinkPill As GameObject

	' Token: 0x02000624 RID: 1572
	Public Enum PillColor
		' Token: 0x04002878 RID: 10360
		Yellow
		' Token: 0x04002879 RID: 10361
		Blue
		' Token: 0x0400287A RID: 10362
		LightPink
		' Token: 0x0400287B RID: 10363
		DarkPink
	End Enum
End Class
