Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004D8 RID: 1240
Public Class AirshipStorkLevelBaby
	Inherits AbstractCollidableObject

	' Token: 0x17000319 RID: 793
	' (get) Token: 0x0600152F RID: 5423 RVA: 0x000BDFAE File Offset: 0x000BC3AE
	' (set) Token: 0x06001530 RID: 5424 RVA: 0x000BDFB6 File Offset: 0x000BC3B6
	Public Property state As AirshipStorkLevelBaby.State

	' Token: 0x06001531 RID: 5425 RVA: 0x000BDFBF File Offset: 0x000BC3BF
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001532 RID: 5426 RVA: 0x000BDFF5 File Offset: 0x000BC3F5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001533 RID: 5427 RVA: 0x000BE00D File Offset: 0x000BC40D
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F AndAlso Me.state <> AirshipStorkLevelBaby.State.Dying Then
			Me.state = AirshipStorkLevelBaby.State.Dying
			Me.Die()
		End If
	End Sub

	' Token: 0x06001534 RID: 5428 RVA: 0x000BE04B File Offset: 0x000BC44B
	Public Sub Init(properties As LevelProperties.AirshipStork.Babies, pos As Vector2, health As Single)
		Me.properties = properties
		Me.health = health
		MyBase.transform.position = pos
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x06001535 RID: 5429 RVA: 0x000BE079 File Offset: 0x000BC479
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001536 RID: 5430 RVA: 0x000BE090 File Offset: 0x000BC490
	Private Iterator Function jump_cr() As IEnumerator
		Me.state = AirshipStorkLevelBaby.State.Move
		Dim pattern As String() = Me.properties.babyDelayString.GetRandom().Split(New Char() { ","c })
		Dim i As Integer = Global.UnityEngine.Random.Range(0, pattern.Length)
		Dim waitTime As Single = 0F
		Me.onGroundY = CSng(Level.Current.Ground)
		While MyBase.transform.position.x > -740F
			If pattern(i)(0) = "D"c Then
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
			Else
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
				Dim goingUp As Boolean = True
				Dim highJump As Boolean = pattern(i)(0) = "H"c
				Dim velocityY As Single = If((Not highJump), Me.properties.lowVerticalSpeed, Me.properties.highVerticalSpeed)
				Dim speedX As Single = If((Not highJump), Me.properties.lowHorizontalSpeed, Me.properties.highHorizontalSpeed)
				Me.gravity = If((Not highJump), Me.properties.lowGravity, Me.properties.highGravity)
				While goingUp OrElse MyBase.transform.position.y > Me.onGroundY
					velocityY -= Me.gravity * CupheadTime.FixedDelta
					MyBase.transform.AddPosition(-speedX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0F)
					If velocityY < 0F AndAlso goingUp Then
						goingUp = False
					End If
					Yield Nothing
				End While
				MyBase.transform.SetPosition(Nothing, New Single?(Me.onGroundY), Nothing)
			End If
			i = (i + 1) Mod pattern.Length
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06001537 RID: 5431 RVA: 0x000BE0AB File Offset: 0x000BC4AB
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04001E8E RID: 7822
	Private properties As LevelProperties.AirshipStork.Babies

	' Token: 0x04001E8F RID: 7823
	Private damageDealer As DamageDealer

	' Token: 0x04001E90 RID: 7824
	Private damageReceiver As DamageReceiver

	' Token: 0x04001E91 RID: 7825
	Private onGroundY As Single

	' Token: 0x04001E92 RID: 7826
	Private gravity As Single

	' Token: 0x04001E93 RID: 7827
	Private health As Single

	' Token: 0x020004D9 RID: 1241
	Public Enum State
		' Token: 0x04001E95 RID: 7829
		Move
		' Token: 0x04001E96 RID: 7830
		Dying
	End Enum
End Class
