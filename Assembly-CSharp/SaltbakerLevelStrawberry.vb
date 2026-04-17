Imports System
Imports UnityEngine

' Token: 0x020007D6 RID: 2006
Public Class SaltbakerLevelStrawberry
	Inherits SaltbakerLevelPhaseOneProjectile

	' Token: 0x1700040F RID: 1039
	' (get) Token: 0x06002DCB RID: 11723 RVA: 0x001B0778 File Offset: 0x001AEB78
	Protected Overrides ReadOnly Property Direction As Vector3
		Get
			Return-MyBase.transform.up
		End Get
	End Property

	' Token: 0x06002DCC RID: 11724 RVA: 0x001B078C File Offset: 0x001AEB8C
	Public Function Create(position As Vector2, rotation As Single, speed As Single, anim As Integer) As BasicProjectile
		Dim saltbakerLevelStrawberry As SaltbakerLevelStrawberry = CType(MyBase.Create(position, rotation, speed), SaltbakerLevelStrawberry)
		saltbakerLevelStrawberry.anim.Play(anim.ToString())
		Return saltbakerLevelStrawberry
	End Function

	' Token: 0x06002DCD RID: 11725 RVA: 0x001B07C4 File Offset: 0x001AEBC4
	Protected Overrides Sub Move()
		If Not Me.coll.enabled Then
			Return
		End If
		MyBase.Move()
		If MyBase.transform.position.y - 40F < CSng(Level.Current.Ground) Then
			Me.shadow.enabled = False
			Me.Die()
		Else
			MyBase.HandleShadow(40F, 0F)
		End If
	End Sub

	' Token: 0x06002DCE RID: 11726 RVA: 0x001B0838 File Offset: 0x001AEC38
	Protected Overrides Sub Die()
		Me.coll.enabled = False
		Me.createSparks = False
		MyBase.transform.eulerAngles = Vector3.zero
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06002DCF RID: 11727 RVA: 0x001B086D File Offset: 0x001AEC6D
	Private Sub OnDeathAnimationEnd()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003654 RID: 13908
	Private Const OFFSET As Single = 40F

	' Token: 0x04003655 RID: 13909
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04003656 RID: 13910
	<SerializeField()>
	Private coll As Collider2D
End Class
