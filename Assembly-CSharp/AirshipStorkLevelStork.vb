Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004DB RID: 1243
Public Class AirshipStorkLevelStork
	Inherits LevelProperties.AirshipStork.Entity

	' Token: 0x0600153D RID: 5437 RVA: 0x000BE57F File Offset: 0x000BC97F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600153E RID: 5438 RVA: 0x000BE5B5 File Offset: 0x000BC9B5
	Private Sub Start()
		CupheadLevelCamera.Current.StartFloat(25F, 3F)
	End Sub

	' Token: 0x0600153F RID: 5439 RVA: 0x000BE5CB File Offset: 0x000BC9CB
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001540 RID: 5440 RVA: 0x000BE5E3 File Offset: 0x000BC9E3
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001541 RID: 5441 RVA: 0x000BE5F8 File Offset: 0x000BC9F8
	Public Overrides Sub LevelInit(properties As LevelProperties.AirshipStork)
		MyBase.LevelInit(properties)
		Me.knobSwitch = AirshipLevelKnob.Create(Me.knobSprite.transform)
		AddHandler Me.knobSwitch.OnActivate, AddressOf Me.OnKnobParry
		Dim main As LevelProperties.AirshipStork.Main = properties.CurrentState.main
		Dim position As Vector3 = MyBase.transform.position
		position.y = main.headHeight
		MyBase.transform.position = position
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001542 RID: 5442 RVA: 0x000BE677 File Offset: 0x000BCA77
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001543 RID: 5443 RVA: 0x000BE68E File Offset: 0x000BCA8E
	Private Sub OnKnobParry()
		MyBase.properties.DealDamage(MyBase.properties.CurrentState.main.parryDamage)
		MyBase.StartCoroutine(Me.hurt_cr())
	End Sub

	' Token: 0x06001544 RID: 5444 RVA: 0x000BE6C0 File Offset: 0x000BCAC0
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 5F)
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.spiral_shot_cr())
		MyBase.StartCoroutine(Me.babies_cr())
		Return
	End Function

	' Token: 0x06001545 RID: 5445 RVA: 0x000BE6DC File Offset: 0x000BCADC
	Private Iterator Function hurt_cr() As IEnumerator
		Me.knobSwitch.enabled = False
		Me.knobSprite.GetComponent(Of SpriteRenderer)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.main.pinkDurationOff)
		Me.knobSwitch.enabled = True
		Me.knobSprite.GetComponent(Of SpriteRenderer)().enabled = True
		Return
	End Function

	' Token: 0x06001546 RID: 5446 RVA: 0x000BE6F8 File Offset: 0x000BCAF8
	Private Iterator Function move_cr() As IEnumerator
		Dim offset As Single = 220F
		Dim moveTime As Single = 0F
		Dim p As LevelProperties.AirshipStork.Main = MyBase.properties.CurrentState.main
		Dim leftMovementPattern As String() = p.leftMovementTime.GetRandom().Split(New Char() { ","c })
		Dim pos As Vector3 = MyBase.transform.position
		Parser.FloatTryParse(leftMovementPattern(Me.index), moveTime)
		Dim t As Single = 0F
		While True
			If Me.farRight Then
				While t < moveTime
					MyBase.transform.position -= MyBase.transform.right * (p.movementSpeed * CupheadTime.Delta)
					t += CupheadTime.Delta
					Yield Nothing
				End While
				t = 0F
				Me.farRight = Not Me.farRight
			Else
				While MyBase.transform.position.x < 640F - offset
					pos.x = Mathf.MoveTowards(MyBase.transform.position.x, 640F - offset, p.movementSpeed * CupheadTime.Delta)
					MyBase.transform.position = pos
					Yield Nothing
				End While
				Me.farRight = Not Me.farRight
			End If
			moveTime = (moveTime + 1F) Mod CSng(leftMovementPattern.Length)
		End While
		Return
	End Function

	' Token: 0x06001547 RID: 5447 RVA: 0x000BE714 File Offset: 0x000BCB14
	Private Iterator Function spiral_shot_cr() As IEnumerator
		Dim p As LevelProperties.AirshipStork.SpiralShot = MyBase.properties.CurrentState.spiralShot
		Dim pinkPattern As String() = p.pinkString.GetRandom().Split(New Char() { ","c })
		Dim delayPattern As String() = p.shotDelayString.GetRandom().Split(New Char() { ","c })
		Dim directionPattern As String() = p.spiralDirection.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		Dim pinkIndex As Integer = Global.UnityEngine.Random.Range(0, pinkPattern.Length)
		Dim directionIndex As Integer = Global.UnityEngine.Random.Range(0, directionPattern.Length)
		Dim seconds As Single = 0F
		Dim direction As Integer = 0
		While True
			Yield CupheadTime.WaitForSeconds(Me, seconds)
			Parser.FloatTryParse(delayPattern(delayIndex), seconds)
			Parser.IntTryParse(directionPattern(directionIndex), direction)
			If pinkPattern(pinkIndex)(0) = "R"c Then
				Me.projectile.Create(Me.projectileRoot.transform.position, 0F, p.movementSpeed, p.spiralRate, direction)
			ElseIf pinkPattern(pinkIndex)(0) = "P"c Then
				Me.projectilePink.Create(Me.projectileRoot.transform.position, 0F, p.movementSpeed, p.spiralRate, direction)
			End If
			pinkIndex = (pinkIndex + 1) Mod pinkPattern.Length
			delayIndex = (delayIndex + 1) Mod delayPattern.Length
			directionIndex = (directionIndex + 1) Mod directionPattern.Length
		End While
		Return
	End Function

	' Token: 0x06001548 RID: 5448 RVA: 0x000BE730 File Offset: 0x000BCB30
	Private Iterator Function babies_cr() As IEnumerator
		Dim p As LevelProperties.AirshipStork.Babies = MyBase.properties.CurrentState.babies
		Dim delayPattern As String() = p.babyDelayString.GetRandom().Split(New Char() { ","c })
		Dim index As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		Dim delay As Single = 0F
		While True
			Parser.FloatTryParse(delayPattern(index), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			Dim pos As Vector2 = MyBase.transform.position
			pos.y = CSng(Level.Current.Ground)
			pos.x = CSng(Level.Current.Right)
			Dim baby As AirshipStorkLevelBaby = Global.UnityEngine.[Object].Instantiate(Of AirshipStorkLevelBaby)(Me.babyPrefab)
			baby.Init(p, pos, p.HP)
			Yield Nothing
			index = (index + 1) Mod delayPattern.Length
		End While
		Return
	End Function

	' Token: 0x04001E9A RID: 7834
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x04001E9B RID: 7835
	<SerializeField()>
	Private knobSprite As Transform

	' Token: 0x04001E9C RID: 7836
	<SerializeField()>
	Private projectile As AirshipStorkLevelProjectile

	' Token: 0x04001E9D RID: 7837
	<SerializeField()>
	Private projectilePink As AirshipStorkLevelProjectile

	' Token: 0x04001E9E RID: 7838
	<SerializeField()>
	Private babyPrefab As AirshipStorkLevelBaby

	' Token: 0x04001E9F RID: 7839
	Private farRight As Boolean = True

	' Token: 0x04001EA0 RID: 7840
	Private index As Integer

	' Token: 0x04001EA1 RID: 7841
	Private damageDealer As DamageDealer

	' Token: 0x04001EA2 RID: 7842
	Private damageReceiver As DamageReceiver

	' Token: 0x04001EA3 RID: 7843
	Private knobSwitch As AirshipLevelKnob
End Class
