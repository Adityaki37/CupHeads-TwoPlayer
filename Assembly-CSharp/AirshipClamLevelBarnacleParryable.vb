Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004CC RID: 1228
Public Class AirshipClamLevelBarnacleParryable
	Inherits ParrySwitch

	' Token: 0x060014D3 RID: 5331 RVA: 0x000BA6D7 File Offset: 0x000B8AD7
	Protected Overrides Sub Awake()
		Me.parried = False
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.Awake()
	End Sub

	' Token: 0x060014D4 RID: 5332 RVA: 0x000BA6F1 File Offset: 0x000B8AF1
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x060014D5 RID: 5333 RVA: 0x000BA700 File Offset: 0x000B8B00
	Public Sub InitBarnacle(dir As Integer, properties As LevelProperties.AirshipClam)
		Me.onPlayerCollisionDeath = True
		Me.properties = properties
		MyBase.GetComponent(Of SpriteRenderer)().sprite = Me.parraybleBarnacleSprite
		Me.direction = dir
		Me.circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060014D6 RID: 5334 RVA: 0x000BA74C File Offset: 0x000B8B4C
	Private Iterator Function move_cr() As IEnumerator
		Me.velocity = New Vector3(Me.properties.CurrentState.barnacles.initialArcMovementX * CSng(Me.direction), Me.properties.CurrentState.barnacles.initialArcMovementY, 0F)
		While True
			MyBase.transform.position += Me.velocity * CupheadTime.Delta
			If Not Me.parried Then
				Me.velocity.y = Me.velocity.y + Me.properties.CurrentState.barnacles.initialGravity
			Else
				Me.velocity.y = Me.velocity.y + Me.properties.CurrentState.barnacles.parryGravity
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060014D7 RID: 5335 RVA: 0x000BA767 File Offset: 0x000B8B67
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.onPlayerCollisionDeath Then
			Me.damageDealer.DealDamage(hit)
			MyBase.OnCollisionPlayer(hit, phase)
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060014D8 RID: 5336 RVA: 0x000BA794 File Offset: 0x000B8B94
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		Me.velocity.x = 0F
		MyBase.OnCollisionWalls(hit, phase)
	End Sub

	' Token: 0x060014D9 RID: 5337 RVA: 0x000BA7AE File Offset: 0x000B8BAE
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060014DA RID: 5338 RVA: 0x000BA7C4 File Offset: 0x000B8BC4
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		Me.direction = If((MyBase.transform.position.x <= player.center.x), (-1), 1)
		Me.parried = True
		Me.velocity.y = Me.properties.CurrentState.barnacles.parryArcMovementY
		Me.velocity.x = Me.properties.CurrentState.barnacles.parryArcMovementX * CSng(Me.direction)
		MyBase.OnParryPostPause(player)
		Me.circleCollider.enabled = True
		MyBase.StartCoroutine(Me.damageTypes_cr())
	End Sub

	' Token: 0x060014DB RID: 5339 RVA: 0x000BA874 File Offset: 0x000B8C74
	Private Iterator Function damageTypes_cr() As IEnumerator
		Me.damageDealer.SetDamageFlags(False, False, False)
		Me.onPlayerCollisionDeath = False
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.damageDealer.SetDamageFlags(True, False, False)
		Me.onPlayerCollisionDeath = True
		Return
	End Function

	' Token: 0x060014DC RID: 5340 RVA: 0x000BA88F File Offset: 0x000B8C8F
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04001E34 RID: 7732
	Public parried As Boolean

	' Token: 0x04001E35 RID: 7733
	Private onPlayerCollisionDeath As Boolean

	' Token: 0x04001E36 RID: 7734
	Private direction As Integer

	' Token: 0x04001E37 RID: 7735
	Private velocity As Vector3

	' Token: 0x04001E38 RID: 7736
	Private properties As LevelProperties.AirshipClam

	' Token: 0x04001E39 RID: 7737
	Private circleCollider As CircleCollider2D

	' Token: 0x04001E3A RID: 7738
	Private damageDealer As DamageDealer

	' Token: 0x04001E3B RID: 7739
	<SerializeField()>
	Private parraybleBarnacleSprite As Sprite
End Class
