Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005A2 RID: 1442
Public Class DicePalaceBoozeLevelOlive
	Inherits AbstractCollidableObject

	' Token: 0x06001BB6 RID: 7094 RVA: 0x000FC954 File Offset: 0x000FAD54
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.moving = False
		Me.shotCount = 0
		Me.moveCount = 0
		Me.nextPlayerTarget = PlayerId.PlayerOne
		MyBase.Awake()
	End Sub

	' Token: 0x06001BB7 RID: 7095 RVA: 0x000FC9B1 File Offset: 0x000FADB1
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001BB8 RID: 7096 RVA: 0x000FC9C9 File Offset: 0x000FADC9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001BB9 RID: 7097 RVA: 0x000FC9E7 File Offset: 0x000FADE7
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Not Me.isDead Then
			Me.isDead = True
			Me.OnDeath()
		End If
	End Sub

	' Token: 0x06001BBA RID: 7098 RVA: 0x000FCA24 File Offset: 0x000FAE24
	Public Sub InitOlive(properties As LevelProperties.DicePalaceBooze, maxShotCount As Integer, yCoordinates As String, xCoordinates As String)
		Me.properties = properties
		Me.shotCountMax = maxShotCount
		Me.health = CSng(properties.CurrentState.martini.oliveHP)
		Me.yCoordinates = yCoordinates
		Me.xCoordinates = xCoordinates
		Me.moveCountMaxIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.martini.moveString.Split(New Char() { ","c }).Length)
		Me.moveCountMax = Parser.IntParse(properties.CurrentState.martini.moveString.Split(New Char() { ","c })(Me.moveCountMaxIndex))
		Me.verticalCoordinateIndex = Global.UnityEngine.Random.Range(0, yCoordinates.Split(New Char() { ","c }).Length)
		Me.horizontalCoordinateindex = Global.UnityEngine.Random.Range(0, xCoordinates.Split(New Char() { ","c }).Length)
		Me.moveToTarget.y = CSng((Level.Current.Ground + Parser.IntParse(yCoordinates.Split(New Char() { ","c })(Me.verticalCoordinateIndex))))
		Me.moveToTarget.x = CSng((Level.Current.Left + 50 + Parser.IntParse(xCoordinates.Split(New Char() { ","c })(Me.horizontalCoordinateindex))))
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001BBB RID: 7099 RVA: 0x000FCB94 File Offset: 0x000FAF94
	Public Sub ResetOlive(maxShotCount As Integer)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.shotCountMax = maxShotCount
		Me.health = CSng(Me.properties.CurrentState.martini.oliveHP)
		MyBase.StartCoroutine(Me.attack_cr())
		Me.isDead = False
	End Sub

	' Token: 0x06001BBC RID: 7100 RVA: 0x000FCBE4 File Offset: 0x000FAFE4
	Private Iterator Function attack_cr() As IEnumerator
		While True
			If Me.moveCount < Me.moveCountMax Then
				Me.GetNextTarget()
				MyBase.StartCoroutine(Me.move_cr())
				Me.moveCount += 1
				While Me.moving
					Yield Nothing
				End While
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.martini.oliveStopDuration)
			Else
				AudioManager.Play("booze_olive_attack")
				Me.emitAudioFromObject.Add("booze_olive_attack")
				MyBase.animator.SetTrigger("OnAttack")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.martini.oliveHesitateAfterShooting)
			End If
		End While
		Return
	End Function

	' Token: 0x06001BBD RID: 7101 RVA: 0x000FCBFF File Offset: 0x000FAFFF
	Private Sub Shoot()
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06001BBE RID: 7102 RVA: 0x000FCC10 File Offset: 0x000FB010
	Private Iterator Function shoot_cr() As IEnumerator
		Me.moveCount = 0
		Dim target As Vector3 = PlayerManager.GetPlayer(Me.nextPlayerTarget).center - MyBase.transform.position
		Dim proj As BasicProjectile = Me.pimentoPrefab.Create(MyBase.transform.position, 0F, Me.properties.CurrentState.martini.bulletSpeed)
		proj.animator.SetBool("Reverse", Rand.Bool())
		proj.transform.right = target
		Dim enumerator As IEnumerator = proj.GetComponentInChildren(Of Transform)().GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim transform As Transform = CType(obj, Transform)
				transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			Dim disposable3 As IDisposable = disposable
			If disposable2 IsNot Nothing Then
				disposable3.Dispose()
			End If
		End Try
		Me.shotCount += 1
		If Me.shotCount > Me.shotCountMax Then
			proj.SetParryable(True)
			Me.shotCount = 0
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06001BBF RID: 7103 RVA: 0x000FCC2C File Offset: 0x000FB02C
	Private Iterator Function move_cr() As IEnumerator
		Me.moving = True
		While Vector3.Distance(MyBase.transform.position, Me.moveToTarget) > 5F
			Dim dir As Vector3 = (Me.moveToTarget - MyBase.transform.position).normalized
			MyBase.transform.position += dir * Me.properties.CurrentState.martini.oliveSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.moving = False
		Return
	End Function

	' Token: 0x06001BC0 RID: 7104 RVA: 0x000FCC48 File Offset: 0x000FB048
	Private Sub GetNextTarget()
		Me.verticalCoordinateIndex += 1
		If Me.verticalCoordinateIndex >= Me.yCoordinates.Split(New Char() { ","c }).Length Then
			Me.verticalCoordinateIndex = 0
		End If
		Me.horizontalCoordinateindex += 1
		If Me.horizontalCoordinateindex >= Me.xCoordinates.Split(New Char() { ","c }).Length Then
			Me.horizontalCoordinateindex = 0
		End If
		Me.moveToTarget.y = CSng((Level.Current.Ground + Parser.IntParse(Me.yCoordinates.Split(New Char() { ","c })(Me.verticalCoordinateIndex))))
		Me.moveToTarget.x = CSng((Level.Current.Left + 50 + Parser.IntParse(Me.xCoordinates.Split(New Char() { ","c })(Me.horizontalCoordinateindex))))
	End Sub

	' Token: 0x06001BC1 RID: 7105 RVA: 0x000FCD3A File Offset: 0x000FB13A
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.pimentoPrefab = Nothing
	End Sub

	' Token: 0x06001BC2 RID: 7106 RVA: 0x000FCD50 File Offset: 0x000FB150
	Private Sub OnDeath()
		AudioManager.Play("booze_olive_death")
		Me.emitAudioFromObject.Add("booze_olive_death")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		If MyBase.gameObject.activeInHierarchy Then
			MyBase.animator.SetTrigger("OnDeath")
		End If
	End Sub

	' Token: 0x06001BC3 RID: 7107 RVA: 0x000FCDA9 File Offset: 0x000FB1A9
	Private Sub Deactivate()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x040024C2 RID: 9410
	<SerializeField()>
	Private pimentoPrefab As BasicProjectile

	' Token: 0x040024C3 RID: 9411
	Private verticalCoordinateIndex As Integer

	' Token: 0x040024C4 RID: 9412
	Private horizontalCoordinateindex As Integer

	' Token: 0x040024C5 RID: 9413
	Private health As Single

	' Token: 0x040024C6 RID: 9414
	Private shotCount As Integer

	' Token: 0x040024C7 RID: 9415
	Private shotCountMax As Integer

	' Token: 0x040024C8 RID: 9416
	Private moveCount As Integer

	' Token: 0x040024C9 RID: 9417
	Private moveCountMaxIndex As Integer

	' Token: 0x040024CA RID: 9418
	Private moveCountMax As Integer

	' Token: 0x040024CB RID: 9419
	Private isDead As Boolean

	' Token: 0x040024CC RID: 9420
	Private moving As Boolean

	' Token: 0x040024CD RID: 9421
	Private yCoordinates As String

	' Token: 0x040024CE RID: 9422
	Private xCoordinates As String

	' Token: 0x040024CF RID: 9423
	Private nextPlayerTarget As PlayerId

	' Token: 0x040024D0 RID: 9424
	Private moveToTarget As Vector3

	' Token: 0x040024D1 RID: 9425
	Private properties As LevelProperties.DicePalaceBooze

	' Token: 0x040024D2 RID: 9426
	Private damageReceiver As DamageReceiver

	' Token: 0x040024D3 RID: 9427
	Private damageDealer As DamageDealer
End Class
