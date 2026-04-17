Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004DE RID: 1246
Public Class BaronessLevelBaronessProjectileBunch
	Inherits AbstractProjectile

	' Token: 0x06001560 RID: 5472 RVA: 0x000BF464 File Offset: 0x000BD864
	Public Sub Init(pos As Vector2, velocity As Single, pointAt As Single, properties As LevelProperties.Baroness.BaronessVonBonbon, parent As BaronessLevelCastle)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.pointAt = MathUtils.AngleToDirection(pointAt)
		Me.velocity = velocity
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.KillProjectileBunch
	End Sub

	' Token: 0x06001561 RID: 5473 RVA: 0x000BF4C1 File Offset: 0x000BD8C1
	Private Sub KillProjectileBunch()
		Me.isActive = False
	End Sub

	' Token: 0x06001562 RID: 5474 RVA: 0x000BF4CA File Offset: 0x000BD8CA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isActive = True
	End Sub

	' Token: 0x06001563 RID: 5475 RVA: 0x000BF4D9 File Offset: 0x000BD8D9
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.scale_up_cr())
	End Sub

	' Token: 0x06001564 RID: 5476 RVA: 0x000BF4EE File Offset: 0x000BD8EE
	Protected Overrides Sub Update()
		MyBase.Update()
		If Not Me.isActive Then
			Me.Dying()
		End If
	End Sub

	' Token: 0x06001565 RID: 5477 RVA: 0x000BF507 File Offset: 0x000BD907
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		MyBase.transform.position += Me.pointAt * Me.velocity * CupheadTime.FixedDelta
	End Sub

	' Token: 0x06001566 RID: 5478 RVA: 0x000BF540 File Offset: 0x000BD940
	Private Iterator Function scale_up_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 0.3F
		MyBase.transform.SetScale(New Single?(0F), New Single?(0F), New Single?(0F))
		While t < time
			t += CupheadTime.FixedDelta
			MyBase.transform.SetScale(New Single?(t / time), New Single?(t / time), New Single?(t / time))
			Yield New WaitForFixedUpdate()
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001567 RID: 5479 RVA: 0x000BF55B File Offset: 0x000BD95B
	Private Sub Dying()
		If MyBase.GetComponent(Of SpriteRenderer)() IsNot Nothing Then
			MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		End If
		MyBase.Die()
	End Sub

	' Token: 0x04001EB7 RID: 7863
	Public properties As LevelProperties.Baroness.BaronessVonBonbon

	' Token: 0x04001EB8 RID: 7864
	Private parent As BaronessLevelCastle

	' Token: 0x04001EB9 RID: 7865
	Private velocity As Single

	' Token: 0x04001EBA RID: 7866
	Private isActive As Boolean

	' Token: 0x04001EBB RID: 7867
	Private pointAt As Vector3
End Class
