Imports System
Imports UnityEngine

' Token: 0x020006EB RID: 1771
Public Class MouseLevelCherryBombProjectile
	Inherits AbstractProjectile

	' Token: 0x060025EF RID: 9711 RVA: 0x00163580 File Offset: 0x00161980
	Public Function Create(pos As Vector2, velocity As Vector2, gravity As Single, childSpeed As Single) As MouseLevelCherryBombProjectile
		Dim mouseLevelCherryBombProjectile As MouseLevelCherryBombProjectile = Me.InstantiatePrefab(Of MouseLevelCherryBombProjectile)()
		mouseLevelCherryBombProjectile.transform.position = pos
		mouseLevelCherryBombProjectile.velocity = velocity
		mouseLevelCherryBombProjectile.gravity = gravity
		mouseLevelCherryBombProjectile.childSpeed = childSpeed
		mouseLevelCherryBombProjectile.state = MouseLevelCherryBombProjectile.State.Moving
		Return mouseLevelCherryBombProjectile
	End Function

	' Token: 0x060025F0 RID: 9712 RVA: 0x001635C4 File Offset: 0x001619C4
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.state = MouseLevelCherryBombProjectile.State.Moving Then
			If MyBase.transform.position.y < CSng(Level.Current.Ground) + 60F Then
				Me.state = MouseLevelCherryBombProjectile.State.Dead
				MyBase.animator.SetTrigger("OnExplode")
				Return
			End If
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.FixedDelta, Me.velocity.y * CupheadTime.FixedDelta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x060025F1 RID: 9713 RVA: 0x00163674 File Offset: 0x00161A74
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060025F2 RID: 9714 RVA: 0x0016369D File Offset: 0x00161A9D
	Private Sub Explode()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060025F3 RID: 9715 RVA: 0x001636AC File Offset: 0x00161AAC
	Private Sub SpawnChildren()
		Me.cloud.Create(New Vector3(MyBase.transform.position.x + 20F, MyBase.transform.position.y + 200F), New Vector3(0.52F, 0.52F, 0.52F))
		Dim basicProjectile As BasicProjectile = Me.childProjectile.Create(MyBase.transform.position - New Vector3(0F, 40F, 0F), 0F, New Vector2(0.6F, 0.6F), -Me.childSpeed)
		basicProjectile.GetComponent(Of Animator)().SetBool("isRight", False)
		Dim basicProjectile2 As BasicProjectile = Me.childProjectile.Create(MyBase.transform.position - New Vector3(0F, 40F, 0F), 0F, New Vector2(-0.6F, -0.6F), Me.childSpeed)
		basicProjectile2.GetComponent(Of Animator)().SetBool("isRight", True)
	End Sub

	' Token: 0x060025F4 RID: 9716 RVA: 0x001637D1 File Offset: 0x00161BD1
	Protected Overrides Sub Die()
		Me.Explode()
		MyBase.Die()
	End Sub

	' Token: 0x060025F5 RID: 9717 RVA: 0x001637DF File Offset: 0x00161BDF
	Private Sub SoundAnimCherryBomExp()
		AudioManager.Play("level_mouse_cannon_bomb_explode")
		Me.emitAudioFromObject.Add("level_mouse_cannon_bomb_explode")
	End Sub

	' Token: 0x060025F6 RID: 9718 RVA: 0x001637FB File Offset: 0x00161BFB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.cloud = Nothing
		Me.childProjectile = Nothing
	End Sub

	' Token: 0x04002E78 RID: 11896
	Private Const ChildProjectileScale As Single = 0.6F

	' Token: 0x04002E79 RID: 11897
	<SerializeField()>
	Private cloud As Effect

	' Token: 0x04002E7A RID: 11898
	<SerializeField()>
	Private childProjectile As BasicProjectile

	' Token: 0x04002E7B RID: 11899
	Private state As MouseLevelCherryBombProjectile.State

	' Token: 0x04002E7C RID: 11900
	Private velocity As Vector2

	' Token: 0x04002E7D RID: 11901
	Private gravity As Single

	' Token: 0x04002E7E RID: 11902
	Private childSpeed As Single

	' Token: 0x020006EC RID: 1772
	Public Enum State
		' Token: 0x04002E80 RID: 11904
		Init
		' Token: 0x04002E81 RID: 11905
		Moving
		' Token: 0x04002E82 RID: 11906
		Dead
	End Enum
End Class
