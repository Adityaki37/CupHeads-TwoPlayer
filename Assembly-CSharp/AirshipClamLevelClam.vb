Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004CD RID: 1229
Public Class AirshipClamLevelClam
	Inherits LevelProperties.AirshipClam.Entity

	' Token: 0x060014DE RID: 5342 RVA: 0x000BAB03 File Offset: 0x000B8F03
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x060014DF RID: 5343 RVA: 0x000BAB39 File Offset: 0x000B8F39
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x060014E0 RID: 5344 RVA: 0x000BAB46 File Offset: 0x000B8F46
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060014E1 RID: 5345 RVA: 0x000BAB5C File Offset: 0x000B8F5C
	Public Overrides Sub LevelInit(properties As LevelProperties.AirshipClam)
		MyBase.LevelInit(properties)
		Me.pivotPoint.x = CSng((Level.Current.Left + Level.Current.Width / 2))
		Me.pivotPoint.y = CSng(Level.Current.Ground) + CSng(Level.Current.Height) * 0.65F
		Me.pivotPoint.z = 0F
		Me.attacking = False
		Me.clamOut = False
		Me.time = 0F
		Me.idleSpeed = properties.CurrentState.spit.movementSpeedScale
		Me.pShotAttackDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.spit.attackDelayString.Split(New Char() { ","c }).Length)
		Me.barnacleAttackDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.barnacles.attackDelayString.Split(New Char() { ","c }).Length)
		Me.barnacleTypeIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.barnacles.typeString.Split(New Char() { ","c }).Length)
		Me.clamOutShotCountIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.clamOut.shotString.Split(New Char() { ","c }).Length)
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060014E2 RID: 5346 RVA: 0x000BACC0 File Offset: 0x000B90C0
	Private Iterator Function move_cr() As IEnumerator
		While True
			If Not Me.attacking Then
				Dim pos As Vector3 = Me.pivotPoint + Vector3.right * Mathf.Sin(Me.time * Me.idleSpeed) * 300F
				MyBase.transform.position = pos + Vector3.up * Mathf.Sin(Me.time * (Me.idleSpeed * 4F)) * 50F
				Me.time += CupheadTime.Delta
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060014E3 RID: 5347 RVA: 0x000BACDB File Offset: 0x000B90DB
	Public Sub OnSpitStart(callback As Action)
		Me.callback = callback
		MyBase.StartCoroutine(Me.spit_cr())
	End Sub

	' Token: 0x060014E4 RID: 5348 RVA: 0x000BACF4 File Offset: 0x000B90F4
	Private Iterator Function spit_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.spit.initialShotDelay)
		Me.attacking = True
		MyBase.animator.SetTrigger("OnPearlShot")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.spit.preShotDelay)
		Dim target As Vector3 = PlayerManager.GetNext().center + Vector3.up * 50F
		Dim rotation As Single = Vector3.Angle(Vector3.down, MyBase.transform.position - target)
		If target.x > MyBase.transform.position.x Then
			rotation += 270F
		Else
			rotation = 270F - rotation
		End If
		Me.pearlPrefab.Create(Me.spawnPoints(0).position, -rotation, MyBase.properties.CurrentState.spit.bulletSpeed)
		MyBase.animator.SetTrigger("OnPearlShot")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
		Me.attacking = False
		Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.spit.attackDelayString.Split(New Char() { ","c })(Me.pShotAttackDelayIndex)))
		Me.pShotAttackDelayIndex += 1
		If Me.pShotAttackDelayIndex >= MyBase.properties.CurrentState.spit.attackDelayString.Split(New Char() { ","c }).Length Then
			Me.pShotAttackDelayIndex = 0
		End If
		If Me.callback IsNot Nothing Then
			Me.callback()
		End If
		Return
	End Function

	' Token: 0x060014E5 RID: 5349 RVA: 0x000BAD0F File Offset: 0x000B910F
	Public Sub OnBarnaclesStart(callback As Action)
		Me.callback = callback
		MyBase.StartCoroutine(Me.spawnBarnacles_cr())
	End Sub

	' Token: 0x060014E6 RID: 5350 RVA: 0x000BAD28 File Offset: 0x000B9128
	Private Iterator Function spawnBarnacles_cr() As IEnumerator
		Dim parryable As Boolean = False
		Dim duration As Single = MyBase.properties.CurrentState.barnacles.attackDuration.RandomFloat()
		While duration > 0F
			If MyBase.properties.CurrentState.barnacles.typeString.Split(New Char() { ","c })(Me.barnacleTypeIndex)(0) = "P"c Then
				parryable = True
			End If
			Me.barnacleTypeIndex += 1
			If Me.barnacleTypeIndex >= MyBase.properties.CurrentState.barnacles.typeString.Split(New Char() { ","c }).Length Then
				Me.barnacleTypeIndex = 0
			End If
			If Not parryable Then
				Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.barnaclePrefab.gameObject, Me.spawnPoints(0).position, Quaternion.identity)
				gameObject.transform.localScale = Vector3.one * MyBase.properties.CurrentState.barnacles.barnacleScale
				gameObject.GetComponent(Of AirshipClamLevelBarnacle)().InitBarnacle(-1, MyBase.properties)
				gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.barnaclePrefab.gameObject, Me.spawnPoints(1).position, Quaternion.identity)
				gameObject.transform.localScale = Vector3.one * MyBase.properties.CurrentState.barnacles.barnacleScale
				gameObject.GetComponent(Of AirshipClamLevelBarnacle)().InitBarnacle(1, MyBase.properties)
			Else
				Dim gameObject2 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.barnacleParryablePrefab.gameObject, Me.spawnPoints(0).position, Quaternion.identity)
				gameObject2.transform.localScale = Vector3.one * MyBase.properties.CurrentState.barnacles.barnacleScale
				gameObject2.GetComponent(Of AirshipClamLevelBarnacleParryable)().InitBarnacle(-1, MyBase.properties)
				gameObject2 = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.barnacleParryablePrefab.gameObject, Me.spawnPoints(1).position, Quaternion.identity)
				gameObject2.transform.localScale = Vector3.one * MyBase.properties.CurrentState.barnacles.barnacleScale
				gameObject2.GetComponent(Of AirshipClamLevelBarnacleParryable)().InitBarnacle(1, MyBase.properties)
			End If
			Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.barnacles.attackDelayString.Split(New Char() { ","c })(Me.barnacleAttackDelayIndex)))
			duration -= Parser.FloatParse(MyBase.properties.CurrentState.barnacles.attackDelayString.Split(New Char() { ","c })(Me.barnacleAttackDelayIndex))
		End While
		Me.barnacleAttackDelayIndex += 1
		If Me.barnacleAttackDelayIndex >= MyBase.properties.CurrentState.barnacles.attackDelayString.Split(New Char() { ","c }).Length Then
			Me.barnacleAttackDelayIndex = 0
		End If
		If Not Me.clamOut AndAlso Me.callback IsNot Nothing Then
			Me.callback()
		End If
		Return
	End Function

	' Token: 0x060014E7 RID: 5351 RVA: 0x000BAD43 File Offset: 0x000B9143
	Private Sub OnStringShot()
		MyBase.animator.SetBool("OnStringShot", True)
		MyBase.StartCoroutine(Me.clamOut_cr())
	End Sub

	' Token: 0x060014E8 RID: 5352 RVA: 0x000BAD64 File Offset: 0x000B9164
	Private Iterator Function clamOut_cr() As IEnumerator
		Me.damageReceiver.enabled = True
		Dim max As Integer = Parser.IntParse(MyBase.properties.CurrentState.clamOut.shotString.Split(New Char() { ","c })(Me.clamOutShotCountIndex))
		Dim target As Vector3 = PlayerManager.GetNext().center + Vector3.up * 50F
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.clamOut.preShotDelay)
		For i As Integer = 0 To max - 1
			If i <> 0 Then
				Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.clamOut.bulletRepeatDelay)
			End If
			target = PlayerManager.GetNext().center
			target = PlayerManager.GetNext().center + Vector3.up * 50F
			Dim rotation As Single = Vector3.Angle(Vector3.down, MyBase.transform.position - target)
			If target.x > MyBase.transform.position.x Then
				rotation += 270F
			Else
				rotation = 270F - rotation
			End If
			Me.pearlPrefab.Create(Me.spawnPoints(0).position, -rotation, MyBase.properties.CurrentState.spit.bulletSpeed)
		Next
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.clamOut.bulletMainDelay)
		Me.clamOutShotCountIndex += 1
		If Me.clamOutShotCountIndex >= MyBase.properties.CurrentState.clamOut.shotString.Split(New Char() { ","c }).Length Then
			Me.clamOutShotCountIndex = 0
		End If
		MyBase.animator.SetBool("OnStringShot", False)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
		Me.damageReceiver.enabled = False
		If Me.callback IsNot Nothing Then
			Me.callback()
		End If
		Return
	End Function

	' Token: 0x060014E9 RID: 5353 RVA: 0x000BAD7F File Offset: 0x000B917F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x060014EA RID: 5354 RVA: 0x000BADA4 File Offset: 0x000B91A4
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		If Not Me.attacking Then
			Dim component As AirshipClamLevelBarnacleParryable = hit.GetComponent(Of AirshipClamLevelBarnacleParryable)()
			If component IsNot Nothing AndAlso component.parried Then
				MyBase.animator.SetBool("OnBarnacles", False)
				Me.OnStringShot()
				Global.UnityEngine.[Object].Destroy(hit.gameObject)
			End If
		End If
		MyBase.OnCollisionOther(hit, phase)
	End Sub

	' Token: 0x060014EB RID: 5355 RVA: 0x000BAE04 File Offset: 0x000B9204
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04001E3C RID: 7740
	<SerializeField()>
	Private pearlPrefab As BasicProjectile

	' Token: 0x04001E3D RID: 7741
	<SerializeField()>
	Private barnaclePrefab As AirshipClamLevelBarnacle

	' Token: 0x04001E3E RID: 7742
	<SerializeField()>
	Private barnacleParryablePrefab As AirshipClamLevelBarnacleParryable

	' Token: 0x04001E3F RID: 7743
	Private attacking As Boolean

	' Token: 0x04001E40 RID: 7744
	Private idleSpeed As Single

	' Token: 0x04001E41 RID: 7745
	Private pShotAttackDelayIndex As Integer

	' Token: 0x04001E42 RID: 7746
	Private barnacleAttackDelayIndex As Integer

	' Token: 0x04001E43 RID: 7747
	Private barnacleTypeIndex As Integer

	' Token: 0x04001E44 RID: 7748
	Private clamOut As Boolean

	' Token: 0x04001E45 RID: 7749
	Private clamOutShotCountIndex As Integer

	' Token: 0x04001E46 RID: 7750
	Private pivotPoint As Vector3

	' Token: 0x04001E47 RID: 7751
	<SerializeField()>
	Private spawnPoints As Transform()

	' Token: 0x04001E48 RID: 7752
	Private callback As Action

	' Token: 0x04001E49 RID: 7753
	Private time As Single

	' Token: 0x04001E4A RID: 7754
	Private damageDealer As DamageDealer

	' Token: 0x04001E4B RID: 7755
	Private damageReceiver As DamageReceiver
End Class
