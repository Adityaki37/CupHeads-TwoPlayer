Imports System
Imports UnityEngine

' Token: 0x020006C6 RID: 1734
Public Class GraveyardLevelPlatformAnimator
	Inherits AbstractMonoBehaviour

	' Token: 0x060024D8 RID: 9432 RVA: 0x00159414 File Offset: 0x00157814
	Private Sub Start()
		For i As Integer = 0 To Me.trailSparks.Length - 1
			Me.trailSparks(i).transform.parent = Nothing
		Next
		For j As Integer = 0 To Me.xPositionBuffer.Length - 1
			Me.xPositionBuffer(j) = MyBase.transform.position.x
		Next
	End Sub

	' Token: 0x060024D9 RID: 9433 RVA: 0x00159484 File Offset: 0x00157884
	Private Sub LateUpdate()
		Me.strikeSpark.transform.position = New Vector3(Me.strikeSpark.transform.position.x, CSng(Level.Current.Ground))
		For i As Integer = Me.xPositionBuffer.Length - 1 To 0 + 1 Step -1
			Me.xPositionBuffer(i) = Me.xPositionBuffer(i - 1)
		Next
		Me.xPositionBuffer(0) = MyBase.transform.position.x
		For j As Integer = 0 To 3 - 1
			Me.trailSparks(j).transform.position = New Vector3(Me.xPositionBuffer(j * 2 + 1), CSng(Level.Current.Ground))
			Me.trailSparks(j).transform.localScale = New Vector3(Mathf.Sign(Me.xPositionBuffer(1) - Me.xPositionBuffer(0)), 1F)
		Next
	End Sub

	' Token: 0x04002D78 RID: 11640
	<SerializeField()>
	Private strikeSpark As SpriteRenderer

	' Token: 0x04002D79 RID: 11641
	<SerializeField()>
	Private trailSparks As SpriteRenderer()

	' Token: 0x04002D7A RID: 11642
	Private xPositionBuffer As Single() = New Single(7) {}
End Class
