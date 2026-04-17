Imports System
Imports UnityEngine

' Token: 0x0200081B RID: 2075
Public Class TrainLevelGhostCannons
	Inherits LevelProperties.Train.Entity

	' Token: 0x06003025 RID: 12325 RVA: 0x001C6D1E File Offset: 0x001C511E
	Public Sub Shoot(cannon As Integer)
		If Not Me.shooting Then
			Return
		End If
		Me.cannon = cannon
		MyBase.animator.SetInteger("Cannon", cannon)
		MyBase.animator.SetTrigger("OnShoot")
	End Sub

	' Token: 0x06003026 RID: 12326 RVA: 0x001C6D54 File Offset: 0x001C5154
	Public Sub [End]()
		Me.shooting = False
	End Sub

	' Token: 0x06003027 RID: 12327 RVA: 0x001C6D60 File Offset: 0x001C5160
	Private Sub ShootAnim()
		If Not Me.shooting Then
			Return
		End If
		AudioManager.Play("train_cannon_shoot")
		Me.emitAudioFromObject.Add("train_cannon_shoot")
		Me.cannonSmoke.Create(Me.cannonRoots(Me.cannon).position)
		Me.ghostPrefab.Create(Me.cannonRoots(Me.cannon).position, MyBase.properties.CurrentState.lollipopGhouls.ghostDelay, MyBase.properties.CurrentState.lollipopGhouls.ghostSpeed, MyBase.properties.CurrentState.lollipopGhouls.ghostAimSpeed, MyBase.properties.CurrentState.lollipopGhouls.ghostHealth, MyBase.properties.CurrentState.lollipopGhouls.skullSpeed)
	End Sub

	' Token: 0x06003028 RID: 12328 RVA: 0x001C6E38 File Offset: 0x001C5238
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.ghostPrefab = Nothing
		Me.cannonSmoke = Nothing
	End Sub

	' Token: 0x040038EE RID: 14574
	<SerializeField()>
	Private cannonSmoke As Effect

	' Token: 0x040038EF RID: 14575
	<SerializeField()>
	Private cannonRoots As Transform()

	' Token: 0x040038F0 RID: 14576
	<SerializeField()>
	Private ghostPrefab As TrainLevelGhostCannonGhost

	' Token: 0x040038F1 RID: 14577
	Private cannon As Integer

	' Token: 0x040038F2 RID: 14578
	Private shooting As Boolean = True
End Class
