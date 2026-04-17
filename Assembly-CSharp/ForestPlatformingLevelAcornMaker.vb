Imports System
Imports UnityEngine

' Token: 0x02000878 RID: 2168
Public Class ForestPlatformingLevelAcornMaker
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x0600325B RID: 12891 RVA: 0x001D5604 File Offset: 0x001D3A04
	Protected Overrides Sub Shoot()
		Dim direction As ForestPlatformingLevelAcorn.Direction
		If Me._target.transform.position.x < MyBase.transform.position.x Then
			direction = ForestPlatformingLevelAcorn.Direction.Left
		Else
			direction = ForestPlatformingLevelAcorn.Direction.Right
		End If
		Me.acornPrefab.Spawn(Me, Me.spawnRoot.transform.position, direction, True)
	End Sub

	' Token: 0x0600325C RID: 12892 RVA: 0x001D5670 File Offset: 0x001D3A70
	Protected Overrides Sub Die()
		If Not Me.isDying Then
			If Me.killAcorns IsNot Nothing Then
				Me.killAcorns()
			End If
			MyBase.animator.SetTrigger("Death")
			Dim componentsInChildren As Collider2D() = MyBase.GetComponentsInChildren(Of Collider2D)()
			For i As Integer = 0 To componentsInChildren.Length - 1
				componentsInChildren(i).enabled = False
			Next
			Me.isDying = True
			Me.explosion.Create(Me.gruntRoot.transform.position)
			Me.gruntSprite.enabled = False
		Else
			MyBase.Die()
		End If
	End Sub

	' Token: 0x0600325D RID: 12893 RVA: 0x001D570C File Offset: 0x001D3B0C
	Private Sub PlayGruntSFX()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
			AudioManager.Play("level_acorn_maker_grunt")
			Me.emitAudioFromObject.Add("level_acorn_maker_grunt")
		End If
	End Sub

	' Token: 0x0600325E RID: 12894 RVA: 0x001D5764 File Offset: 0x001D3B64
	Private Sub PlayIdleSFX()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
			AudioManager.Play("level_acorn_maker_idle")
			Me.emitAudioFromObject.Add("level_acorn_maker_idle")
		End If
	End Sub

	' Token: 0x0600325F RID: 12895 RVA: 0x001D57B9 File Offset: 0x001D3BB9
	Private Sub PlayDeathSFX()
		AudioManager.Play("level_acorn_maker_death")
		Me.emitAudioFromObject.Add("level_acorn_maker_death")
	End Sub

	' Token: 0x06003260 RID: 12896 RVA: 0x001D57D5 File Offset: 0x001D3BD5
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.acornPrefab = Nothing
		Me.explosion = Nothing
	End Sub

	' Token: 0x04003ABA RID: 15034
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F

	' Token: 0x04003ABB RID: 15035
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04003ABC RID: 15036
	<SerializeField()>
	Private gruntRoot As Transform

	' Token: 0x04003ABD RID: 15037
	<SerializeField()>
	Private gruntSprite As SpriteRenderer

	' Token: 0x04003ABE RID: 15038
	<SerializeField()>
	Private acornPrefab As ForestPlatformingLevelAcorn

	' Token: 0x04003ABF RID: 15039
	<SerializeField()>
	Private spawnRoot As Transform

	' Token: 0x04003AC0 RID: 15040
	Private isDying As Boolean

	' Token: 0x04003AC1 RID: 15041
	Public killAcorns As Action
End Class
