Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007D1 RID: 2001
Public Class SaltbakerLevelPhaseOneProjectile
	Inherits BasicProjectile

	' Token: 0x06002D6C RID: 11628 RVA: 0x001A8604 File Offset: 0x001A6A04
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.createSparks = True
		MyBase.StartCoroutine(Me.spawn_sparkles_cr())
	End Sub

	' Token: 0x06002D6D RID: 11629 RVA: 0x001A8620 File Offset: 0x001A6A20
	Protected Overridable Function SparksFollow() As Boolean
		Return False
	End Function

	' Token: 0x06002D6E RID: 11630 RVA: 0x001A8624 File Offset: 0x001A6A24
	Private Iterator Function spawn_sparkles_cr() As IEnumerator
		Me.sparkAngle = CSng(Global.UnityEngine.Random.Range(0, 360))
		While Me.createSparks
			Yield CupheadTime.WaitForSeconds(Me, Me.sparkSpawnDelay)
			Dim count As Integer = 1
			If Me.sparkSpawnDelay < CupheadTime.Delta Then
				count = CInt((CupheadTime.Delta / Me.sparkSpawnDelay))
			End If
			For i As Integer = 0 To count - 1
				Dim effect As Effect = Me.sparkEffect.Create(MyBase.transform.position + MathUtils.AngleToDirection(Me.sparkAngle) * Me.sparkDistanceRange.RandomFloat())
				If Me.SparksFollow() Then
					effect.transform.parent = MyBase.transform
				End If
				Me.sparkAngle = (Me.sparkAngle + Me.sparkAngleShiftRange.RandomFloat()) Mod 360F
			Next
		End While
		Return
	End Function

	' Token: 0x06002D6F RID: 11631 RVA: 0x001A8640 File Offset: 0x001A6A40
	Protected Sub HandleShadow(heightOffset As Single, shadowPosOffset As Single)
		Me.shadow.transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground) + shadowPosOffset)
		Dim num As Single = Mathf.InverseLerp(Me.shadowScaleHeightRange.max, Me.shadowScaleHeightRange.min, MyBase.transform.position.y - heightOffset - CSng(Level.Current.Ground))
		Me.shadow.transform.eulerAngles = Vector3.zero
		Me.shadow.transform.localScale = Vector3.Lerp(New Vector3(0.25F, 0.25F), New Vector3(1F, 1F), num)
		Me.shadow.color = New Color(1F, 1F, 1F, Mathf.Lerp(0.25F, 1F, num))
	End Sub

	' Token: 0x040035EF RID: 13807
	<SerializeField()>
	Protected shadow As SpriteRenderer

	' Token: 0x040035F0 RID: 13808
	<SerializeField()>
	Protected shadowScaleHeightRange As MinMax = New MinMax(100F, 500F)

	' Token: 0x040035F1 RID: 13809
	<SerializeField()>
	Protected sparkEffect As Effect

	' Token: 0x040035F2 RID: 13810
	<SerializeField()>
	Private sparkSpawnDelay As Single = 0.15F

	' Token: 0x040035F3 RID: 13811
	<SerializeField()>
	Private sparkAngleShiftRange As MinMax = New MinMax(60F, 300F)

	' Token: 0x040035F4 RID: 13812
	<SerializeField()>
	Private sparkDistanceRange As MinMax = New MinMax(0F, 20F)

	' Token: 0x040035F5 RID: 13813
	Private sparkAngle As Single

	' Token: 0x040035F6 RID: 13814
	Protected createSparks As Boolean = True
End Class
