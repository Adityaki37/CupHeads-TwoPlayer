Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C4 RID: 1476
Public Class DicePalaceFlyingHorseLevelMiniHorse
	Inherits AbstractProjectile

	' Token: 0x17000363 RID: 867
	' (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00107988 File Offset: 0x00105D88
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 20F
		End Get
	End Property

	' Token: 0x06001CC8 RID: 7368 RVA: 0x00107990 File Offset: 0x00105D90
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler Me.jockey.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		Me.damageReceiver = Me.jockey.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.jockeyAnimator = Me.jockey.GetComponent(Of Animator)()
	End Sub

	' Token: 0x06001CC9 RID: 7369 RVA: 0x001079F9 File Offset: 0x00105DF9
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Not Me.jockeyDead Then
			Me.KillJockey()
			Me.jockeyDead = True
		End If
	End Sub

	' Token: 0x06001CCA RID: 7370 RVA: 0x00107A36 File Offset: 0x00105E36
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001CCB RID: 7371 RVA: 0x00107A54 File Offset: 0x00105E54
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001CCC RID: 7372 RVA: 0x00107A74 File Offset: 0x00105E74
	Public Sub Init(position As Vector3, hp As Single, properties As LevelProperties.DicePalaceFlyingHorse.MiniHorses, player As AbstractPlayerController, type As DicePalaceFlyingHorseLevelHorse.MiniHorseType, isPink As Boolean, threeProximity As Single, lane As Integer, backgroundLane As Vector3)
		MyBase.transform.position = position
		Me.hp = hp
		Me.properties = properties
		Me.isPink = isPink
		Me.player = player
		Me.threeProximity = threeProximity
		Me.backgroundLane = backgroundLane
		MyBase.animator.SetInteger("Horse", Global.UnityEngine.Random.Range(1, 3))
		If type <> DicePalaceFlyingHorseLevelHorse.MiniHorseType.One Then
			If type <> DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two Then
				If type = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three Then
					Me.jockeyAnimator.SetInteger("Caddy", 4)
					Me.horseCoroutine = MyBase.StartCoroutine(Me.horse_three_cr())
				End If
			Else
				Me.jockeyAnimator.SetInteger("Caddy", Global.UnityEngine.Random.Range(1, 4))
				Me.horseCoroutine = MyBase.StartCoroutine(Me.horse_two_cr())
			End If
		Else
			Me.jockeyAnimator.SetInteger("Caddy", Global.UnityEngine.Random.Range(1, 4))
		End If
		For i As Integer = 0 To Me.renderers.Length - 1
			Me.renderers(i).sortingOrder = Me.renderers.Length * lane + Me.renderers(i).sortingOrder
		Next
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001CCD RID: 7373 RVA: 0x00107BAC File Offset: 0x00105FAC
	Private Iterator Function move_cr() As IEnumerator
		Dim speed As Single = Me.properties.miniSpeedRange.RandomFloat()
		While MyBase.transform.position.x > -740F
			MyBase.transform.AddPosition(-speed * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		MyBase.transform.position = Me.backgroundLane
		MyBase.transform.localScale = New Vector3(-0.5F, 0.5F, 0.5F)
		Dim horseRenderer As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		horseRenderer.color = ColorUtils.HexToColor("C5C5C5FF")
		horseRenderer.sortingLayerName = "Default"
		horseRenderer.sortingOrder -= 100
		If Me.jockey IsNot Nothing Then
			Dim component As SpriteRenderer = Me.jockey.GetComponent(Of SpriteRenderer)()
			component.material = horseRenderer.material
			component.color = horseRenderer.color
			component.sortingLayerName = "Default"
			component.sortingOrder -= 100
			Me.jockey.GetComponent(Of Collider2D)().enabled = False
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		While MyBase.transform.position.x < 740F
			MyBase.transform.AddPosition(speed * CupheadTime.Delta * 0.5F, 0F, 0F)
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CCE RID: 7374 RVA: 0x00107BC8 File Offset: 0x00105FC8
	Private Iterator Function horse_two_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.miniTwoShotDelayRange.RandomFloat())
		If Not Me.jockeyDead Then
			Me.ShootBullet()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CCF RID: 7375 RVA: 0x00107BE4 File Offset: 0x00105FE4
	Private Sub ShootBullet()
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim vector As Vector3 = Me.player.transform.position - MyBase.transform.position
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		If Me.isPink Then
			Me.pinkBullet.Create(MyBase.transform.position, num, Me.properties.miniTwoBulletSpeed)
		Else
			Me.bullet.Create(MyBase.transform.position, num, Me.properties.miniTwoBulletSpeed)
		End If
	End Sub

	' Token: 0x06001CD0 RID: 7376 RVA: 0x00107CAC File Offset: 0x001060AC
	Private Iterator Function horse_three_cr() As IEnumerator
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim dist As Single = MyBase.transform.position.x - Me.player.transform.position.x
		While dist > Me.threeProximity
			If Me.player Is Nothing OrElse Me.player.IsDead Then
				Me.player = PlayerManager.GetNext()
			End If
			dist = MyBase.transform.position.x - Me.player.transform.position.x
			Yield Nothing
		End While
		If Not Me.jockeyDead Then
			Me.jockeyAnimator.SetTrigger("Attack")
			Yield Me.jockeyAnimator.WaitForAnimationToStart(Me, "CloakedAttack_End", False)
		End If
		If Not Me.jockeyDead Then
			Me.jockey.transform.SetParent(Nothing)
			Me.jockey.transform.GetChild(0).SetParent(MyBase.transform)
		End If
		While Me.jockey.transform.position.y < 360F AndAlso Not Me.jockeyDead
			Me.jockey.transform.AddPosition(0F, Me.properties.miniThreeJockeySpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		If Not Me.jockeyDead Then
			Me.KillJockey()
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CD1 RID: 7377 RVA: 0x00107CC7 File Offset: 0x001060C7
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		MyBase.Die()
	End Sub

	' Token: 0x06001CD2 RID: 7378 RVA: 0x00107CD5 File Offset: 0x001060D5
	Private Sub KillJockey()
		MyBase.StopCoroutine(Me.horseCoroutine)
		Global.UnityEngine.[Object].Destroy(Me.jockey)
	End Sub

	' Token: 0x06001CD3 RID: 7379 RVA: 0x00107CEE File Offset: 0x001060EE
	Public Overrides Sub OnLevelEnd()
	End Sub

	' Token: 0x040025B3 RID: 9651
	<SerializeField()>
	Private bullet As BasicProjectile

	' Token: 0x040025B4 RID: 9652
	<SerializeField()>
	Private pinkBullet As BasicProjectile

	' Token: 0x040025B5 RID: 9653
	<SerializeField()>
	Private jockey As GameObject

	' Token: 0x040025B6 RID: 9654
	<SerializeField()>
	Private renderers As SpriteRenderer()

	' Token: 0x040025B7 RID: 9655
	Private properties As LevelProperties.DicePalaceFlyingHorse.MiniHorses

	' Token: 0x040025B8 RID: 9656
	Private player As AbstractPlayerController

	' Token: 0x040025B9 RID: 9657
	Private damageReceiver As DamageReceiver

	' Token: 0x040025BA RID: 9658
	Private horseCoroutine As Coroutine

	' Token: 0x040025BB RID: 9659
	Private hp As Single

	' Token: 0x040025BC RID: 9660
	Private threeProximity As Single

	' Token: 0x040025BD RID: 9661
	Private isPink As Boolean

	' Token: 0x040025BE RID: 9662
	Private jockeyDead As Boolean

	' Token: 0x040025BF RID: 9663
	Private backgroundLane As Vector3

	' Token: 0x040025C0 RID: 9664
	Private jockeyAnimator As Animator
End Class
