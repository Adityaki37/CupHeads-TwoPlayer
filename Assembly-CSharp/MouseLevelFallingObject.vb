Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006ED RID: 1773
Public Class MouseLevelFallingObject
	Inherits AbstractProjectile

	' Token: 0x060025F8 RID: 9720 RVA: 0x0016381C File Offset: 0x00161C1C
	Public Function Create(xPos As Single, properties As LevelProperties.Mouse.Claw) As MouseLevelFallingObject
		Dim vector As Vector2 = New Vector2(-600F, 50F)
		Dim mouseLevelFallingObject As MouseLevelFallingObject = Me.InstantiatePrefab(Of MouseLevelFallingObject)()
		mouseLevelFallingObject.GetComponent(Of Animator)().SetInteger("Pick", Global.UnityEngine.Random.Range(0, 3))
		mouseLevelFallingObject.speed = properties.objectStartingFallSpeed
		mouseLevelFallingObject.gravity = properties.objectGravity
		mouseLevelFallingObject.transform.SetPosition(New Single?(xPos + vector.x), New Single?(CSng(Level.Current.Ceiling) + vector.y), Nothing)
		mouseLevelFallingObject.StartCoroutine(mouseLevelFallingObject.move_cr())
		Return mouseLevelFallingObject
	End Function

	' Token: 0x060025F9 RID: 9721 RVA: 0x001638B8 File Offset: 0x00161CB8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060025FA RID: 9722 RVA: 0x001638D6 File Offset: 0x00161CD6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060025FB RID: 9723 RVA: 0x001638F4 File Offset: 0x00161CF4
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.y > CSng(Level.Current.Ground)
			Me.speed += Me.gravity * CupheadTime.FixedDelta
			MyBase.transform.AddPosition(0F, -Me.speed * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Me.explosionSmall.Create(MyBase.transform.position)
		MyBase.animator.SetTrigger("Death")
		AudioManager.Play("level_mouse_debris_smash")
		Me.emitAudioFromObject.Add("level_mouse_debris_smash")
		Return
	End Function

	' Token: 0x060025FC RID: 9724 RVA: 0x0016390F File Offset: 0x00161D0F
	Private Sub DestroyWood()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060025FD RID: 9725 RVA: 0x0016391C File Offset: 0x00161D1C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosionSmall = Nothing
	End Sub

	' Token: 0x04002E83 RID: 11907
	<SerializeField()>
	Private explosionSmall As Effect

	' Token: 0x04002E84 RID: 11908
	Private gravity As Single

	' Token: 0x04002E85 RID: 11909
	Private speed As Single
End Class
