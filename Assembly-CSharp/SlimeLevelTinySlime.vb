Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007E0 RID: 2016
Public Class SlimeLevelTinySlime
	Inherits AbstractCollidableObject

	' Token: 0x06002E1A RID: 11802 RVA: 0x001B2CA4 File Offset: 0x001B10A4
	Public Sub Init(pos As Vector3, properties As LevelProperties.Slime.Tombstone, goingRight As Boolean, parent As SlimeLevelTombstone)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.goingRight = goingRight
		Me.parent = parent
		Dim slimeLevelTombstone As SlimeLevelTombstone = Me.parent
		slimeLevelTombstone.onDeath = CType([Delegate].Combine(slimeLevelTombstone.onDeath, AddressOf Me.Death), Action)
		If goingRight Then
			MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		Else
			MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		End If
	End Sub

	' Token: 0x06002E1B RID: 11803 RVA: 0x001B2D74 File Offset: 0x001B1174
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.Health = Me.properties.tinyHealth
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002E1C RID: 11804 RVA: 0x001B2DCD File Offset: 0x001B11CD
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002E1D RID: 11805 RVA: 0x001B2DE5 File Offset: 0x001B11E5
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002E1E RID: 11806 RVA: 0x001B2E03 File Offset: 0x001B1203
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health < 0F AndAlso Not Me.melted Then
			Me.Melt()
		End If
	End Sub

	' Token: 0x06002E1F RID: 11807 RVA: 0x001B2E39 File Offset: 0x001B1239
	Private Sub Melt()
		Me.melted = True
		MyBase.StartCoroutine(Me.melt_cr())
	End Sub

	' Token: 0x06002E20 RID: 11808 RVA: 0x001B2E50 File Offset: 0x001B1250
	Private Iterator Function melt_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		AudioManager.[Stop]("level_blobrunner")
		AudioManager.Play("level_frogs_tall_firefly_death")
		MyBase.animator.Play("Melt")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Melt", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.tinyMeltDelay)
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("level_blobrunner_reform")
		Me.emitAudioFromObject.Add("level_blobrunner_reform")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.tinyTimeUntilUnmelt)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Unmelt", False, True)
		Me.melted = False
		Me.Health = Me.properties.tinyHealth
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x06002E21 RID: 11809 RVA: 0x001B2E6C File Offset: 0x001B126C
	Private Iterator Function move_cr() As IEnumerator
		Dim sizeX As Single = Me.sprite.bounds.size.x
		Dim sizeY As Single = Me.sprite.bounds.size.y
		Dim left As Single = -640F + sizeX / 2F
		Dim right As Single = 640F - sizeX / 2F
		Dim down As Single = CSng(Level.Current.Ground) + sizeY / 3F
		Dim t As Single = 0F
		Dim time As Single = Me.properties.tinyRunTime
		Dim ease As EaseUtils.EaseType = EaseUtils.EaseType.linear
		Dim speed As Single = 600F
		Dim acceleration As Single = 10F
		Dim endPos As Vector3 = Vector3.zero
		endPos = If((Not Me.goingRight), New Vector3(left, down), New Vector3(right, down))
		While MyBase.transform.position <> endPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, endPos, speed * CupheadTime.Delta)
			speed += acceleration
			Yield Nothing
		End While
		Dim start As Single = 0F
		Dim [end] As Single = 0F
		Me.sprite.sortingLayerName = SpriteLayer.Projectiles.ToString()
		If Me.goingRight Then
			start = MyBase.transform.position.x
			[end] = right
		Else
			start = MyBase.transform.position.x
			[end] = left
		End If
		time = 0F
		While True
			t = 0F
			While t < time
				If Not Me.melted Then
					Dim num As Single = t / time
					MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(ease, start, [end], num)), Nothing, Nothing)
					t += CupheadTime.Delta
				End If
				Yield Nothing
			End While
			MyBase.animator.Play("Turn")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
			MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
			Me.goingRight = Not Me.goingRight
			If Me.goingRight Then
				start = left
				[end] = right
			Else
				start = right
				[end] = left
			End If
			time = Me.properties.tinyRunTime
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002E22 RID: 11810 RVA: 0x001B2E87 File Offset: 0x001B1287
	Private Sub Death()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Melt")
	End Sub

	' Token: 0x06002E23 RID: 11811 RVA: 0x001B2EAB File Offset: 0x001B12AB
	Protected Overrides Sub OnDestroy()
		Dim slimeLevelTombstone As SlimeLevelTombstone = Me.parent
		slimeLevelTombstone.onDeath = CType([Delegate].Remove(slimeLevelTombstone.onDeath, AddressOf Me.Death), Action)
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04003694 RID: 13972
	<SerializeField()>
	Private sprite As SpriteRenderer

	' Token: 0x04003695 RID: 13973
	Private properties As LevelProperties.Slime.Tombstone

	' Token: 0x04003696 RID: 13974
	Private parent As SlimeLevelTombstone

	' Token: 0x04003697 RID: 13975
	Private damageDealer As DamageDealer

	' Token: 0x04003698 RID: 13976
	Private damageReceiver As DamageReceiver

	' Token: 0x04003699 RID: 13977
	Private goingRight As Boolean

	' Token: 0x0400369A RID: 13978
	Private melted As Boolean

	' Token: 0x0400369B RID: 13979
	Private Health As Single
End Class
