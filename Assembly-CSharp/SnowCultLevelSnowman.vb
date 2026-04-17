Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007F7 RID: 2039
Public Class SnowCultLevelSnowman
	Inherits AbstractCollidableObject

	' Token: 0x06002ED4 RID: 11988 RVA: 0x001BA130 File Offset: 0x001B8530
	Public Sub Init(pos As Vector3, properties As LevelProperties.SnowCult.Snowman, goingRight As Boolean)
		MyBase.transform.position = pos
		Me.yPos = MyBase.transform.position.y
		Me.properties = properties
		Me.goingRight = goingRight
		If goingRight Then
			MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
		Else
			MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x06002ED5 RID: 11989 RVA: 0x001BA1E8 File Offset: 0x001B85E8
	Private Sub Start()
		Me.coll = MyBase.GetComponent(Of Collider2D)()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.Health = Me.properties.health
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002ED6 RID: 11990 RVA: 0x001BA24D File Offset: 0x001B864D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002ED7 RID: 11991 RVA: 0x001BA265 File Offset: 0x001B8665
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002ED8 RID: 11992 RVA: 0x001BA283 File Offset: 0x001B8683
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health < 0F AndAlso Not Me.melted Then
			Me.melted = True
			Me.Melt()
		End If
	End Sub

	' Token: 0x06002ED9 RID: 11993 RVA: 0x001BA2C0 File Offset: 0x001B86C0
	Private Sub Melt()
		MyBase.StartCoroutine(Me.melt_cr())
	End Sub

	' Token: 0x06002EDA RID: 11994 RVA: 0x001BA2D0 File Offset: 0x001B86D0
	Private Iterator Function melt_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Melt")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Melt", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.timeUntilUnmelt)
		MyBase.animator.SetTrigger("Continue")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.unmeltLoopTime)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Unmelt", False, True)
		Me.melted = False
		Me.Health = Me.properties.health
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06002EDB RID: 11995 RVA: 0x001BA2EC File Offset: 0x001B86EC
	Private Sub Turn()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
	End Sub

	' Token: 0x06002EDC RID: 11996 RVA: 0x001BA330 File Offset: 0x001B8730
	Private Iterator Function move_cr() As IEnumerator
		Dim sizeX As Single = Me.coll.bounds.size.x
		Dim left As Single = -640F + sizeX / 2F
		Dim right As Single = 640F - sizeX / 2F
		Dim t As Single = 0F
		Dim time As Single = Me.properties.runTime
		Dim ease As EaseUtils.EaseType = EaseUtils.EaseType.linear
		Dim endPos As Vector3 = Vector3.zero
		endPos = If((Not Me.goingRight), New Vector3(left, Me.yPos), New Vector3(right, Me.yPos))
		Dim speed As Single = Vector3.Distance(New Vector3(right, Me.yPos), New Vector3(left, Me.yPos)) / time
		While MyBase.transform.position <> endPos
			While Me.melted
				Yield Nothing
			End While
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, endPos, speed * CupheadTime.Delta)
			Yield Nothing
		End While
		Dim start As Single = 0F
		Dim [end] As Single = 0F
		MyBase.animator.Play("Turn")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
		If Me.goingRight Then
			start = MyBase.transform.position.x
			[end] = left
		Else
			start = MyBase.transform.position.x
			[end] = right
		End If
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
			MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), Nothing, Nothing)
			Me.goingRight = Not Me.goingRight
			If Not Me.goingRight Then
				start = left
				[end] = right
			Else
				start = right
				[end] = left
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002EDD RID: 11997 RVA: 0x001BA34B File Offset: 0x001B874B
	Public Sub Die()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04003785 RID: 14213
	Private properties As LevelProperties.SnowCult.Snowman

	' Token: 0x04003786 RID: 14214
	Private coll As Collider2D

	' Token: 0x04003787 RID: 14215
	Private damageDealer As DamageDealer

	' Token: 0x04003788 RID: 14216
	Private damageReceiver As DamageReceiver

	' Token: 0x04003789 RID: 14217
	Private goingRight As Boolean

	' Token: 0x0400378A RID: 14218
	Private melted As Boolean

	' Token: 0x0400378B RID: 14219
	Private Health As Single

	' Token: 0x0400378C RID: 14220
	Private yPos As Single
End Class
