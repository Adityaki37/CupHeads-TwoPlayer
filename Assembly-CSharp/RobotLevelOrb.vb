Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200077F RID: 1919
Public Class RobotLevelOrb
	Inherits AbstractProjectile

	' Token: 0x06002A24 RID: 10788 RVA: 0x0018A28A File Offset: 0x0018868A
	Protected Overrides Sub Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06002A25 RID: 10789 RVA: 0x0018A2B6 File Offset: 0x001886B6
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.fade_in_cr())
	End Sub

	' Token: 0x06002A26 RID: 10790 RVA: 0x0018A2CB File Offset: 0x001886CB
	Protected Overrides Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		MyBase.Update()
	End Sub

	' Token: 0x06002A27 RID: 10791 RVA: 0x0018A2E9 File Offset: 0x001886E9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002A28 RID: 10792 RVA: 0x0018A307 File Offset: 0x00188707
	Protected Overridable Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Not Me.activeShields Then
			Me.health -= info.damage
			If Me.health <= 0F Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
		End If
	End Sub

	' Token: 0x06002A29 RID: 10793 RVA: 0x0018A344 File Offset: 0x00188744
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		If Me.activeShields Then
			Me.lasers.SetActive(False)
			Me.activeShields = False
			Me.pinkTop.enabled = False
			Me.pinkBottom.enabled = False
			Me.SetParryable(False)
			AudioManager.Play("robot_orb_death")
			Me.emitAudioFromObject.Add("robot_orb_death")
			MyBase.animator.SetTrigger("Continue")
			MyBase.StartCoroutine(Me.slide_in_cr())
		End If
	End Sub

	' Token: 0x06002A2A RID: 10794 RVA: 0x0018A3C8 File Offset: 0x001887C8
	Public Function Create(position As Vector3, offsetAfterSpawn As Vector3) As RobotLevelOrb
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject, position, Quaternion.identity)
		Dim component As RobotLevelOrb = gameObject.GetComponent(Of RobotLevelOrb)()
		component.offsetAfterSpawn = offsetAfterSpawn
		Return component
	End Function

	' Token: 0x06002A2B RID: 10795 RVA: 0x0018A3F8 File Offset: 0x001887F8
	Private Iterator Function fade_in_cr() As IEnumerator
		MyBase.transform.SetScale(New Single?(0.5F), New Single?(0.5F), Nothing)
		Me.pinkTop.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
		Me.pinkBottom.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 0F)
		Dim t As Single = 0F
		Dim time As Single = 0.9F
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0.5F, 1F, t / time)
			MyBase.transform.SetScale(New Single?(val), New Single?(val), Nothing)
			Me.pinkTop.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / time)
			Me.pinkBottom.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, t / time)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002A2C RID: 10796 RVA: 0x0018A414 File Offset: 0x00188814
	Public Sub InitOrb(properties As LevelProperties.Robot)
		MyBase.transform.position += Vector3.left * CSng(properties.CurrentState.orb.orbMovementSpeed) * CupheadTime.Delta
		Me.properties = properties
		If properties.CurrentState.orb.orbShieldIsActive Then
			Me.SetParryable(True)
			Me.activeShields = True
			Me.pinkTop.enabled = True
			Me.pinkBottom.enabled = True
		Else
			Me.activeShields = False
		End If
		Me.health = CSng(properties.CurrentState.orb.orbHP)
		Me.speed = properties.CurrentState.orb.orbMovementSpeed
		MyBase.transform.right = Vector3.left
		MyBase.StartCoroutine(Me.fade_color_cr())
		MyBase.StartCoroutine(Me.lasers_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002A2D RID: 10797 RVA: 0x0018A513 File Offset: 0x00188913
	Public Sub InitChildOrb(speed As Integer, health As Single, activeShields As Boolean)
		Me.speed = speed
		Me.health = health
		Me.activeShields = activeShields
		MyBase.StartCoroutine(Me.move_cr())
		Me.lasers.SetActive(Me.lasers.activeSelf)
	End Sub

	' Token: 0x06002A2E RID: 10798 RVA: 0x0018A550 File Offset: 0x00188950
	Private Iterator Function lasers_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.orb.orbInitalOpenDelay)
		If Not Me.activeShields Then
			Return
		End If
		MyBase.StartCoroutine(Me.slide_out_cr())
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.orb.orbInitialLaserDelay)
		If Not Me.activeShields Then
			Return
		End If
		MyBase.animator.Play("Laser_Start")
		Me.lasers.SetActive(True)
		AudioManager.PlayLoop("robot_orb_spark_loop")
		Yield Nothing
		Return
	End Function

	' Token: 0x06002A2F RID: 10799 RVA: 0x0018A56C File Offset: 0x0018896C
	Private Iterator Function shields_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.orb.orbSpawnDelay)
		Me.activeShields = True
		Return
	End Function

	' Token: 0x06002A30 RID: 10800 RVA: 0x0018A588 File Offset: 0x00188988
	Private Iterator Function move_cr() As IEnumerator
		While True
			If MyBase.transform.position.x < Me.offsetAfterSpawn.x AndAlso MyBase.transform.position.y < Me.offsetAfterSpawn.y Then
				MyBase.transform.position += Vector3.up * CSng(Me.speed) * CupheadTime.Delta * 0.5F
			End If
			MyBase.transform.position += Vector3.left * CSng(Me.speed) * CupheadTime.Delta
			If MyBase.transform.position.x < CSng(Level.Current.Left) - MyBase.GetComponents(Of BoxCollider2D)()(0).size.x / 2F Then
				AudioManager.[Stop]("robot_orb_spark_loop")
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002A31 RID: 10801 RVA: 0x0018A5A4 File Offset: 0x001889A4
	Private Iterator Function slide_out_cr() As IEnumerator
		Dim sizeY As Single = MyBase.GetComponent(Of Collider2D)().bounds.size.y
		Dim localPosTop As Single = Me.top.transform.localPosition.y + sizeY / 4F
		Dim localPosBottom As Single = Me.bottom.transform.localPosition.y - sizeY / 4F
		Dim topPos As Vector3 = Me.top.transform.localPosition
		Dim bottomPos As Vector3 = Me.bottom.transform.localPosition
		Dim time As Single = 0.5F
		Dim t As Single = 0F
		If Me.activeShields Then
			Me.wasActive = True
			AudioManager.Play("robot_orb_spark_start")
			MyBase.animator.Play("Sparks_Start")
		End If
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			topPos.y = Mathf.Lerp(0F, localPosTop, val)
			bottomPos.y = Mathf.Lerp(0F, localPosBottom, val)
			Me.top.transform.localPosition = topPos
			Me.bottom.transform.localPosition = bottomPos
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002A32 RID: 10802 RVA: 0x0018A5C0 File Offset: 0x001889C0
	Private Iterator Function slide_in_cr() As IEnumerator
		Dim topPos As Vector3 = Me.top.transform.localPosition
		Dim bottomPos As Vector3 = Me.bottom.transform.localPosition
		Dim time As Single = 0.5F
		Dim t As Single = 0F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			topPos.y = Mathf.Lerp(topPos.y, 0F, val)
			bottomPos.y = Mathf.Lerp(bottomPos.y, 0F, val)
			Me.top.transform.localPosition = topPos
			Me.bottom.transform.localPosition = bottomPos
			t += CupheadTime.Delta
			Yield Nothing
		End While
		If Me.wasActive Then
			AudioManager.[Stop]("robot_orb_spark_loop")
			MyBase.animator.Play("Sparks_End")
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002A33 RID: 10803 RVA: 0x0018A5DC File Offset: 0x001889DC
	Protected Overridable Iterator Function fade_color_cr() As IEnumerator
		Dim t As Single = 0F
		Dim fadeTime As Single = 0.5F
		While t < fadeTime
			If Not Me.activeShields Then
				Me.top.GetComponent(Of SpriteRenderer)().color = New Color(t / fadeTime, t / fadeTime, t / fadeTime, 1F)
				Me.bottom.GetComponent(Of SpriteRenderer)().color = New Color(t / fadeTime, t / fadeTime, t / fadeTime, 1F)
			Else
				Me.pinkTop.color = New Color(t / fadeTime, t / fadeTime, t / fadeTime, 1F)
				Me.pinkBottom.color = New Color(t / fadeTime, t / fadeTime, t / fadeTime, 1F)
			End If
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x04003302 RID: 13058
	<SerializeField()>
	Private lasers As GameObject

	' Token: 0x04003303 RID: 13059
	<SerializeField()>
	Private top As Transform

	' Token: 0x04003304 RID: 13060
	<SerializeField()>
	Private bottom As Transform

	' Token: 0x04003305 RID: 13061
	<SerializeField()>
	Private pinkTop As SpriteRenderer

	' Token: 0x04003306 RID: 13062
	<SerializeField()>
	Private pinkBottom As SpriteRenderer

	' Token: 0x04003307 RID: 13063
	Private properties As LevelProperties.Robot

	' Token: 0x04003308 RID: 13064
	Private damageReceiver As DamageReceiver

	' Token: 0x04003309 RID: 13065
	Private activeShields As Boolean

	' Token: 0x0400330A RID: 13066
	Private wasActive As Boolean

	' Token: 0x0400330B RID: 13067
	Private health As Single

	' Token: 0x0400330C RID: 13068
	Private speed As Integer

	' Token: 0x0400330D RID: 13069
	Private offsetAfterSpawn As Vector3
End Class
