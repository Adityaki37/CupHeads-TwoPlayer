Imports System
Imports UnityEngine

' Token: 0x02000A54 RID: 2644
Public Class PlayerSuperChaliceIIISpear
	Inherits AbstractProjectile

	' Token: 0x06003F04 RID: 16132 RVA: 0x00228837 File Offset: 0x00226C37
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06003F05 RID: 16133 RVA: 0x00228839 File Offset: 0x00226C39
	Protected Overrides Sub Start()
		Me._countParryTowardsScore = False
		Me.basePos = MyBase.transform.position
	End Sub

	' Token: 0x06003F06 RID: 16134 RVA: 0x00228853 File Offset: 0x00226C53
	Public Sub DetachFromSuper(p As LevelPlayerController)
		Me.sourcePlayer = p
		AddHandler Me.sourcePlayer.weaponManager.OnSuperStart, AddressOf Me.Die
		MyBase.transform.parent = Nothing
	End Sub

	' Token: 0x06003F07 RID: 16135 RVA: 0x00228885 File Offset: 0x00226C85
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		AudioManager.Play("player_super_chalice_barrage_spearparry")
		MyBase.OnParry(player)
	End Sub

	' Token: 0x06003F08 RID: 16136 RVA: 0x00228898 File Offset: 0x00226C98
	Public Overrides Sub OnParryDie()
		Me.Die()
	End Sub

	' Token: 0x06003F09 RID: 16137 RVA: 0x002288A0 File Offset: 0x00226CA0
	Protected Overrides Sub Die()
		Me.coll.enabled = False
		MyBase.animator.Play("Die")
	End Sub

	' Token: 0x06003F0A RID: 16138 RVA: 0x002288BE File Offset: 0x00226CBE
	Protected Overrides Sub OnDestroy()
		If Me.sourcePlayer IsNot Nothing Then
			RemoveHandler Me.sourcePlayer.weaponManager.OnSuperStart, AddressOf Me.Die
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06003F0B RID: 16139 RVA: 0x002288F4 File Offset: 0x00226CF4
	Protected Overrides Sub FixedUpdate()
	End Sub

	' Token: 0x06003F0C RID: 16140 RVA: 0x002288F8 File Offset: 0x00226CF8
	Protected Overrides Sub Update()
		Me.floatT += CupheadTime.Delta * Me.floatSpeed
		MyBase.transform.position = New Vector3(Me.basePos.x, Me.basePos.y + Mathf.Sin(Me.floatT) * Me.floatAmplitude)
		Me.timer += CupheadTime.Delta
		If Me.timer > 10F Then
			Me.Die()
		End If
		If MyBase.transform.parent Is Nothing AndAlso Me.sourcePlayer Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0400461D RID: 17949
	Private Const EXPIRE_TIME As Single = 10F

	' Token: 0x0400461E RID: 17950
	<SerializeField()>
	Private coll As BoxCollider2D

	' Token: 0x0400461F RID: 17951
	<SerializeField()>
	Private floatAmplitude As Single = 20F

	' Token: 0x04004620 RID: 17952
	<SerializeField()>
	Private floatT As Single

	' Token: 0x04004621 RID: 17953
	<SerializeField()>
	Private floatSpeed As Single = 1F

	' Token: 0x04004622 RID: 17954
	Private basePos As Vector3

	' Token: 0x04004623 RID: 17955
	Private timer As Single

	' Token: 0x04004624 RID: 17956
	Public sourcePlayer As LevelPlayerController
End Class
